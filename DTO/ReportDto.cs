using System;
using System.Collections.Generic;

namespace DTO {
    public class ReportDto {
        public ReportDto() {
            Engineers = new List<EngineerDto>();
        }
        public String Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public String Title { get; set; }
        public List<EngineerDto> Engineers { get; set; }
        public TopicDto Topic { get; set; }
        public PartnerDto Partner { get; set; }
    }
}
