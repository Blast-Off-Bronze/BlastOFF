namespace BlastOFF.Models.UserModel
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager,
            string authType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authType);

            return userIdentity;
        }
    }
}
