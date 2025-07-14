using System;
using System.Collections.Generic;

namespace MSSQLServer.EntitiesModels;

public partial class BlogPost
{
    public int PostId { get; set; }

    public int AuthorId { get; set; }

    public string Title { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string Category { get; set; } = null!;

    public bool? IsPublished { get; set; }

    public DateTime? PublishedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User Author { get; set; } = null!;
}
