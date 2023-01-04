using TeaBreakApi.Domain;

namespace TeaBreakApi.Data
{
    public class TeabreakRepository
    {
        private List<TeaBreak> _teaBreaks = new List<TeaBreak>();

        public List<TeaBreak> GetAll()
        { 
            return _teaBreaks;
        }

        public TeaBreak Add(TeaBreak teaBreak)
        {
            var existingBreak = _teaBreaks.Where(b => b.Name.Equals(teaBreak.Name, StringComparison.OrdinalIgnoreCase) && b.StartTime.Equals(teaBreak.StartTime) && b.EndTime.Equals(teaBreak.EndTime)).SingleOrDefault();
            if (existingBreak is not null)
                return existingBreak;

            teaBreak.Id = Guid.NewGuid();
            _teaBreaks.Add(teaBreak);

            return teaBreak;
        }

        public TeaBreak Get(Guid id)
        { 
            return _teaBreaks.FirstOrDefault(b => b.Id == id);
        }

        public TeaBreak Update(TeaBreak teaBreak)
        {
            return teaBreak;
        }
    }
}
