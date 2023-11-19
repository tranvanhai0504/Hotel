namespace HotelServer.Common
{
    public class SupportFunctions
    {
        public static string HashPassword(string password)
        {
            int salt = 12;
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }

        public static bool ComparePassword(string nativePassword, string hashPassword)
        {
            return BCrypt.Net.BCrypt.Verify(nativePassword, hashPassword);
        }

        public static string GeneralId(string head)
        {
            return head + DateTime.Now.ToString("yyyyMMddHHmmssffffff");
        }
    }
}
