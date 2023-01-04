namespace TeaBreakApi.Domain
{
    public class TeaBreak
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfParticipants { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public List<Order> Orders { get; set; } = new();
    }
}
