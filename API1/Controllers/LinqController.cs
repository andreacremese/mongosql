using DTO;
using LinqToSql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API1.Controllers {
    [RoutePrefix("api/linq")]
    public class LinqController : ApiController {
        private readonly dbperformanceDataContext _repo = new dbperformanceDataContext(
        ConfigurationManager.ConnectionStrings["dbPerformanceContext"].ConnectionString);
        private readonly ReportMapper _mapper = new ReportMapper();

        [Route("reports")]
        [HttpGet]
        public HttpResponseMessage reportsByEngineerName(String engineerName) {
            var result = (from r in _repo.Reports
                          join re in _repo.ReportEngineers on r.Id equals re.Report_Id
                          join e in _repo.Engineers on re.Engineer_Id equals e.Id

                          where e.Name.Equals(engineerName)
                          // select _mapper.mapReport(r)
                          select new ReportDto {
                              Id = r.Id.ToString(),
                              StartDate = r.StartDate,
                              EndDate = r.EndDate,
                              Title = r.Title,
                              Topic = new TopicDto {
                                  Id = r.Topic.Id,
                                  Name = r.Topic.Name,
                                  TopicFamily = r.Topic.TopicFamily.Name
                              },
                              Partner = new PartnerDto {
                                  Id = r.Partner.Id.ToString(),
                                  Name = r.Partner.Name
                              },
                              Engineers = r.ReportEngineers.Select(re => new EngineerDto {
                                  Id = re.Engineer.Id.ToString(),
                                  Name = re.Engineer.Name,
                                  Email = re.Engineer.Email
                              }).ToList()
                          });

            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [Route("reportsWithMapping")]
        [HttpGet]
        public HttpResponseMessage reportsByEngineerNameMaper(String engineerName) {
            var result = (from r in _repo.Reports
                          join re in _repo.ReportEngineers on r.Id equals re.Report_Id
                          join e in _repo.Engineers on re.Engineer_Id equals e.Id

                          where e.Name.Equals(engineerName)
                          select _mapper.mapReport(r));

            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [Route("reportsSproc")]
        [HttpGet]
        public HttpResponseMessage reportsByEngineerNameSProc(String engineerName){
            var result = _repo.GetReportsByEngineers(engineerName);

            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [Route("reportsFunc")]
        [HttpGet]
        public HttpResponseMessage reportsByEngineerNameFunc(String engineerName) {
            var result = _repo.GetReportsByEngineersFunc(engineerName);
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }

        [Route("allEngineers")]
        [HttpGet]
        public HttpResponseMessage getengineers() {
            var result = _repo.Engineers.Where(e => true).Select(e => _mapper.mapEngineer(e)).ToList();
            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}
