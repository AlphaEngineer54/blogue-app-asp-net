using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Projet_RedSocial_app.Models;

public partial class PostDislike
{
    public int DisLikeId { get; set; }

    public int? PostId { get; set; }

    public int? MemberId { get; set; }

    [JsonIgnore]
    public virtual Member? Member { get; set; }

    [JsonIgnore]
    public virtual Post? Post { get; set; }
}
