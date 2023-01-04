namespace TeaBreakApi.Domain
{
    public class Provider
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public List<Product> Products { get; set; }
    }

    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
    }

    public class Order
    {
        public Guid Id { get; set; }
        public Guid Product { get; set; }
        public Guid Provider { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }
    }
}
