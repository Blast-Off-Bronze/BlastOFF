namespace BlastOFF.Models.UserModel
{
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    using BlastModels;

    public class ApplicationUser : IdentityUser
    {
        private ICollection<Blast> blasts;
        private ICollection<BlastComment> comments;
        private ICollection<ApplicationUser> followedUsers;
        private ICollection<ApplicationUser> followedBy;

        public ApplicationUser()
        {
            this.blasts = new HashSet<Blast>();
            this.comments = new HashSet<BlastComment>();
            this.followedBy = new HashSet<ApplicationUser>();
            this.followedUsers = new HashSet<ApplicationUser>();
        }

        public virtual ICollection<Blast> Blasts
        {
            get { return this.blasts; }
            set { this.blasts = value; }                
        }

        public virtual ICollection<BlastComment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<ApplicationUser> FollowedUsers
        {
            get { return this.followedUsers; }
            set { this.followedUsers = value; }
        }

        public virtual ICollection<ApplicationUser> FollowedBy
        {
            get { return this.followedBy; }
            set { this.followedBy = value; }
        }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager,
            string authType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authType);

            return userIdentity;
        }
    }
}
