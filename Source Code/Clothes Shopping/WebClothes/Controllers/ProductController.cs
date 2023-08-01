using DAOLibrary.Repository.Interface;
using DTOLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using WebClothes.Models;
using DAOLibrary.Repository.Object;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace WebClothes.Controllers
{
    public class ProductController : Controller
    {
        IProductRepository productRepository = null;
        ICategoryRepository categoryRepository = null;

        public ProductController()
        {
            productRepository = new ProductRepository();
            categoryRepository = new CategoryRepository();
        }

        // GET: Hiển thị danh sách sản phẩm với các tùy chọn lọc theo trạng thái và danh mục
        public ActionResult Index(string status, string categoryId)
        {
            var productList = productRepository.GetProductList();

            // Lọc sản phẩm theo danh mục nếu categoryId được chọn
            if (!string.IsNullOrEmpty(categoryId))
            {
                productList = productList.Where(p => p.CategoryId.ToString() == categoryId);
            }

            // Lọc sản phẩm theo trạng thái (true: hoạt động, false: không hoạt động)
            if (status == "true")
            {
                productList = productList.Where(p => p.Status == true);
            }
            else if (status == "false")
            {
                productList = productList.Where(p => p.Status == false);
            }

            ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName");
            return View(productList);
        }

        // GET: Hiển thị form tạo mới sản phẩm
        public ActionResult Create()
        {
            ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: Xử lý tạo mới sản phẩm sau khi submit form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductModel productViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Tạo đối tượng Product từ ProductModel để thêm vào cơ sở dữ liệu
                    Product product = new Product
                    {
                        ProductName = productViewModel.ProductName,
                        CategoryId = productViewModel.CategoryId,
                        Price = productViewModel.Price,
                        Quantity = productViewModel.Quantity,
                        Status = productViewModel.Status,
                        Image = productViewModel.Image
                    };
                    productRepository.CreatePostProduct(product);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName");
                ViewBag.Message = ex.Message;
                return View(productViewModel);
            }
        }

        // GET: Hiển thị chi tiết sản phẩm theo id
        public ActionResult Detail(int id)
        {
            var product = productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Hiển thị form chỉnh sửa sản phẩm theo id
        public ActionResult Edit(int id)
        {
            // Lấy thông tin sản phẩm từ Repository
            Product product = productRepository.GetProductById(id);
            ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName", product.CategoryId);
            ProductModel productModel = new ProductModel
            {
                ProductName = product.ProductName,
                CategoryId = (int)product.CategoryId,
                Price = (int)product.Price,
                Quantity = (int)product.Quantity,
                Image = product.Image,
                ProductId = product.ProductId,
                Status = (bool)product.Status
            };
            return View(productModel);
        }

        // POST: Xử lý cập nhật thông tin sản phẩm sau khi submit form chỉnh sửa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, ProductModel productModel)
        {
            try
            {
                if (id != productModel.ProductId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    // Tạo đối tượng Product từ ProductModel để cập nhật thông tin trong cơ sở dữ liệu
                    Product product = new Product
                    {
                        ProductName = productModel.ProductName,
                        CategoryId = productModel.CategoryId,
                        Price = productModel.Price,
                        Quantity = productModel.Quantity,
                        Status = productModel.Status,
                        ProductId = productModel.ProductId,
                        Image = productModel.Image // Bao gồm URL hình ảnh trong model
                    };

                    productRepository.UpdateProduct(product);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName", productModel.CategoryId);
                return View(productModel);
            }
        }

        // GET: Hiển thị form xác nhận xóa sản phẩm theo id
        public ActionResult Delete(int id)
        {
            var product = productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Xử lý xóa sản phẩm sau khi xác nhận xóa
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                // Thực hiện thao tác xóa sản phẩm có mã id được chỉ định
                productRepository.DeleteProduct(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(productRepository.GetProductById(id));
            }
        }
    }
}