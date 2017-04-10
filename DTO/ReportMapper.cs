using DataModel;
using DataObjects;
using MongoDataObjects;
using MongoDB.Bson;
using MongoDriver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DTO {
    public class ReportMapper {
        private readonly ReportRepository _repo = new ReportRepository();
        private readonly MongoDataContext _mongorepo = new MongoDataContext();

        public ReportDto mapReport(MongoReport rep) {
            var result = new ReportDto {
                Id = rep.Id.ToString(),
                Partner = mapPartner(_mongorepo.FindPartnerById(rep.PartnerId)),
                Topic = new TopicDto {
                    Name = rep.Topic.Name,
                    TopicFamily = rep.Topic.TopicFamily
                },
                Title = rep.Title,
                EndDate = rep.EndDate,
                StartDate = rep.StartDate,
                Engineers = new List<EngineerDto>()
            };
            foreach (var _id in rep.EngineerIds) {
                var mongoEng = _mongorepo.FindEngineerById(_id);
                result.Engineers.Add(mapEngineer(mongoEng));
            }
            return result;
        }


        public EngineerDto mapEngineer(MongoEngineer engineer) {
            return new EngineerDto {
                Id = engineer.Id.ToString(),
                Name = engineer.Name,
                Email = engineer.Email
            };
        }

        public EngineerDto mapEngineer(LinqToSql.Engineer engineer) {
            return new EngineerDto {
                Id = engineer.Id.ToString(),
                Name = engineer.Name,
                Email = engineer.Email
            };
        }

        public ReportDto mapReport(Report rep) {
            return new ReportDto {
                Id = rep.Id.ToString(),
                Partner = mapPartner(rep.Partner),
                Topic = mapTopic(rep.Topic),
                Title = rep.Title,
                EndDate = rep.EndDate,
                StartDate = rep.StartDate,
                Engineers = rep.Engineers.Select(e => mapEngineer(e)).ToList(),
            };
        }

        public ReportDto mapReport(LinqToSql.Report rep) {
            return new ReportDto {
                Id = rep.Id.ToString(),
                Partner = mapPartner(rep.Partner),
                Topic = mapTopic(rep.Topic),
                Title = rep.Title,
                EndDate = rep.EndDate,
                StartDate = rep.StartDate,
                Engineers = rep.ReportEngineers.Select(e => mapEngineer(e.Engineer)).ToList()
            };
        }

        public EngineerDto mapEngineer(Engineer engineer) {
            return new EngineerDto {
                Id = engineer.Id.ToString(),
                Name = engineer.Name,
                Email = engineer.Email
            };
        }

        public PartnerDto mapPartner(Partner partner) {
            return new PartnerDto {
                Id = partner.Id.ToString(),
                Name = partner.Name
            };
        }

        public PartnerDto mapPartner(LinqToSql.Partner partner) {
            return new PartnerDto {
                Id = partner.Id.ToString(),
                Name = partner.Name
            };
        }

        public PartnerDto mapPartner(MongoPartner partner) {
            return new PartnerDto {
                Id = partner.Id.ToString(),
                Name = partner.Name
            };
        }

        public TopicDto mapTopic(Topic topic) {
            return new TopicDto {
                Id = topic.Id,
                Name = topic.Name,
                TopicFamily = _repo.getTopicFamily(topic.TopicFamilyId).Name
            };
        }

        public TopicDto mapTopic(LinqToSql.Topic topic) {
            return new TopicDto {
                Id = topic.Id,
                Name = topic.Name,
                TopicFamily = topic.TopicFamily.Name
            };
        }
    }
}
