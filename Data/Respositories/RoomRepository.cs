using HotelServer.Data.Infrastructure;
using HotelServer.Model;
using Microsoft.EntityFrameworkCore;

namespace HotelServer.Data.Respositories
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<IEnumerable<TypeRoom>> GetTypeRooms();
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

        public async Task<IEnumerable<TypeRoom>> GetTypeRooms()
        {
            var typeRoom = await DbContext.TypeRoom.ToListAsync();
            return typeRoom;
        }
    }
}
