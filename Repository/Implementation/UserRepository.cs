using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Eletronic_Api.Repository.Abastract;

namespace Eletronic_Api.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly APIContext _context;
        public UserRepository(APIContext context)
        {
            _context = context;
        }
        public bool Add(User user)
        {
            try
            {
                _context.Users.Add(user);
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
