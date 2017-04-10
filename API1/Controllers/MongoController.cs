using MongoDriver;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using DTO;
using System.Threading.Tasks;

namespace API1.Controllers {
    [RoutePrefix("api/mongo")]
    public class MongoController : ApiController {
        private readonly MongoDataContext _repo = new MongoDataContext();
        private readonly ReportMapper _mapper = new ReportMapper();

        /// <summary>
        /// get all reports associated with an engineer, without the serialization
        /// </summary>
        /// <param name="engineerName"></param>
        /// <returns></returns>
        [Route("reports")]
        [HttpGet]
        public async Task<HttpResponseMessage> getReportsByengineerNoSerialize(string engineerName) {
            var result = await _repo.GetReportsByEngineerName(engineerName);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// get all reports associated with an engineer, implementation Mongo > 3.3.4 (using aggregation)
        /// </summary>
        /// <param name="engineerName"></param>
        /// <returns></returns>
        [Route("reportsWithMapping")]
        [HttpGet]
        public async Task<HttpResponseMessage> getReportsByengineer(string engineerName) {
            var result = await _repo.GetReportsByEngineerName(engineerName);
            var strings = result.Select(r => MongoDB.Bson.BsonExtensionMethods.ToJson(r));
            return Request.CreateResponse(HttpStatusCode.OK, strings);
        }

        [Route("test")]
        [HttpGet]
        public HttpResponseMessage test() {
            var response = Request.CreateResponse(HttpStatusCode.OK,"ok");
            return response;
        }

        [Route("allReports")]
        [HttpGet]
        public HttpResponseMessage getreports() {
            var response = Request.CreateResponse(HttpStatusCode.OK, _repo.getReports());
            return response;
        }

        [Route("allEngineers")]
        [HttpGet]
        public HttpResponseMessage getengineers() {
            return Request.CreateResponse(HttpStatusCode.OK, _repo.GetEngineers());
        }

        [Route("allPartners")]
        [HttpGet]
        public HttpResponseMessage getPartners() {
            return Request.CreateResponse(HttpStatusCode.OK, _repo.GetAllPartners().Select(p => _mapper.mapPartner(p)));
        }

        /// <summary>
        /// get all reports associated with an engineer, implementation Mongo before 3.3.4 (using manula mapping)
        /// </summary>
        /// <param name="engineerName"></param>
        /// <returns></returns>
        [Route("reportsbyengineernamePre3")]
        [HttpGet]
        public HttpResponseMessage getReportsByengineerOld(string engineerName) {
            var result = _repo.GetReportsByEngNameNoAggregation(engineerName).Select(r => _mapper.mapReport(r));
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("reportsbypartnername")]
        [HttpGet]
        public async Task<HttpResponseMessage> getReportsByPartners(string partnerName) {
            var result = await _repo.GetReportsByPartnerName(partnerName);
            var strings = result.Select(r => MongoDB.Bson.BsonExtensionMethods.ToJson(r));
            return Request.CreateResponse(HttpStatusCode.OK, strings);
        }

    }
}
