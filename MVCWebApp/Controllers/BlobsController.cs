using Microsoft.AspNetCore.Mvc;
using MVCWebApp.Models;
using TAO.AzureStorage;
using TAO.AzureStorage.Enums;

namespace MVCWebApp.Controllers
{
    public class BlobsController : Controller
    {
        private readonly IBlobStorage _blobStorage;
        public BlobsController(IBlobStorage blobStorage)
        {
            _blobStorage = blobStorage;
        }
        public IActionResult Index()
        {
            var names = _blobStorage.GetNames(EContainerName.pictures);
            string blobUrl = $"{_blobStorage.BlobUrl}/{EContainerName.pictures.ToString()}";
            ViewBag.blobs = names.Select(x => new FileBlob
            {
                Name = x,
                Url = $"{blobUrl}/x"

            }).ToList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile picture)
        {
            var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);

            await _blobStorage.UploadAsync(picture.OpenReadStream(),newFileName,EContainerName.pictures);

            return RedirectToAction("Index");

        }

    }
}
