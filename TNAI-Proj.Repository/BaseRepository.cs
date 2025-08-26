
using TNAI_Proj.Model;

namespace TNAI_Proj.Repository
{
    public abstract class BaseRepository
    {
        protected ApplicationDbContext DbContext;

        public BaseRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
