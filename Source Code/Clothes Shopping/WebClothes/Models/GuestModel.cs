using DTOLibrary.Models;

namespace WebClothes.Models
{
    public class GuestModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
