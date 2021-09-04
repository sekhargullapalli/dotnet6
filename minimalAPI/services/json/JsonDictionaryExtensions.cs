#nullable disable
namespace  MinimalJsonServer;
internal static class JsonDictionaryExtensions
{
    internal static void AddIdentity(this JsonDictionary item, bool overwrite = false)
    {
        if (!item.ContainsKey("__id__"))
            item.Add("__id__", Guid.NewGuid());
        else
        {
            if (overwrite ||!Guid.TryParse(item["__id__"].ToString(), out var guid))
                item["__id__"] = Guid.NewGuid();
            else
                item["__id__"] = guid;
        }
        var keys = item.Keys.Where(k => k.ToLower() == "__id__").ToList();
        if (keys.Count > 1)
        {
            foreach (var key in keys)
                item.Remove(key);
            AddIdentity(item);
        }
    }
}
