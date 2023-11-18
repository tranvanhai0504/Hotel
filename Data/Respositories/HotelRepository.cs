using HotelServer.Data.Infrastructure;
using HotelServer.Model;
using HotelServer.Data.Infrastructure;

namespace HotelServer.Data.Respositories
{
    public interface IHotelRepository : IRepository<Hotel>
    {
        IEnumerable<TypeHotel> GetTypes();
    }
    public class HotelRepository : RepositoryBase<Hotel>, IHotelRepository
    {
        
        public HotelRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }

        public IEnumerable<TypeHotel> GetTypes()
        {
            return DbContext.TypeHotel.ToList();
        }
    }
}
