using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Eletronic_Api.Repository.Abastract;

namespace Eletronic_Api.Repository.Implementation
{
    public class BrandRepository : IBrandRepository
    {
        private readonly APIContext _context;
        public BrandRepository(APIContext context)
        {
            _context = context;
        }
        public bool Add(Model.Brand brand)
        {
            try
            {
                _context.Brands.Add(brand);
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
