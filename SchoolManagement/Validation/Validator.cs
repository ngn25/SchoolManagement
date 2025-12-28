namespace SchoolManagement.Validation
{
    public static class Validator
    {
        public static void ValidateEmail(string Email)
        {
            if (string.IsNullOrWhiteSpace(Email))
                throw new ArgumentException("Email cannot be empty!");

            if (!Email.Contains('@'))
                throw new ArgumentException("Email must contain '@'");
        }

        public static void ValidatePhoneNumber(string PhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(PhoneNumber))
                throw new ArgumentException("Phone cannot be empty!");

       
            PhoneNumber = PhoneNumber.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

  
            PhoneNumber = PhoneNumber
                .Replace("۰","0").Replace("۱","1").Replace("۲","2")
                .Replace("۳","3").Replace("۴","4").Replace("۵","5")
                .Replace("۶","6").Replace("۷","7").Replace("۸","8").Replace("۹","9");

            if (PhoneNumber.Length != 11 || !PhoneNumber.StartsWith("09"))
                throw new ArgumentException("Phone number must be 11 digits and start with '09'");
        }
    }
}