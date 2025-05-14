using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Projet_RedSocial_app.Models;

public partial class CommentDislike
{
    public int DisLikeId { get; set; }

    public int? CommentId { get; set; }

    public int? MemberId { get; set; }

    [JsonIgnore]
    public virtual Comment? Comment { get; set; }

    [JsonIgnore]
    public virtual Member? Member { get; set; }
}
