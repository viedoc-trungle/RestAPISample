using TeaBreakApi.Domain;

namespace TeaBreakApi.Controllers.TeaBreaks.v3
{
    public class TeaBreakUpdateRequest
    {
        public Guid Id { get; set; }
        public int NumberOfParticipants { get; set; }
    }

    public class TeaBreakRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfParticipants { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<OrderRequest>? Orders { get; set; }
    }

    public class OrderRequest
    {
        public Guid TeaBreak { get; set; }
        public Guid Provider { get; set; }
        public Guid Product { get; set; }
        public int Quantity { get; set; }
    }

    public class TeaBreakResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfParticipants { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<OrderResponse> Orders { get; set; }
    }

    public class OrderResponse
    {
        public Guid Id { get; set; }
        public string Product { get; set; }
        public string Provider { get; set; }
        public int Quantity { get; set; }
        public float Total { get; set; }
    }
}
