namespace SchoolManagement.Validation
{
    public static class Validator
    {
        public static bool ValidateEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
                return false;

            if (!Email.Contains('@'))
                return false;

            return true;
        }

        public static bool ValidatePhoneNumber(string PhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber))
                return false;

            PhoneNumber = PhoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            PhoneNumber = PhoneNumber
                .Replace("۰","0").Replace("۱","1").Replace("۲","2")
                .Replace("۳","3").Replace("۴","4").Replace("۵","5")
                .Replace("۶","6").Replace("۷","7").Replace("۸","8").Replace("۹","9");

            if (PhoneNumber.Length != 11 || !PhoneNumber.StartsWith("09"))
                return false;

            return true;
        }
    }
}
