using DAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Models
{
    public enum MediaSortBy { Title, PublishDate }

    public class Media : Record
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string YoutubeId { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.Now;

        public int OwnerId { get; set; } = 1;
        public bool Shared { get; set; } = true;
        [JsonIgnore]
        public User Owner => DB.Users.Get(OwnerId).Copy();

        public List<MediaLike> Likes { get; set; } = new List<MediaLike>();
        public int LikesCount => Likes.Count;
        public bool LikedByCurrentUser => Likes.Any(l => l.UserId == User.ConnectedUser.Id);
        public List<string> UserNames => Likes.Select(l => l.UserName ?? "Utilisateur inconnu").ToList();

        public override bool IsValid()
        {
            if (!HasRequiredLength(Title, 1)) return false;
            if (!HasRequiredLength(Category, 1)) return false;
            if (!HasRequiredLength(Description, 1)) return false;
            if (DB.Medias.ToList().Where(m => m.YoutubeId == YoutubeId && m.Id != Id).Any()) return false;
            return true;
        }
    }
}