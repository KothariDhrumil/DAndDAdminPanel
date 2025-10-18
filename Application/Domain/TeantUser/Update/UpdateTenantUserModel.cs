namespace Application.Domain.TeantUser.Update
{
    public class UpdateTenantUserModel
    {
        public string FirstName { set; get; }
        public string LastName { set; get; }

        public Guid GlobalUserId { set; get; }
    }
}
