using Newtonsoft.Json;

namespace GraphTest.Models.Graph;

public class ImageResponse
{
    [JsonProperty(PropertyName = "_Image")]
    public ImageResponseType Image { get; set; }
}

public class ImageResponseType
{
    public List<Image> Items { get; set; }
}

public class Image
{
    [JsonProperty(PropertyName = "_metadata")]
    public ImageMetaData MetaData { get; set; }
}

public class ImageMetaData
{
    public string Key { get; set; }
    public string DisplayName { get; set; }
    public ImageUrl Url { get; set; }
}

public class ImageUrl
{
    public string Default { get; set; }
}