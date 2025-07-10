using Eletronic_Api.Data;
using Eletronic_Api.Model;
using Eletronic_Api.Repository.Abastract;

namespace Eletronic_Api.Repository.Implementation
{
    public class StaffRepository : IStaffRepository
    {
        private readonly APIContext _context;
        public StaffRepository(APIContext context)
        {
            _context = context;
        }
        public bool Add(Staff staff)
        {
            if (staff == null)
            {
                return false;
            }
            _context.staffs.Add(staff);
            _context.SaveChanges();
            return true;
        }
    }
}
