using ExamplesCommonCode.CommonAdmin;

namespace Application.Identity.User
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? DesignationId { get; set; }
    }

    public class UserInfo
    {
        public UserViewModel User { get; set; }

        public AuthUserDisplay AuthUser { get; set; }
    }
}
