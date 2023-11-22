using HotelServer.Data.Infrastructure;
using HotelServer.Model;
using HotelServer.Data.Infrastructure;
using System.Dynamic;
using System.Data.Entity;

namespace HotelServer.Data.Respositories
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        IEnumerable<TypeHotel> GetTypes();
        Task<IEnumerable<Room>> GetAllRooms(string id);
    }
    public class HotelRepository : RepositoryBase<Hotel>, IHotelRepository
    {
        
        public HotelRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<IEnumerable<Room>> GetAllRooms(string id)
        {
            var rooms = await DbContext.Rooms.Where(room => room.HotelId == id).ToListAsync();
            return rooms;
        }

        public IEnumerable<TypeHotel> GetTypes()
        {
            return DbContext.TypeHotel.ToList();
        }
    }
}
