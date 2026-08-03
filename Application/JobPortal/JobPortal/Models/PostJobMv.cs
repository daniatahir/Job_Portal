using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace JobPortal.Models
{
    public class PostJobMv
    {
        public int PostJobID { get; set; }
        public int UserID { get; set; }
        public int CompanyID { get; set; }
        public int JobCategoryID { get; set; }

        [StringLength(500, ErrorMessage = "Do not enter more than 500 characters")]

        public string JobTitle { get; set; }
        [StringLength(2000, ErrorMessage = "Do not enter more than 2000 characters")]

        public string JobDescription { get; set; }

        public string MinSalary { get; set; }

        public string MaxSalary { get; set; }

        public string Location { get; set; }

        public int Vacancy { get; set; }
        
        public int JobNatureID { get; set; }
       
        public int JobStatusID { get; set; }

        [DataType(DataType.Url)]
        public string WebUrl { get; set; }



    }
}