namespace HotelServer.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        HotelDbContext Init();

    }
}
