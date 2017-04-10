using DataModel;
using DataObjects;
using System;
using System.Data.Entity;
using Bogus;
using MongoDataObjects;
using System.Linq;
using MongoDriver;
using System.Collections.Generic;
using MongoDB.Bson;
using System.Threading.Tasks;

namespace Populate {
    class Program {
        static void Main(string[] args) {
            Task.Run(async () => {
                await RemoveReports();
                //RemovePartners();
                //RemoveTopicFamilies();
                //RemoveEngineers();
                //SeedEngineers();
                //SeedPartners();
                //SeedTopicFamilies();
                await SeedReports();
            }).Wait();

            
        }

        private static void RemoveEngineers() {
            // Remove existing seeds
            using (var context = new dbpContext()) {
                context.Database.Log = Console.WriteLine;
                // TODO use store procedure to truncate table instead
                foreach (var eng in context.Engineers) {
                    context.Engineers.Remove(eng);
                }
                context.SaveChanges();
            }

            // mongo
            var _repo = new MongoDataContext();
            _repo.RemoveAllEngineers();
        }
        private static void SeedEngineers() {

            // insert fake engineers
            var engs = HelperMethods.getFakeEnginers();
            foreach (var engineer in engs) {
                // sql
                using (var context = new dbpContext()) {
                    context.Database.Log = Console.WriteLine;
                    context.Engineers.Add(engineer);
                    context.SaveChanges();
                }

                // mongo
                // todo make this class level property
                var _repo = new MongoDataContext();
                _repo.InsertEngineer(new MongoEngineer {
                    Name = engineer.Name,
                    Email = engineer.Email
                });
            }
        }

        private static void RemovePartners() {
            // Remove existing seeds
            using (var context = new dbpContext()) {
                context.Database.Log = Console.WriteLine;
                foreach (var partner in context.Partners)
                {
                    context.Partners.Remove(partner);
                }
                context.SaveChanges();
            }

            var _repo = new MongoDataContext();
            _repo.RemoveAllPartners();
        }

        private static void SeedPartners() {

            // insert partners
            var partners = HelperMethods.getFakePartners();
            foreach (var partner in partners) {
                using (var context = new dbpContext()) {
                    context.Database.Log = Console.WriteLine;
                    context.Partners.Add(partner);
                    context.SaveChanges();
                }

                var _repo = new MongoDataContext();
                _repo.InsertPartner(new MongoPartner {
                    Name = partner.Name
                });

            }
        }

        private static void RemoveTopicFamilies() {
            // Remove existing seeds
            using (var context = new dbpContext()) {
                context.Database.Log = Console.WriteLine;
                foreach (var tf in context.TopicFamilies) {
                    context.TopicFamilies.Remove(tf);
                }
                context.SaveChanges();
            }
            using (var context = new dbpContext()) {
                context.Database.Log = Console.WriteLine;
                foreach (var t in context.Topics) {
                    context.Topics.Remove(t);
                }
                context.SaveChanges();
            }
        }

        private static void SeedTopicFamilies() {
            // insert topic families
            var topicFamilies = HelperMethods.getTopicFamilies();
            foreach (var topicFamily in topicFamilies) {
                using (var context = new dbpContext()) {
                    context.Database.Log = Console.WriteLine;
                    context.TopicFamilies.Add(topicFamily);
                    context.SaveChanges();
                }
            }
        }

        private static async Task RemoveReports() {
            // Remove existing seeds
            using (var context = new dbpContext()) {
                context.Database.Log = Console.WriteLine;
                // TODO use store procedure to truncate table instead
                foreach (var eng in context.Reports) {
                    context.Reports.Remove(eng);
                }
                context.SaveChanges();
            }

            // mongo
            var _repo = new MongoDataContext();
            await _repo.RemoveAllReports();
        }

        private async static Task SeedReports() {
            // average number of reports considering 20 % pressure on developers, 230 working days per year
            var numberOfReports = 5000f;
            //var numberOfReports = 14000f;
            var startSampleDate = new DateTime(2013, 08, 01);
            var endSampleDate = new DateTime(2016, 08, 31);
            Random random = new Random();
            for (int i = 0; i < numberOfReports; i++) {
                // add basics for the engineers
                var reportfactory = new Faker<Report>()
                    .RuleFor(rep => rep.StartDate, f => f.Date.Between(startSampleDate, endSampleDate))
                    // end date to be within 3 days of start
                    .RuleFor(
                        rep => rep.EndDate,
                        (f, rep) => f.Date.Between(rep.StartDate, rep.StartDate.Add(new TimeSpan(3, 0, 0, 0))))
                    .RuleFor(rep => rep.Title, f => f.Lorem.Sentence());
                var report = reportfactory.Generate();

                // add 1,2 or 3 engineers
                using (var context = new dbpContext()) {
                    context.Database.Log = Console.WriteLine;
                    // random number of engineers
                    var engineersNumber = random.Next(1, 4);
                    for (int j = 0; j <= engineersNumber; j++)
                    {
                        // get one engineer
                        int toSkip = random.Next(1, context.Engineers.Count());
                        var engineer = context.Engineers.OrderBy(r => Guid.NewGuid()).Skip(toSkip).Take(1).FirstOrDefault();
                        // associate with report
                        report.Engineers.Add(engineer);
                    }
                }

                // one topic at random
                using (var context = new dbpContext()) {
                    context.Database.Log = Console.WriteLine;
                    // random number of engineers
                    int toSkip = random.Next(1, context.Topics.Count());

                    var topic = context.Topics.OrderBy(r => Guid.NewGuid()).Skip(toSkip).Take(1).FirstOrDefault();
                    // associate with report
                    report.Topic = topic;
                }

                // one partner at random
                using (var context = new dbpContext()) {
                    context.Database.Log = Console.WriteLine;
                    // random number of engineers
                    int toSkip = random.Next(1, context.Partners.Count());

                    var partner = context.Partners.OrderBy(r => Guid.NewGuid()).Skip(toSkip).Take(1).FirstOrDefault();
                    // associate with report
                    report.Partner = partner;
                }
                // persist the report
                using (var context = new dbpContext()) {
                    context.Database.Log = Console.WriteLine;
                    // need to reattach the related objects in order to prevent EF to create duplicates
                    foreach (var eng in report.Engineers) {
                        context.Engineers.Attach(eng);
                    }
                    context.Topics.Attach(report.Topic);
                    context.Partners.Attach(report.Partner);
                    context.Reports.Add(report);
                    context.SaveChanges();
                }

                // mongo
                // creating the basic Mongo report
                var _repo = new ReportRepository();
                var _mongorepo = new MongoDataContext();

                var mongoReport = new MongoReport {
                    StartDate = report.StartDate,
                    EndDate = report.EndDate,
                    Title = report.Title,
                    Topic = new MongoTopic {
                        Name = report.Topic.Name,
                        TopicFamily = _repo.getTopicFamily(report.Topic.TopicFamilyId).Name
                    },
                    EngineerIds = new List<ObjectId>()
                };

                var engineersIds = report.Engineers.Select((e) => _mongorepo.FindEngineerId(e.Name)).ToList();
                var res = await Task.WhenAll(engineersIds);
                mongoReport.EngineerIds = res.ToList();
                mongoReport.PartnerId = _mongorepo.FindPartnerId(report.Partner.Name);
                _mongorepo.InsertReport(mongoReport);
            }
        }
    }
}