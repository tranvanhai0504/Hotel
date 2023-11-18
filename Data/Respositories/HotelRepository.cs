using HotelServer.Data.Infrastructure;
using HotelServer.Model;
using HotelServer.Data.Infrastructure;
using System.Dynamic;

namespace HotelServer.Data.Respositories
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        IEnumerable<TypeHotel> GetTypes();
        IEnumerable<ExpandoObject> GetAllRooms(string id);
    }
    public class HotelRepository : RepositoryBase<Hotel>, IHotelRepository
    {
        
        public HotelRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<ExpandoObject> GetAllRooms(string id)
        {
            var rooms = DbContext.Rooms.Where(room => room.HotelId == id).ToList();
            var roomTypes = DbContext.TypeRooms.ToList();
            var listRooms = new List<ExpandoObject>();

            foreach (Room room in rooms)
            {
                dynamic a = new ExpandoObject();
                a.Name = roomTypes.Find(type => type.Id == room.QuantityId).Name;
                a.Id = room.Id;


            }
            return listRooms;
        }

        public IEnumerable<TypeHotel> GetTypes()
        {
            return DbContext.TypeHotel.ToList();
        }
    }
}
