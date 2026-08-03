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
    public class JobNatureTablesController : Controller
    {
        private CareerConnectDbEntities db = new CareerConnectDbEntities();
        int usertypeid;
        // GET: JobNatureTables
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
            return View(db.JobNatureTables.ToList());
        }

       

        // GET: JobNatureTables/Create
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
            return View(new JobNatureTable());
        }

        // POST: JobNatureTables/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "JobNatureID,JobNature")] JobNatureTable jobNatureTable)
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
                db.JobNatureTables.Add(jobNatureTable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobNatureTable);
        }

        // GET: JobNatureTables/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["UserTypeId"] != null && int.TryParse(Session["UserTypeId"].ToString(), out usertypeid))
            {
                // usertypeid will now hold the integer value
            }
            else
            {
                return RedirectToAction("Login", "User"); // Default value if null or not an integer
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobNatureTable jobNatureTable = db.JobNatureTables.Find(id);
            if (jobNatureTable == null)
            {
                return HttpNotFound();
            }
            return View(jobNatureTable);
        }

        // POST: JobNatureTables/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "JobNatureID,JobNature")] JobNatureTable jobNatureTable)
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
                db.Entry(jobNatureTable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobNatureTable);
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
