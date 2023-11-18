namespace HotelServer.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        HotelDbContext dbContext;
        public HotelDbContext Init()
        {
            return dbContext ??= new HotelDbContext();
        }

        protected override void DisposeCore()
        {
            if(dbContext != null)
            {
                dbContext.Dispose();
            }
        }
    }
}
