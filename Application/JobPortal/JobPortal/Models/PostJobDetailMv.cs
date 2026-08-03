using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobPortal.Models
{
    public class PostJobDetailMv
    {
        public PostJobDetailMv()
        {
            Requirements= new List<JobRequireMv>();
        }
        public int PostJobID { get; set; }
        public string Company { get; set; }
        public string JobCategory { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string MinSalary { get; set; }
        public string MaxSalary { get; set; }
        public string Location { get; set; }
        public string Vacancy { get; set; }
        public string JobNature { get; set; }
        public string WebURL { get; set; }

        public List<JobRequireMv> Requirements {  get; set; }
    }
}