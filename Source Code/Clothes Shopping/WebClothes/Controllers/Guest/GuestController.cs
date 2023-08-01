using DAOLibrary.Repository.Interface;
using DAOLibrary.Repository.Object;
using DTOLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using WebClothes.Models;

namespace WebClothes.Controllers.Customer
{
    public class GuestController : Controller
    {
        IProductRepository productRepository = null;
        ICategoryRepository categoryRepository = null;
        public GuestController()
        {
            productRepository = new ProductRepository();
            categoryRepository = new CategoryRepository();
        }
        public IActionResult Index()
        {
            IEnumerable<Product> listProduct = productRepository.GetProductList();
            IEnumerable<Category> listCategory = categoryRepository.GetCategoryList();
            GuestModel models = new GuestModel();
            models.Products = listProduct;
            models.Categories = listCategory;
            if (TempData["CheckoutMessage"] != null)
            {
                ViewBag.Message = TempData["CheckoutMessage"];
                TempData.Remove("CheckoutMessage");
            }
            return View(models);
        }

        //GET - FILTER
        public IActionResult Filter(string CategoryName)
        {
            if (CategoryName == null)
            {
                return NotFound();
            }
            var category = categoryRepository.GetCategoryByName(CategoryName);
            if (category == null)
            {
                return NotFound();
            }
            IEnumerable<Product> listProduct = productRepository.GetProductList()
                .Where(p => p.CategoryId == category.CategoryId).ToList();
            IEnumerable<Category> listCategory = categoryRepository.GetCategoryList();
            GuestModel models = new GuestModel();
            models.Products = listProduct;
            models.Categories = listCategory;
            HttpContext.Session.SetString("SelectedCat", CategoryName);
            return View("Index", models);
        }

        //POST - SEARCH
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Search(string ProductName)
        {
            GuestModel models = new GuestModel();
            IEnumerable<Category> listCategory = categoryRepository.GetCategoryList();
            IEnumerable<Product> listProduct = null;
            var SelectedCat = HttpContext.Session.GetString("SelectedCat");
            if (ProductName == null)
            {
                listProduct = new List<Product>();
            }
            else if (SelectedCat == null)
            {
                listProduct = productRepository.GetProductList()
                     .Where(p => p.ProductName.ToLower().Contains(ProductName.ToLower())).ToList();
            }
            else
            {
                int CategoryId = categoryRepository.GetCategoryByName(SelectedCat).CategoryId;
                listProduct = productRepository.GetProductList()
                    .Where(p => p.CategoryId == CategoryId)
                    .Where(p => p.ProductName.ToLower().Contains(ProductName.ToLower())).ToList();
            }
            models.Products = listProduct;
            models.Categories = listCategory;
            ViewBag.SearchValue = ProductName;
            return View("Index", models);
        }
    }
}
