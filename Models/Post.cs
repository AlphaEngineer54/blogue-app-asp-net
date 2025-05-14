using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Projet_RedSocial_app.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public DateTime PublishingDate { get; set; }

    public string? PostUrl { get; set; }

    public int BlogueId { get; set; }

    public int? MemberId { get; set; }

    [JsonIgnore]
    public virtual Blogue? Blogue { get; set; }

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    [JsonIgnore]
    public virtual Member? Member { get; set; }

    public virtual ICollection<PostDislike> PostDislikes { get; } = new List<PostDislike>();

    public virtual ICollection<PostLike> PostLikes { get; } = new List<PostLike>();
}
