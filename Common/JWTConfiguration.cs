namespace HotelServer.Common
{
    public class JWTConfiguration
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string IssuerSigningKey { get; set; }
    }
}
