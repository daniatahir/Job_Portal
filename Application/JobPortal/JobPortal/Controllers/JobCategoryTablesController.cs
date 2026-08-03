using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DbLayer;

namespace JobPortal.Controllers
{
    public class JobCategoryTablesController : Controller
    {
        private CareerConnectDbEntities db = new CareerConnectDbEntities();

        int usertypeid;
        // GET: JobCategoryTables
        public ActionResult Index()
        {
          
            if (Session["UserTypeId"] != null && int.TryParse(Session["UserTypeId"].ToString(), out usertypeid))
            {
                // usertypeid will now hold the integer value
            }
            else
            {
                return RedirectToAction("Login", "User"); // Default value if null or not an integer
            }

            return View(db.JobCategoryTables.ToList());
        }

        // GET: JobCategoryTables/Details/5


        // GET: JobCategoryTables/Create
        public ActionResult Create()
        {
            if (Session["UserTypeId"] != null && int.TryParse(Session["UserTypeId"].ToString(), out usertypeid))
            {
                // usertypeid will now hold the integer value
            }
            else
            {
                return RedirectToAction("Login", "User"); // Default value if null or not an integer
            }
            ViewBag.JobCategoryID = new SelectList(db.PostJobTables, "PostJobID", "JobTitle");
            return View(new JobCategoryTable());
        }

        // POST: JobCategoryTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobCategoryTable jobCategoryTable)
        {
            if (Session["UserTypeId"] != null && int.TryParse(Session["UserTypeId"].ToString(), out usertypeid))
            {
                // usertypeid will now hold the integer value
            }
            else
            {
                return RedirectToAction("Login", "User"); // Default value if null or not an integer
            }
            if (ModelState.IsValid)
            {
                db.JobCategoryTables.Add(jobCategoryTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobCategoryID = new SelectList(db.PostJobTables, "PostJobID", "JobTitle", jobCategoryTable.JobCategoryID);
            return View(jobCategoryTable);
        }

        // GET: JobCategoryTables/Edit/5
        public ActionResult Edit(int? id)

        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobCategoryTable jobCategoryTable = db.JobCategoryTables.Find(id);
            if (jobCategoryTable == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobCategoryID = new SelectList(db.PostJobTables, "PostJobID", "JobTitle", jobCategoryTable.JobCategoryID);
            return View(jobCategoryTable);
        }

        // POST: JobCategoryTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JobCategoryTable jobCategoryTable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobCategoryTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobCategoryID = new SelectList(db.PostJobTables, "PostJobID", "JobTitle", jobCategoryTable.JobCategoryID);
            return View(jobCategoryTable);
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
