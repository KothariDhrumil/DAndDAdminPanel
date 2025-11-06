namespace Application.Domain.TeantUser.Update
{
    public class UpdateTenantUserModel
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }

        public Guid UserId { set; get; }
        public int UserTypeId { get; set; }
    }
}
