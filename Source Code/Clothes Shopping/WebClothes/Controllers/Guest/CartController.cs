using DAOLibrary.Repository.Interface;
using DAOLibrary.Repository.Object;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebClothes.Models;

namespace WebClothes.Controllers
{
    public class CartController : Controller
    {
        IProductRepository productRepository = null;

        public CartController()
        {
            productRepository = new ProductRepository();
        }

        // GET: Hiển thị trang giỏ hàng
        public IActionResult Index()
        {
            // Lấy dữ liệu giỏ hàng từ Session
            var cart = HttpContext.Session.GetComplexData<List<CartModel>>("CART");
            List<CartModel> list = new List<CartModel>();

            if (cart != null)
            {
                list = (List<CartModel>)cart;
            }

            return View(list);
        }

        public IActionResult Myorder()
        {
            // Lấy dữ liệu giỏ hàng từ Session
            var cart = HttpContext.Session.GetComplexData<List<CartModel>>("CART");
            List<CartModel> list = new List<CartModel>();

            if (cart != null)
            {
                list = (List<CartModel>)cart;
            }

            return View(list);
        }

        // POST: Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(int ProductId, int Quantity)
        {
            var product = productRepository.GetProductById(ProductId);

            // Lấy dữ liệu giỏ hàng từ Session
            var cart = HttpContext.Session.GetComplexData<List<CartModel>>("CART");

            if (cart != null)
            {
                // Nếu giỏ hàng đã có dữ liệu
                var list = (List<CartModel>)cart;

                // Kiểm tra nếu sản phẩm đã tồn tại trong giỏ hàng
                if (list.Exists(i => i.ProductId == product.ProductId))
                {
                    // Nếu có, tăng số lượng sản phẩm 
                    foreach (var item in list)
                    {
                        if (item.ProductId == product.ProductId)
                        {
                            item.Quantity += Quantity;
                        }
                    }
                    // Cập nhật lại giỏ hàng trong Session
                    HttpContext.Session.SetComplexData("CART", list);
                }
                else
                {
                    // Nếu sản phẩm chưa tồn tại trong giỏ hàng
                    // Tạo mới một sản phẩm trong giỏ hàng
                    var item = new CartModel();
                    item.ProductId = product.ProductId;
                    item.ProductName = product.ProductName;
                    item.Price = (float)product.Price;
                    item.Image = product.Image;
                    item.Quantity = Quantity;
                    list.Add(item);
                    // Cập nhật lại giỏ hàng trong Session
                    HttpContext.Session.SetComplexData("CART", list);
                }
            }
            else
            {
                // Nếu giỏ hàng chưa có dữ liệu
                // Tạo mới một sản phẩm trong giỏ hàng
                var item = new CartModel();
                item.ProductId = product.ProductId;
                item.ProductName = product.ProductName;
                item.Price = (float)product.Price;
                item.Image = product.Image;
                item.Quantity = Quantity;
                var list = new List<CartModel>();
                list.Add(item);
                // Lưu danh sách sản phẩm vào Session để tạo giỏ hàng mới
                HttpContext.Session.SetComplexData("CART", list);
            }

            // Sau khi thêm sản phẩm vào giỏ hàng, chuyển hướng người dùng về trang "Guest"
            return Redirect("/Guest");
        }

        // GET: Xóa sản phẩm khỏi giỏ hàng
        public IActionResult Remove(int? ProductId)
        {
            if (ProductId == null)
            {
                return NotFound();
            }

            // Lấy thông tin sản phẩm từ Repository
            var product = productRepository.GetProductById((int)ProductId);

            if (product == null)
            {
                return NotFound();
            }

            // Lấy dữ liệu giỏ hàng từ Session
            var cart = HttpContext.Session.GetComplexData<List<CartModel>>("CART");
            List<CartModel> list = new List<CartModel>();

            if (cart != null)
            {
                list = (List<CartModel>)cart;

                // Kiểm tra nếu sản phẩm có mã ProductId tồn tại trong giỏ hàng
                if (list.Exists(x => x.ProductId == ProductId))
                {
                    // Nếu có, xóa sản phẩm đó khỏi danh sách giỏ hàng
                    foreach (var item in list)
                    {
                        if (item.ProductId == ProductId)
                        {
                            list.Remove(item);
                            break;
                        }
                    }
                    // Cập nhật lại giỏ hàng trong Session
                    HttpContext.Session.SetComplexData("CART", list);
                }
            }

            // Sau khi xóa sản phẩm khỏi giỏ hàng, chuyển hướng người dùng về trang Cart
            return RedirectToAction("Index");
        }
    }
}

#region ExtendSession
public static class SessionExtensions
{
    // Phương thức GetComplexData dùng để lấy dữ liệu có kiểu phức tạp từ Session
    public static T GetComplexData<T>(this ISession session, string key)
    {
        var data = session.GetString(key);
        if (data == null)
        {
            return default(T);
        }
        return JsonConvert.DeserializeObject<T>(data);
    }

    // Phương thức SetComplexData dùng để lưu trữ dữ liệu phức tạp vào Session
    public static void SetComplexData(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }
}
#endregion