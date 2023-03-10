using Microsoft.AspNetCore.Mvc;
using TAO.AzureStorage.Model;
using TAO.AzureStorage.Services.Abstract;

namespace MVCWebApp.Controllers
{
    public class TableStoragesController : Controller
    {
        private readonly INoSqlStorage<Product> _noSqlStorage;
        public TableStoragesController(INoSqlStorage<Product> noSqlStorage)
        {
            _noSqlStorage = noSqlStorage;
        }
        public IActionResult Index()
        {
            ViewBag.products = _noSqlStorage.GetAll().ToList();
            ViewBag.isUpdate = false;

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            product.RowKey = Guid.NewGuid().ToString();
            product.PartitionKey = "Tech";

            await _noSqlStorage.Add(product);

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Update(string rowKey, string partitionKey)
        {
            var product = await _noSqlStorage.Get(rowKey, partitionKey);

            ViewBag.products = _noSqlStorage.GetAll().ToList();
            ViewBag.isUpdate = true;

            return View("Index",product);

        }
        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            product.ETag = "*";
            ViewBag.isUpdate = true;
            await _noSqlStorage.Update(product);
            
            return RedirectToAction("Index");
        } 
        [HttpGet]
        public async Task<IActionResult> Delete(string rowKey, string partitionKey)
        {
            await _noSqlStorage.Delete(rowKey,partitionKey);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Query(int price)
        {
            ViewBag.isUpdate = false;
            ViewBag.products = _noSqlStorage.Query(x => x.Price >= price).ToList();

            return View("Index");

        }
    }
}
