using HotelServer.Data.Infrastructure;
using HotelServer.Model;
using System.Linq.Expressions;

namespace HotelServer.Data.Respositories
{
    public interface IBillRepository : IRepository<Bill>
    {
        void ChangeRoom(Bill bill, string idRoom);
        
    }
    public class BillRepository : RepositoryBase<Bill>, IBillRepository
    {
        public BillRepository(IDbFactory dbFactory) : base(dbFactory)
        {
            
        }

        public void ChangeRoom(Bill bill, string idRoom)
        {
            bill.RoomId = idRoom;
            DbContext.Bills.Update(bill);
        }
    }
}
