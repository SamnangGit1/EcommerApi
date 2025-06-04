using Eletronic_Api.Data;
using Eletronic_Api.Repository.Abastract;

namespace Eletronic_Api.Repository.Implementation
{
    public class CategoryRepository :ICategoryRepository
    {
        private readonly APIContext _context;
        public CategoryRepository(APIContext context)
        {
            _context = context;
        }
        public bool Add(Model.Category category)
        {
            try
            {
                _context.Categories.Add(category);
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
