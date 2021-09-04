#nullable disable
namespace  MinimalJsonServer;
internal class JsonDictionary : Dictionary<string, object>
{
    public JsonDictionary() : base(StringComparer.OrdinalIgnoreCase) { }
}
