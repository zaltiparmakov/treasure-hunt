using LovNaZaklad_WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace LovNaZaklad_WebAPI.ApiControllers
{
    [Authorize]
    public class AndroidController : ApiController
    {
        private LovNaZakladDbContext db = new LovNaZakladDbContext();

        [HttpGet]
        public User UserInfo()
        {
            return db.Users.FirstOrDefault(u => u.Username == "zaltiparmakov");
        }

        [HttpGet]
        public IEnumerable<Question> Questions()
        {
            var questions_list = db.Questions.Take(4).ToList();
            var treasure = db.Treasures.Single();
            return questions_list;
        }

        [HttpPost]
        public void Photo()
        {
            var httpRequest = HttpContext.Current.Request;

            var file = httpRequest.Files[0];

            if(file != null && file.ContentLength > 0)
            {
                int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                var ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                var extension = ext.ToLower();

                if (!AllowedFileExtensions.Contains(extension))
                {
                    var message = string.Format("Please Upload image of type .jpg,.gif,.png.");
                }
                else if (file.ContentLength > MaxContentLength)
                {
                    var message = string.Format("Please Upload a file upto 1 mb.");
                }
                else
                {
                    LovNaZaklad_WebAPI.EmguCV.ImageComparer imgCompare = new EmguCV.ImageComparer();
                    float[] features = imgCompare.features(file.InputStream);

                    
                }
            }
        }

        // used for CORS pre-flight
        [HttpOptions]
        [AllowAnonymous]
        public void Options() { }
    }
}
