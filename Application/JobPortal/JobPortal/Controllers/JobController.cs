using DbLayer;
using JobPortal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Mvc;

namespace JobPortal.Controllers
{
    public class JobController : Controller
    {
        private CareerConnectDbEntities DB = new CareerConnectDbEntities();

        public ActionResult PostJob()
        {
            var job = new PostJobMv();
            ViewBag.JobCategoryID = new SelectList(
                DB.JobCategoryTables.ToList(),
                "JobCategoryID",
                "JobCategory",
                "0");
            ViewBag.JobNatureID = new SelectList(
                DB.JobNatureTables.ToList(),
                "JobNatureID",
                "JobNature",
                "0");
            return View(job);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        //public ActionResult PostJob(PostJobMv postjobmv)
        //{
        //    if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
        //    {
        //        return RedirectToAction("Login", "User");
        //    }

        //    int userid = 0;
        //    int companyid = 0;
        //    int.TryParse(Convert.ToString(Session["UserID"]), out userid);
        //    int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

        //    postjobmv.UserID = userid;
        //    postjobmv.CompanyID = companyid;

        //    if (ModelState.IsValid)
        //    {
        //        var post = new PostJobTable
        //        {

        //            UserID = postjobmv.UserID,
        //            CompanyID = postjobmv.CompanyID,
        //            JobCategoryID = postjobmv.JobCategoryID,
        //            JobTitle = postjobmv.JobTitle,
        //            JobDescription = postjobmv.JobDescription,
        //            MinSalary = postjobmv.MinSalary,
        //            MaxSalary = postjobmv.MaxSalary,
        //            Location = postjobmv.Location,
        //            Vacancy = postjobmv.Vacancy,
        //            JobStatusID = 1, 
        //            JobNatureID = 1,
        //            WebURL = postjobmv.WebUrl
        //        };

        //        // Save the record to the database
        //        DB.PostJobTables.Add(post);
        //        DB.SaveChanges();

        //        // Access the database-generated PostJobID
        //        Session["PostJobID"] = post.PostJobID;

        //        return RedirectToAction("CompanyJobsList");
        //    }

        //    // Populate dropdowns and return the view if the model is invalid
        //    ViewBag.JobCategoryID = new SelectList(
        //        DB.JobCategoryTables.ToList(),
        //        "JobCategoryID",
        //        "JobCategory",
        //        "0");
        //    ViewBag.JobNatureID = new SelectList(
        //        DB.JobNatureTables.ToList(),
        //        "JobNatureID",
        //        "JobNature",
        //        "0");

        //    return View(postjobmv);
        //}

        public ActionResult PostJob(PostJobMv postjobmv)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);

            postjobmv.UserID = userid;
            postjobmv.CompanyID = companyid;

            if (ModelState.IsValid)
            {
                try
                {
                    // Custom SQL query for insertion
                    string sqlQuery = @"
                INSERT INTO PostJobTable (UserID, CompanyID, JobCategoryID, JobTitle, JobDescription, MinSalary, MaxSalary, Location, Vacancy, JobStatusID, JobNatureID, WebURL)
                VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11)";

                    DB.Database.ExecuteSqlCommand(
                        sqlQuery,
                        postjobmv.UserID,
                        postjobmv.CompanyID,
                        postjobmv.JobCategoryID,
                        postjobmv.JobTitle,
                        postjobmv.JobDescription,
                        postjobmv.MinSalary,
                        postjobmv.MaxSalary,
                        postjobmv.Location,
                        postjobmv.Vacancy,
                         //  postjobmv.JobStatusID, // JobStatusID: Assuming "1" means active or new job
                         1,
                        postjobmv.JobNatureID, // JobNatureID: Assuming "1" is the default value
                        postjobmv.WebUrl
                    );

                    // Optional: Retrieve the newly inserted PostJobID if required
                    var lastInsertedJob = DB.PostJobTables
                        .OrderByDescending(j => j.PostJobID)
                        .FirstOrDefault();

                    if (lastInsertedJob != null)
                    {
                        Session["PostJobID"] = lastInsertedJob.PostJobID;
                    }

                    return RedirectToAction("CompanyJobsList");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error while saving data: " + ex.Message);
                }
            }

            // Populate dropdowns and return the view if the model is invalid
            ViewBag.JobCategoryID = new SelectList(
                DB.JobCategoryTables.ToList(),
                "JobCategoryID",
                "JobCategory",
                "0");
            ViewBag.JobNatureID = new SelectList(
                DB.JobNatureTables.ToList(),
                "JobNatureID",
                "JobNature",
                "0");

            return View(postjobmv);
        }

        public ActionResult CompanyJobsList()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            var allpost = DB.PostJobTables.Where(c=>c.CompanyID==companyid && c.UserID==userid).ToList();
            return View(allpost);
        }



        public ActionResult AllCompanyPendingJobs()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            int userid = 0;
            int companyid = 0;
            int.TryParse(Convert.ToString(Session["UserID"]), out userid);
            int.TryParse(Convert.ToString(Session["CompanyID"]), out companyid);
            var allpost = DB.PostJobTables.ToList();

            if (allpost.Count() > 0)
            {
                allpost = allpost.OrderByDescending(o => o.PostJobID).ToList();
            }

            return View(allpost);
        }


        public ActionResult AddRequirements(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var details = DB.JobRequirementDetailTables.Where(j => j.PostJobID == id).ToList();
            if (details.Count() > 0)
            {
                details = details.OrderBy(r => r.JobRequirementID).ToList();
            }

            var requirements = new JobRequirementsMv();
            requirements.Details = details;
            requirements.PostJobID = (int)id;

            ViewBag.JobRequirementID = new SelectList(DB.JobRequirementsTables.ToList(), "JobRequirementID", "JobRequirementTitle", "0");


            return View(requirements);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRequirements(JobRequirementsMv jobRequirementsMv)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }
            var requirements = new JobRequirementDetailTable();
            try
            {
                string sqlQuery = @"
            INSERT INTO JobRequirementDetailTable(JobRequirementID, JobRequirementDetail,PostJobID)
                VALUES(@p0, @p1, @p2)";
                DB.Database.ExecuteSqlCommand(
                    sqlQuery,
                requirements.JobRequirementID = jobRequirementsMv.JobRequirementID,
                requirements.JobRequirementDetail = jobRequirementsMv.JobRequirementDetail,
                requirements.PostJobID = jobRequirementsMv.PostJobID
                );
                return RedirectToAction("AddRequirements", new { @id=requirements.PostJobID});
            }
            catch (Exception ex)
            {
                var details = DB.JobRequirementDetailTables.Where(j => j.PostJobID == jobRequirementsMv.PostJobID).ToList();
                if (details.Count() > 0)
                {
                    details = details.OrderBy(r => r.JobRequirementID).ToList();
                }
                jobRequirementsMv.Details = details;
                ModelState.AddModelError("JobRequirementID", "Required");
            }
            /*
            var requirements = new JobRequirementDetailTable();
            requirements.JobRequirementID = jobRequirementsMv.JobRequirementID;
            requirements.JobRequirementDetail = jobRequirementsMv.JobRequirementDetail;
            requirements.PostJobID = jobRequirementsMv.PostJobID;
            DB.JobRequirementDetailTables.Add(requirements);
            DB.SaveChanges();*/
            
            ViewBag.JobRequirementID = new SelectList(DB.JobRequirementsTables.ToList(), "JobRequirementID", "JobRequirementTitle", jobRequirementsMv.JobRequirementID); ;
            return View(jobRequirementsMv);
        }

        public ActionResult DeleteRequirements(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpostid = DB.JobRequirementDetailTables.Find(id).PostJobID;
            var requirements = DB.JobRequirementDetailTables.Find(id);
            DB.Entry(requirements).State = System.Data.Entity.EntityState.Deleted;
            DB.SaveChanges();

            return RedirectToAction("AddRequirements", new {@id= jobpostid });
        }

        public ActionResult DeleteJobPost(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = DB.PostJobTables.Find(id);
            DB.Entry(jobpost).State = System.Data.Entity.EntityState.Deleted;
            DB.SaveChanges();
            return RedirectToAction("CompanyJobsList");
        }



        public ActionResult ApprovedPost(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = DB.PostJobTables.Find(id);
            jobpost.JobStatusID = 2;
            DB.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("AllCompanyPendingJobs");

        }


        public ActionResult CanceledPost(int? id)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["UserTypeID"])))
            {
                return RedirectToAction("Login", "User");
            }

            var jobpost = DB.PostJobTables.Find(id);
            jobpost.JobStatusID = 3;
            DB.Entry(jobpost).State = System.Data.Entity.EntityState.Modified;
            DB.SaveChanges();
            return RedirectToAction("AllCompanyPendingJobs");

        }


        public ActionResult JobDetails(int? id)
        {
            var getpostjob = DB.PostJobTables.Find(id);
            var postjob = new PostJobDetailMv();
            postjob.PostJobID = getpostjob.PostJobID;
            postjob.Company = getpostjob.CompanyTable.CompanyName;
            postjob.JobCategory =getpostjob.JobCategoryTable.JobCategory;
            postjob.JobTitle = getpostjob.JobTitle;
            postjob.JobDescription = getpostjob.JobDescription;
            postjob.MinSalary = getpostjob.MinSalary;
            postjob.MaxSalary = getpostjob.MaxSalary;
            postjob.Location = getpostjob.Location;
            postjob.Vacancy = getpostjob.Vacancy;
            postjob.JobNature = getpostjob.JobNatureTable.JobNature;
            postjob.WebURL = getpostjob.WebURL;

            //var jobDetails = new List<JobRequirementDetailTable>();


            getpostjob.JobRequirementDetailTables = getpostjob.JobRequirementDetailTables.OrderBy(d => d.JobRequirementID).ToList();
            int jobrequirementid = 0;
            var jobrequirement = new JobRequireMv();
            foreach(var detail in getpostjob.JobRequirementDetailTables)
            {
                var jobrequirementdetails = new JobRequirementsDetailsMv();

                if (jobrequirementid == 0)
                {
                    jobrequirement.JobRequirementID = detail.JobRequirementID;
                    jobrequirement.JobRequirementTitle = detail.JobRequirementsTable.JobRequirementTitle;
                    jobrequirementdetails.JobRequirementID = detail.JobRequirementID;
                    jobrequirementdetails.JobRequirementDetail = detail.JobRequirementDetail;
                    jobrequirement.Details.Add(jobrequirementdetails);
                    jobrequirementid = detail.JobRequirementID;
                }
                else if (jobrequirementid == detail.JobRequirementID)
                {
                    jobrequirementdetails.JobRequirementID = detail.JobRequirementID;
                    jobrequirementdetails.JobRequirementDetail = detail.JobRequirementDetail;
                    jobrequirement.Details.Add(jobrequirementdetails);
                    jobrequirementid = detail.JobRequirementID;
                }
                else if(jobrequirementid != detail.JobRequirementID)
                {
                    postjob.Requirements.Add(jobrequirement);
                    jobrequirement = new JobRequireMv();
                    jobrequirement.JobRequirementID = detail.JobRequirementID;
                    jobrequirement.JobRequirementTitle = detail.JobRequirementsTable.JobRequirementTitle;
                    jobrequirementdetails.JobRequirementID = detail.JobRequirementID;
                    jobrequirementdetails.JobRequirementDetail = detail.JobRequirementDetail;
                    jobrequirement.Details.Add(jobrequirementdetails);
                    jobrequirementid = detail.JobRequirementID;
                }

            }
            postjob.Requirements.Add(jobrequirement);

            return View(postjob);
        }

        public ActionResult FilterJob()
        {
            var obj = new FilterJobMv();
            var result = DB.PostJobTables.Where(r => r.JobStatusID==2).ToList();
            obj.Result = result;
            ViewBag.JobCategoryID = new SelectList(
                DB.JobCategoryTables.ToList(),
                "JobCategoryID",
                "JobCategory",
                "0");
            ViewBag.JobNatureID = new SelectList(
                DB.JobNatureTables.ToList(),
                "JobNatureID",
                "JobNature",
                "0");

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult FilterJob(FilterJobMv filterJobMv)
        {
            var result = DB.PostJobTables.Where(r => r.JobStatusID==2 && (r.JobCategoryID == filterJobMv.JobCategoryID && r.JobNatureID == filterJobMv.JobNatureID)).ToList();
            filterJobMv.Result = result;



            ViewBag.JobCategoryID = new SelectList(
                DB.JobCategoryTables.ToList(),
                "JobCategoryID",
                "JobCategory",
                filterJobMv.JobCategoryID);
            ViewBag.JobNatureID = new SelectList(
                DB.JobNatureTables.ToList(),
                "JobNatureID",
                "JobNature",
                filterJobMv.JobNatureID);

            return View(filterJobMv);
        }



    }
}