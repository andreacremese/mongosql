using System.Net;
using System.Net.Http;
using System.Web.Http;
using System;
using DataModel;
using System.Linq;
using DTO;
using System.Linq.Expressions;
using DataObjects;
using System.Collections.Generic;

namespace API1.Controllers {
    [RoutePrefix("api/ef")]
    public class EFController : ApiController {
        // TODO include unity and injection for testing
        private readonly ReportRepository _repo = new ReportRepository();
        private readonly ReportMapper _mapper = new ReportMapper();

        [Route("reports")]
        [HttpGet]
        public HttpResponseMessage getReportsByEngineerNoMap(string engineerName) {
            List<ReportDto> result;
            using (var context = new dbpContext()) {
                result = (from r in context.Reports
                         where r.Engineers.Any(e => e.Name == engineerName)
                         select new ReportDto {
                             Id = r.Id.ToString(),
                             StartDate = r.StartDate,
                             EndDate = r.EndDate,
                             Title = r.Title,
                             Topic = new TopicDto
                             {
                                 Id = r.Topic.Id,
                                 Name = r.Topic.Name,
                                 //TopicFamily = r.Topic.TopicFamilyId
                             },
                             Partner = new PartnerDto
                             {
                                 Id = r.Partner.Id.ToString(),
                                 Name = r.Partner.Name
                             },
                             Engineers = r.Engineers.Select(re => new EngineerDto
                             {
                                 Id = re.Id.ToString(),
                                 Name = re.Name,
                                 Email = re.Email
                             }).ToList()

                         }).ToList();
            }
            return Request.CreateResponse(HttpStatusCode.OK,result);
        }

        [Route("reportsWithMapping")]
        [HttpGet]
        public HttpResponseMessage getReportsByEngineer(string engineerName) {
            return Request.CreateResponse(HttpStatusCode.OK,
                _repo.getReports(r => r.Engineers.Any(e => e.Name == engineerName))
                .Select(rep => _mapper.mapReport(rep)));
        }

        [Route("reportsQuery")]
        [HttpGet]
        public HttpResponseMessage getReports(
            String engineer = "", 
            String start = "", 
            String end="", 
            String topic = "",
            String partner = "",
            String enginerId = "",
            String partnerId = "",
            String topicId = "",
            String engineerId = "") {

            var startDate = start == "" ? new DateTime(2014, 01, 01) : DateTime.Parse(start);
            var endDate = end == "" ? DateTime.Now : DateTime.Parse(end);
            Expression<Func<Report, bool>> expression = r => r.StartDate < endDate && r.EndDate > startDate;

            if (engineer != "") {
                expression = expression.And(r => r.Engineers.Any(e => e.Name.Contains(engineer) || e.Email.Contains(engineer)));
            }

            if (topic != "") {
                expression = expression.And(r => r.Topic.Name.Contains(topic));
            }

            if (partner != "") {
                expression = expression.And(r => r.Partner.Name.Contains(partner));
            }

            Int32 eId;
            if (Int32.TryParse(engineerId, out eId)) {
                expression = expression.And(r => r.Engineers.Any(e => e.Id == eId));
            }

            Int32 tId;
            if (Int32.TryParse(topicId, out tId)) {
                expression = expression.And(r => r.Topic.Id == tId);
            }

            Int32 pId;
            if (Int32.TryParse(partnerId, out pId)) {
                expression = expression.And(r => r.Partner.Id == pId);
            }

            return Request.CreateResponse(HttpStatusCode.OK, 
                _repo.getReports(expression)
                .Select(rep => _mapper.mapReport(rep)));
        }

        
        [Route("allEngineers")]
        [HttpGet]
        public HttpResponseMessage getengineers() {
            var result = _repo.getEngineers().Select(e => _mapper.mapEngineer(e));
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [Route("allTopics")]
        [HttpGet]
        public HttpResponseMessage getTopics() {
            var result = _repo.getTopics().Select(t => _mapper.mapTopic(t));
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [Route("allPartners")]
        [HttpGet]
        public HttpResponseMessage getPartners() {
            var result = _repo.getPartners().Select(p => _mapper.mapPartner(p));
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }


        [Route("reportsbypartnername")]
        [HttpGet]
        public HttpResponseMessage getReportsByPartners(string partnerName) {
            return Request.CreateResponse(HttpStatusCode.OK,
                _repo.getReports(r => r.Partner.Name == partnerName)
                .Select(rep => _mapper.mapReport(rep)));
        }
    }
}
