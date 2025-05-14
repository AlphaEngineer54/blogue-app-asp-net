using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Projet_RedSocial_app.Models;

public partial class Member { 
    public int MemberId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreationDate { get; set; }

    public bool IsOnline { get; set; }

    public virtual ICollection<Blogue> Blogues { get; } = new List<Blogue>();

    public virtual ICollection<CommentDislike> CommentDislikes { get; } = new List<CommentDislike>();

    public virtual ICollection<CommentLike> CommentLikes { get; } = new List<CommentLike>();

    public virtual ICollection<Comment> Comments { get; } = new List<Comment>();

    public virtual ICollection<PostDislike> PostDislikes { get; } = new List<PostDislike>();

    public virtual ICollection<PostLike> PostLikes { get; } = new List<PostLike>();

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}
