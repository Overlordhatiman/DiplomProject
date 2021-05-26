using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Lifetime;
using System.Web;
using System.Web.Mvc;
using Diploma.Models;
using Diploma.Models.DataBase;
using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;

namespace Diploma.Controllers
{
    [Authorize]
    public class PicturesController : Controller
    {
        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("C:/Users/Nameless/source/repos/Diploma/Diploma/Content/haarcascade_frontalface_alt_tree.xml");
        private DBContext db = new DBContext();

        // GET: Pictures
        public ActionResult Index()
        {
            var pictures = db.Pictures.Include(p => p.Employee);
            return View(pictures.ToList());
        }

        // GET: Pictures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        [HttpGet]
        public ActionResult Detect(int? id)
        {
            var arrayOfEmployees = db.Pictures.Include(p => p.Employee);
            var tmp = arrayOfEmployees.ToList();

            foreach (var item in tmp)
            {
                if (item.Id == id)
                {
                    ViewData["IsAnEmployee"] = "Я нашел его. Это - " + item.Employee.FirstName;
                    break;
                }
                else
                {
                    ViewData["IsAnEmployee"] = "Cant find this person";
                }
            }

            Picture picture = db.Pictures.Find(id);            

            if (ModelState.IsValid && picture != null)
            {
                Bitmap bitmap;

                using (var ms = new MemoryStream(picture.Image))
                {
                    bitmap = new Bitmap(ms);
                }

                Image<Bgr, byte> img = new Image<Bgr, byte>(bitmap);

                Rectangle[] rectangles = cascadeClassifier.DetectMultiScale(img, 1.1, 1);

                foreach (Rectangle rec in rectangles)
                {
                    using (Graphics gh = Graphics.FromImage(bitmap))
                    {
                        using (Pen pen = new Pen(Color.Red, 1))
                        {
                            gh.DrawRectangle(pen, rec);
                        }
                    }
                }

                var bitmapBytes = BitmapToBytes(bitmap); //Convert bitmap into a byte array

                Picture temp = new Picture
                {
                    Id = picture.Id,
                    Employee = picture.Employee,
                    EmployeeId = picture.EmployeeId,
                    Image = bitmapBytes,
                    Name = picture.Name
                };

                return View(temp);//Return as file result
            }

            return RedirectToAction("Index");
        }
        
        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        // GET: Pictures/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName");
            return View();
        }

        // POST: Pictures/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Picture pic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                // установка массива байтов
                pic.Image = imageData;

                db.Pictures.Add(pic);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", pic.EmployeeId);
            return View(pic);
        }

        // GET: Pictures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", picture.EmployeeId);
            return View(picture);
        }

        // POST: Pictures/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Image,EmployeeId")] Picture pic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;
                // считываем переданный файл в массив байтов
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                // установка массива байтов
                pic.Image = imageData;

                db.Entry(pic).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(db.Employees, "Id", "FirstName", pic.EmployeeId);
            return View(pic);
        }

        // GET: Pictures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Picture picture = db.Pictures.Find(id);
            if (picture == null)
            {
                return HttpNotFound();
            }
            return View(picture);
        }

        // POST: Pictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Picture picture = db.Pictures.Find(id);
            db.Pictures.Remove(picture);
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
