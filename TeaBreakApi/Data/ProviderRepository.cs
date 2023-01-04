using System.Xml.Linq;
using TeaBreakApi.Domain;

namespace TeaBreakApi.Data
{
    public class ProviderRepository
    {
        private List<Provider> _providers = new List<Provider>()
        {
            new Provider() 
            { 
                Id = new Guid("38690a24-1d4c-4c01-a242-60a097d3a473"), 
                Name = "Anvat.com", 
                Information = "Chuyên bán đồ ăn vặt!", 
                Products = new List<Product>()
                { 
                    new Product() { Id = new Guid("cbe4aaae-386b-4cb7-a7fa-1706e398540c"), Name = "Bánh rán", Price = 5000 },
                    new Product() { Id = new Guid("47976c04-3d1e-4fec-8f2f-04110cb53cf3"), Name = "Bánh bột lọc", Price = 15000 },
                    new Product() { Id = new Guid("cac32fe9-8aee-4ae1-8895-906f32dc95fc"), Name = "Bánh đúc", Price = 20000 }
                }
            },
            new Provider()
            {
                Id = new Guid("19525261-cb12-4671-8fa4-2f9f35ea6d8c"),
                Name = "Sangchanh.com",
                Information = "Chuyên bán đồ ăn sang chảnh!",
                Products = new List<Product>()
                {
                    new Product() { Id = new Guid("3146c312-8d5b-44c4-8556-e77b5e2e099b"), Name = "Bánh Pie bò Wagyu Anh", Price = 353000000 },
                    new Product() { Id = new Guid("c9d7e222-0402-4d13-abd1-2391d165bc45"), Name = "Bánh Fortress Stilt Fisherman Indulgence", Price = 336000000 },
                    new Product() { Id = new Guid("187ba20b-6f17-4fd7-af78-fc6b0ede55a0"), Name = "Bánh Pizza Louis XIII", Price = 266000000 }
                }
            },
            new Provider()
            {
                Id = new Guid("ac70de18-5cc4-410d-9856-7484f79625c9"),
                Name = "Cámlợn.com",
                Information = "Chuyên bán cám lợn!",
                Products = new List<Product>()
                {
                    new Product() { Id = new Guid("33feb3d2-7e54-4b89-b5d6-a29d9f887b66"), Name = "Trà sữa trân châu trắng", Price = 45000 },
                    new Product() { Id = new Guid("7080db2d-c105-498b-a201-7de7bb3fb9a3"), Name = "Trà sữa Oreo Cake Cream", Price = 50000 },
                    new Product() { Id = new Guid("5b1f6d16-3aab-4718-af3f-7a81d4d80877"), Name = "Trà sữa matcha đậu đỏ", Price = 65000 }
                }
            }
        };

        public List<Provider> GetAll()
        {
            return _providers;
        }

        public Provider Get(Guid id)
        {
            return _providers.FirstOrDefault(p => p.Id.Equals(id));
        }
    }
}
