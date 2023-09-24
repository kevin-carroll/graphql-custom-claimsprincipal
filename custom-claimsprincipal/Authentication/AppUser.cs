namespace GraphQL.AspNet.Examples.CustomClaimsPrincipal.Authentication
{
    public class AppUser
    {
        public AppUser(string id, string username, string employeeId)
        {
            this.Id = id;
            this.UserName = username;
            this.EmployeeId = employeeId;
        }

        public string Id { get; }

        public string UserName { get; set; }

        public string EmployeeId { get; set; }
    }
}