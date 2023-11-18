namespace HotelServer.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}
