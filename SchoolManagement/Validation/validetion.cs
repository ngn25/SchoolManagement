    using System.Text.RegularExpressions;
    
    namespace SchoolManagement.Validation
{
    public static class Validation
    {
        private static readonly Regex EmailRegex = new Regex(
            @"^[\w!#$%&'*+/=?`{|}~^-]+(?:\.[\w!#$%&'*+/=?`{|}~^-]+)*@(?:[a-zA-Z0-9-]+\.)+[a-zA-Z]{2,}$",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex PhoneRegex = new Regex(
            @"^09\d{9}$",
            RegexOptions.Compiled);

        public static void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null or empty.", nameof(email));

            email = email.Trim();

            if (!EmailRegex.IsMatch(email))
                throw new ArgumentException("The email format is invalid.", nameof(email));
        }


        public static void ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be null or empty.", nameof(phoneNumber));

            string cleaned = phoneNumber
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "");

            cleaned = cleaned
                .Replace("۰", "0").Replace("۱", "1").Replace("۲", "2").Replace("۳", "3").Replace("۴", "4")
                .Replace("۵", "5").Replace("۶", "6").Replace("۷", "7").Replace("۸", "8").Replace("۹", "9");

            if (!PhoneRegex.IsMatch(cleaned))
                throw new ArgumentException(
                    "Phone number must start with '09' and contain exactly 11 digits.",
                    nameof(phoneNumber));
        }
    }
}

