using Newtonsoft.Json;

namespace GraphTest.Models.Graph;

public class MetaData
{
    public string Key { get; set; }
}

public class ArticleContractResponse
{ 
    public ArticleContractType ArticleContract { get; set; }
}

public class ArticleContractType
{
    public List<ArticleContract> Items { get; set; }
}

public class ArticleContract
{
    [JsonProperty(PropertyName = "_metadata")]
    public MetaData MetaData { get; set; }

    public string Excerpt { get; set; }
}
