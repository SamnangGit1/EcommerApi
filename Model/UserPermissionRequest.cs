namespace Eletronic_Api.Model
{
    public class UserPermissionRequest
    {
        public int UserID { get; set; }
        public List<string>? PermissionName { get; set; }
    }
}
