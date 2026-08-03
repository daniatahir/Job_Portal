using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Models
{
    public class JobRequireMv
    {

        public JobRequireMv() {
            Details = new List<JobRequirementsDetailsMv>();
        }
        public int JobRequirementID { get; set; }
        public string JobRequirementTitle { get; set; }

        public List<JobRequirementsDetailsMv> Details {  get; set; }
    }
}