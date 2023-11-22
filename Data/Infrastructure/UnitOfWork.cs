namespace HotelServer.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbFactory dbFactory;
        private HotelDbContext dbContext;

        public UnitOfWork(IDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public HotelDbContext DbContext
        {
            get { return dbFactory.Init(); }
        }

        public void Commit() { 
            DbContext.SaveChanges();
        }
    }
}
