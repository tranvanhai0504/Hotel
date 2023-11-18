using HotelServer.Data.Infrastructure;
using HotelServer.Model;

namespace HotelServer.Data.Respositories
{
    public interface IRoomRepository : IRepository<Room>
    {

    }
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
