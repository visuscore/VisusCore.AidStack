using YesSql.Indexes;

namespace VisusCore.AidStack.OrchardCore.Parts.Indexing.Models;

public class ContentPartIndex : MapIndex
{
    public string ContentItemId { get; set; }
    public string ContentItemVersionId { get; set; }
    public string ContentType { get; set; }
    public bool Published { get; set; }
    public bool Latest { get; set; }
}
