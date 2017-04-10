using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Data;

namespace DataModel
{
    public class ReportRepository {
        /// <summary>
        /// return the topic family given the topic family id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TopicFamily</returns>
        public TopicFamily getTopicFamily(Int32 id) {
            using (var context = new dbpContext()) {
                return context.TopicFamilies.AsNoTracking().Where(tf => tf.Id == id).FirstOrDefault();
            }
        }

        /// <summary>
        /// get all engineers
        /// </summary>
        /// <returns></returns>
        public List<Engineer> getEngineers() {
            using (var context = new dbpContext()) {
                return context.Engineers.AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// get all topics
        /// </summary>
        /// <returns></returns>
        public List<Topic> getTopics() {
            using (var context = new dbpContext()) {
                return context.Topics.AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// get all partners
        /// </summary>
        /// <returns></returns>
        public List<Partner> getPartners() {
            using (var context = new dbpContext()) {
                return context.Partners.AsNoTracking().ToList();
            }
        }

        /// <summary>
        /// returns reports applying a .where with the incomign lambda expression
        /// </summary>
        /// <param name="lambda"></param>
        /// <returns></returns>
        public List<Report> getReports(Expression<Func<Report, bool>> lambda) {
            using (var context = new dbpContext()) {
                context.Configuration.ProxyCreationEnabled = false;
                var linqQuery = context.Reports
                    .AsNoTracking()
                    .Include(rep => rep.Engineers)
                    .Include(rep => rep.Topic)
                    .Include(rep => rep.Partner)
                    .Where(lambda);
                return linqQuery.ToList();
            }
        }
    }
}
