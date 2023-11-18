using HotelServer.Data.Infrastructure;
using HotelServer.Model;

namespace HotelServer.Data.Respositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        IEnumerable<TypeRoom> GetTypeRooms();
        TypeRoom GetTypeOfRoom(string idType);
    }
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public TypeRoom GetTypeOfRoom(string idType)
        {
            var typeRoom = DbContext.TypeRoom.ToList().Find(typeRoom => typeRoom.Id == idType);
            return typeRoom;
        }

        public IEnumerable<TypeRoom> GetTypeRooms()
        {
            var typeRoom = DbContext.TypeRoom.ToList();
            return typeRoom;
        }
    }
}
