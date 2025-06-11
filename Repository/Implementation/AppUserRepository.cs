using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Eletronic_Api.Repository.Abastract;

namespace Eletronic_Api.Repository.Implementation
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly APIContext _context;
        public AppUserRepository(APIContext context)
        {
            _context = context;
        }
        public bool Add(AppUser appUser)
        {
            try
            {
                _context.AppUsers.Add(appUser);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
