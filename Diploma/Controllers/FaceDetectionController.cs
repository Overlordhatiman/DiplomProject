using Diploma.Models;
using Diploma.Models.DataBase;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Diploma.Controllers
{

    public class FaceDetectionController : Controller
    {
        static readonly CascadeClassifier cascadeClassifier = new CascadeClassifier("C:/Users/Nameless/source/repos/Diploma/Diploma/Content/haarcascade_frontalface_alt_tree.xml");
        private DBContext db = new DBContext();

        
        // GET: FaceDetection
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Detect(HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                Bitmap bitmap = new Bitmap(uploadImage.InputStream);

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
                return File(bitmapBytes, "image/jpeg"); //Return as file result
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
    }
}