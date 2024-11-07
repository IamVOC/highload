
using System.Runtime.Serialization;
using CoreWCF.OpenApi.Attributes;

[DataContract(Name = "CommandContract", Namespace = "http://gateway.spacebattle.ru")]
public class CommandContract
{
    [DataMember(Name = "type", Order = 1)]
    [OpenApiProperty(Description = "Command type")]
    public string ?type { get; set; }

	[DataMember(Name = "game_id", Order = 2)]
    [OpenApiProperty(Description = "ID of game")]
    public string ?game_id { get; set; }

	[DataMember(Name = "game_item_id", Order = 3)]
    [OpenApiProperty(Description = "ID of game item")]
    public string ?game_item_id { get; set; }

	[DataMember(Name = "properties", Order = 4)]
    [OpenApiProperty(Description = "Properties for item")]
    public Dictionary<string, object> ?item_properties { get; set; }
}

