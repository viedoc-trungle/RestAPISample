using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace TeaBreakApi.Domain
{
    public class TeabreakTime : ValueObject
    {
        public string Value { get; }

        private TeabreakTime(string value)
        {
            Value = value;
        }

        public static Result<TeabreakTime, Error> Create(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Errors.General.ValueIsRequired();

            string email = input.Trim();

            if (email.Length > 150)
                return Errors.General.InvalidLength();

            if (Regex.IsMatch(email, @"^(.+)@(.+)$") == false)
                return Errors.General.ValueIsInvalid();

            return new TeabreakTime(email);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
