using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Projet_RedSocial_app.Models;

public partial class Comment
{
    public int CommentId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime PublishingDate { get; set; }

    public string? CommentUrl { get; set; }

    public int PostId { get; set; }

    public int MemberId { get; set; }

    public virtual ICollection<CommentDislike> CommentDislikes { get; } = new List<CommentDislike>();

    public virtual ICollection<CommentLike> CommentLikes { get; } = new List<CommentLike>();

    [JsonIgnore]
    public virtual Member? Member { get; set; }

    [JsonIgnore]    
    public virtual Post? Post { get; set; }
}
