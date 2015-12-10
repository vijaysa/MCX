using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MCX.Models.DbEntities;
using MCX.Models.Tables;

namespace MCX.Controllers
{
    // [Authorize]
    public class CustomersController : Controller
    {
        private DbEntities db = new DbEntities();

        // GET: Customers
        [HttpGet]
        public async Task<ActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            var count = 0;
            var customers = db.Customers.Include(c => c.LeadSource).Include(c => c.LeadStatu).Include(c => c.Product).Include(c => c.Stage);
            count = customers.Count();
            if (!String.IsNullOrEmpty(searchString))
            {
                customers = customers.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString)
                || s.Email.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    customers = customers.OrderByDescending(s => s.FirstName);
                    break;
                case "Date":
                    customers = customers.OrderBy(s => s.CreatedDate);
                    break;
                case "date_desc":
                    customers = customers.OrderByDescending(s => s.CreatedDate);
                    break;
                default:  // Name ascending 
                    customers = customers.OrderBy(s => s.LastName);
                    break;
            }

            count = customers.Count();

            return View(await customers.ToListAsync());
        }

        // GET: Customers
        public async Task<ActionResult> IndexMvcGrid()
        {
            var customers = db.Customers.Include(c => c.LeadSource).Include(c => c.LeadStatu).Include(c => c.Product).Include(c => c.Stage);
            return View(await customers.ToListAsync());
        }




        // GET: Customers/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = await db.Customers.FindAsync(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.LeadSourceId = new SelectList(db.LeadSources, "LeadSourceID", "SourceName");
            ViewBag.LeadStatusId = new SelectList(db.LeadStatus, "LeadStatusId", "LeadStatus");
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName");
            ViewBag.StageId = new SelectList(db.Stages, "StageId", "StageName");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerID,FirstName,LastName,Email,Mobile,Website,ProductId,LeadStatusId,LeadSourceId,StageId,Password,FollowUp,Address,Description,Phone,City,Investmentid,ConvertToPotential")] Customers customers)
        {


            //LeadOwner
            customers.IsActive = true;
            customers.Status = "A";


            if (customers.ConvertToPotential == 1)
            {
                customers.UserType = "P";
            }
            else
            {
                customers.UserType = "L";
            }
            //CreatedBy,
            customers.CreatedDate = DateTime.UtcNow;
            customers.IsDeleted = false;
            //Deletedby,
            //ModifiedBy,
            customers.ModifiedDate = DateTime.UtcNow;



            if (ModelState.IsValid)
            {
                db.Customers.Add(customers);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.LeadSourceId = new SelectList(db.LeadSources, "LeadSourceID", "SourceName", customers.LeadSourceId);
            ViewBag.LeadStatusId = new SelectList(db.LeadStatus, "LeadStatusId", "LeadStatus", customers.LeadStatusId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", customers.ProductId);
            ViewBag.StageId = new SelectList(db.Stages, "StageId", "StageName", customers.StageId);
            return View(customers);
        }

        // GET: Customers/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }



            Customers customers = await db.Customers.FindAsync(id);
            if (customers == null)
            {
                return HttpNotFound();
            }


            ViewBag.LeadSourceId = new SelectList(db.LeadSources, "LeadSourceID", "SourceName", customers.LeadSourceId);
            ViewBag.LeadStatusId = new SelectList(db.LeadStatus, "LeadStatusId", "LeadStatus", customers.LeadStatusId);
            ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", customers.ProductId);
            ViewBag.StageId = new SelectList(db.Stages, "StageId", "StageName", customers.StageId);
            return View(customers);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CustomerID,FirstName,LastName,LeadOwner,Email,Mobile,Website,ProductId,LeadStatusId,LeadSourceId,StageId,IsActive,Status,Password,FollowUp,Address,Description,Phone,City,UserType,CreatedBy,CreatedDate,IsDeleted,Deletedby,ModifiedBy,ModifiedDate,Investmentid,ConvertToPotential")] Customers customers)
        {

            try
            {


                if (ModelState.IsValid)
                {
                    var LoggedInUser = (Users)Session["LoggedInUser"];


                    customers.ModifiedDate = DateTime.UtcNow;
                    customers.ModifiedBy = LoggedInUser.LoginId;

                    if (customers.ConvertToPotential == 1)
                    {
                        customers.UserType = "P";
                    }

                    db.Entry(customers).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.LeadSourceId = new SelectList(db.LeadSources, "LeadSourceID", "SourceName", customers.LeadSourceId);
                ViewBag.LeadStatusId = new SelectList(db.LeadStatus, "LeadStatusId", "LeadStatus", customers.LeadStatusId);
                ViewBag.ProductId = new SelectList(db.Products, "ProductId", "ProductName", customers.ProductId);
                ViewBag.StageId = new SelectList(db.Stages, "StageId", "StageName", customers.StageId);
                return View(customers);
            }
            catch (Exception)
            {

                return RedirectToAction("Index", "Accounts");
            }
        }

        // GET: Customers/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customers customers = await db.Customers.FindAsync(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Customers customers = await db.Customers.FindAsync(id);
            db.Customers.Remove(customers);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> ConvertToPotential(long? CustomerID)
        {
            Customers customers = await db.Customers.FindAsync(Convert.ToInt32(CustomerID));
            customers.UserType = "P";
            db.Entry(customers).State = EntityState.Modified;
            await db.SaveChangesAsync();

            //return RedirectToAction("Index", "Customers");


            return RedirectToAction("Create", "PaymentDetails", new { CustomerID = CustomerID });
        }

        [NonAction]
        private bool IsAjax(ExceptionContext filterContext)
        {
            return filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            // if the request is AJAX return JSON else view.
            if (IsAjax(filterContext))
            {
                //Because its a exception raised after ajax invocation
                //Lets return Json
                filterContext.Result = new JsonResult()
                {
                    Data = filterContext.Exception.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

                filterContext.ExceptionHandled = true;
                filterContext.HttpContext.Response.Clear();
            }
            else
            {
                //Normal Exception
                //So let it handle by its default ways.
                base.OnException(filterContext);

            }
        }
    }
}
