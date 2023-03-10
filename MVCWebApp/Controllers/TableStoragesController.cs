using Microsoft.AspNetCore.Mvc;
using TAO.AzureStorage;
using TAO.AzureStorage.Model;

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
            return View();
        }
    }
}
