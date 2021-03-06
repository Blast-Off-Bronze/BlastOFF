﻿namespace BlastOFF.Models.UserModel
{
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;

    using BlastModels;

    using System;
    using BlastOFF.Models.ChatModels;
    using BlastOFF.Models.Enumerations;
    using BlastOFF.Models.GalleryModels;
    using BlastOFF.Models.MusicModels;

    public class ApplicationUser : IdentityUser
    {
        private ICollection<Blast> blasts;
        private ICollection<Comment> comments;

        //liked collections
        private ICollection<Song> likedSongs;
        private ICollection<Image> likedImages;
        private ICollection<Blast> likedBlasts;
        private ICollection<MusicAlbum> likedMusicAlbums;
        private ICollection<ImageAlbum> likedImageAlbums;
        private ICollection<Comment> likedComments;

        //follow collections
        private ICollection<ApplicationUser> followedUsers;
        private ICollection<ApplicationUser> followedBy;
        private ICollection<ImageAlbum> followedgImageAlbums;
        private ICollection<MusicAlbum> followedMusicAlbums;

        private ICollection<Message> sentMessages;
        private ICollection<Message> receivedMessages;

        private ICollection<Notification> notifications; 

        public ApplicationUser()
        {
            this.blasts = new HashSet<Blast>();
            this.comments = new HashSet<Comment>();
            
            this.likedSongs = new HashSet<Song>();
            this.likedImages = new HashSet<Image>();
            this.likedBlasts = new HashSet<Blast>();
            this.likedMusicAlbums = new HashSet<MusicAlbum>();
            this.likedImageAlbums = new HashSet<ImageAlbum>();
            this.likedComments = new HashSet<Comment>();

            this.followedBy = new HashSet<ApplicationUser>();
            this.followedUsers = new HashSet<ApplicationUser>();
            this.followedMusicAlbums = new HashSet<MusicAlbum>();
            this.followedgImageAlbums = new HashSet<ImageAlbum>();

            this.sentMessages = new HashSet<Message>();
            this.receivedMessages = new HashSet<Message>();

            this.notifications = new HashSet<Notification>();
        }

        public DateTime? BirthDate { get; set; }

        public string ProfileImage { get; set; }

        public Gender Gender { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Blast> Blasts
        {
            get { return this.blasts; }
            set { this.blasts = value; }                
        }

        public virtual ICollection<Comment> Comments
        {
            get { return this.comments; }
            set { this.comments = value; }
        }

        public virtual ICollection<Notification> Notifications
        {
            get { return this.notifications; }
            set { this.notifications = value; }
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

        public virtual ICollection<ImageAlbum> FollowedImageAlbums
        {
            get { return this.followedgImageAlbums; }
            set { this.followedgImageAlbums = value; }
        }

        public virtual ICollection<MusicAlbum> FollowedMusicAlbums
        {
            get { return this.followedMusicAlbums; }
            set { this.followedMusicAlbums = value; }
        }

        public virtual ICollection<Song> LikedSongs
        {
            get { return this.likedSongs; }
            set { this.likedSongs = value; }
        }

        public virtual ICollection<Image> LikedImages
        {
            get { return this.likedImages; }
            set { this.likedImages = value; }
        }

        public virtual ICollection<Blast> LikedBlasts
        {
            get { return this.likedBlasts; }
            set { this.likedBlasts = value; }
        }

        public virtual ICollection<MusicAlbum> LikedMusicAlbums
        {
            get { return this.likedMusicAlbums; }
            set { this.likedMusicAlbums = value; }
        }

        public virtual ICollection<ImageAlbum> LikedImageAlbums
        {
            get { return this.likedImageAlbums; }
            set { this.likedImageAlbums = value; }
        }

        public virtual ICollection<Comment> LikedComments
        {
            get { return this.likedComments; }
            set { this.likedComments = value; }
        }

        public virtual ICollection<Message> SentMessages
        {
            get { return this.sentMessages; }
            set { this.sentMessages = value; }
        }

        public virtual ICollection<Message> ReceivedMessages
        {
            get { return this.receivedMessages; }
            set { this.receivedMessages = value; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            userIdentity.AddClaim(new Claim(ClaimTypes.Name, userIdentity.Name));
            userIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, this.Id));

            // Add custom user claims here
            return userIdentity;
        }
    }
}
