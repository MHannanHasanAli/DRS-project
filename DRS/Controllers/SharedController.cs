using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DRS.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        public JsonResult UploadPhotos()
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                var file = Request.Files[0];

                var fileName = DateTime.Now.ToString("dd-MM-yyyy") + file.FileName;

                var path = Path.Combine(Server.MapPath("~/Photos/"), fileName);
                file.SaveAs(path);
                result.Data = new { Success = true, DocURL = string.Format("/Photos/{0}", fileName) };
            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Message = ex.Message };
                throw;
            }
            return result;
        }

        public FileResult DownloadFile(string path)
        {
            if (System.IO.File.Exists(Server.MapPath(path)))
            {
                // Determine the content type (MIME type) of the file
                string contentType = MimeMapping.GetMimeMapping(Server.MapPath(path));

                // Read the file data into a byte array
                byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath(path));

                // Create a FileContentResult to send the file to the client
                return File(fileBytes, contentType, Path.GetFileName(Server.MapPath(path)));
            }
            else
            {
                // Return a not found response if the file doesn't exist
                return null;
            }
        }

        public JsonResult ArticleUploadImage()
        {
            JsonResult result = new JsonResult();
            result.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            try
            {
                var file = Request.Files[0];

                var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

                var path = Path.Combine(Server.MapPath("~/Content/template/images/"), fileName);
                file.SaveAs(path);
                result.Data = new { Success = true, ImageURL = string.Format("/Content/template/images/{0}", fileName) };
            }
            catch (Exception ex)
            {
                result.Data = new { Success = false, Message = ex.Message };
                throw;
            }
            return result;
        }

    }
}