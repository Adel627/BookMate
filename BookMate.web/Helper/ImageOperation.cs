using BookMate.web.Core.Models;
using Microsoft.AspNetCore.Hosting;

namespace BookMate.web.Helper
{
    public  class ImageOperation
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
      
        public ImageOperation(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public string Save(IFormFile Image,string FolderName) 
        {
               

                var extension = Path.GetExtension(Image.FileName);

                var imageName = $"{Guid.NewGuid()}{extension}";

                var path = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/{FolderName}", imageName);

                using var stream = System.IO.File.Create(path);
                Image.CopyTo(stream);

                return imageName; 
        }

        public bool RemoveImage(string ImageUrl , string FolderName)
        {
            //Get old path to delete the image from the server
            var oldImagePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/images/{FolderName}", ImageUrl);

            //Delete old photo from server

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
                return true;
            }
            return false;
               
        }
    }
}
