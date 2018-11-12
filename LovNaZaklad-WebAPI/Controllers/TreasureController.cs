using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LovNaZaklad_WebAPI.Models;

namespace LovNaZaklad_WebAPI.Controllers
{
    [Authorize]
    public class TreasureController : Controller
    {
        private LovNaZakladDbContext db = new LovNaZakladDbContext();

        // GET: Treasure
        public ActionResult Index()
        {
            var images = db.Treasures.Include(i => i.Location);
            return View(images.ToList());
        }

        // GET: Treasure/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treasure image = db.Treasures.Include(i => i.Location).FirstOrDefault(x => x.TreasureID == id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // GET: Treasure/Create
        public ActionResult Create()
        {
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Name");
            return View();
        }

        // POST: Treasure/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TreasureID,Name,LocationID")] Treasure image)
        {
            // since we can uplaod just one file, we get the first one
            var file = Request.Files[0];
            // save images in server root in treasures dir
            var filePath = file.FileName;
            var fullPath = Server.MapPath("~/treasures/") + file.FileName;

            LovNaZaklad_WebAPI.EmguCV.ImageComparer imgCompare = new EmguCV.ImageComparer();
            float[] features = imgCompare.features(file.InputStream);
            
            // save features to database
            if (ModelState.IsValid)
            {
                db.Treasures.Add(new Treasure
                {
                    Name = image.Name,
                    Image = new Image
                    {
                        Path = filePath,
                        Features = string.Join(" ", features)
                    },
                    LocationID = image.LocationID
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Name", image.LocationID);
            return View(image);
        }

        // GET: Treasure/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treasure image = db.Treasures.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Name", image.LocationID);
            return View(image);
        }

        // POST: Treasure/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TreasureID,Name,TreasureImage,LocationID")] Treasure image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "Name", image.LocationID);
            return View(image);
        }

        // GET: Treasure/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treasure image = db.Treasures.Find(id);
            if (image == null)
            {
                return HttpNotFound();
            }
            return View(image);
        }

        // POST: Treasure/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Treasure image = db.Treasures.Find(id);
            db.Treasures.Remove(image);
            db.SaveChanges();
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
    }
}
