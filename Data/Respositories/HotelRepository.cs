using HotelServer.Data.Infrastructure;
using HotelServer.Model;
using HotelServer.Data.Infrastructure;
using System.Dynamic;

namespace HotelServer.Data.Respositories
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        IEnumerable<TypeHotel> GetTypes();
        IEnumerable<Room> GetAllRooms(string id);
    }
    public class HotelRepository : RepositoryBase<Hotel>, IHotelRepository
    {
        
        public HotelRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<Room> GetAllRooms(string id)
        {
            return DbContext.Rooms.Where(room => room.HotelId == id).ToList();
        }

        public IEnumerable<TypeHotel> GetTypes()
        {
            return DbContext.TypeHotel.ToList();
        }
    }
}
