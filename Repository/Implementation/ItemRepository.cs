using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Eletronic_Api.Repository.Abastract;

namespace Eletronic_Api.Repository.Implementation
{
    public class ItemRepository : IItemRepository   
    {
        private readonly APIContext _context;
        public ItemRepository(APIContext context)
        {
            _context = context;
        }
        public bool Add(Model.Item item)
        {
            try
            {
                _context.Items.Add(item);
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
