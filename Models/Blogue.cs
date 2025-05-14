using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Projet_RedSocial_app.Models;

public partial class Blogue
{
    public int BlogueId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreationDate { get; set; }

    public string? Categorie { get; set; }

    public int? MemberId { get; set; }

    [JsonIgnore]
    public virtual Member? Member { get; set; }

    public virtual ICollection<Post> Posts { get; } = new List<Post>();
}
