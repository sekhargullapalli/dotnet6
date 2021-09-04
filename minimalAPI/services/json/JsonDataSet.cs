#nullable disable
namespace  MinimalJsonServer;
internal class JsonDataSet
{
    internal Dictionary<string, ICollection<JsonDictionary>> OriginalTables { get; set; }
        = new Dictionary<string, ICollection<JsonDictionary>>(StringComparer.OrdinalIgnoreCase);
    internal Dictionary<string, ICollection<JsonDictionary>> Tables { get; set; }
    = new Dictionary<string, ICollection<JsonDictionary>>(StringComparer.OrdinalIgnoreCase);
    internal void ReadJson(string path)
    {
        if (File.Exists(path))
        {
            try
            {
                JsonSerializer.Deserialize<Dictionary<string, ICollection<JsonDictionary>>>(File.ReadAllText(path))
                .ToList()
                .ForEach(x =>
                {
                    if (!Tables.ContainsKey(x.Key))
                    {
                        Tables.Add(x.Key, x.Value);                        
                        AssignIDs(x.Key);
                    }
                    else
                        x.Value.ToList().ForEach(i =>
                        {
                            i.AddIdentity();
                            while (Tables[x.Key].Where(v => v["__id__"].ToString() == i["__id__"].ToString()).Count() > 0)
                                i.AddIdentity(overwrite:true);
                            Tables[x.Key].Add(i);                            
                        });
                });
            }
            catch (System.Text.Json.JsonException) { }            
        }
    }
    internal void WriteJson(string saveTo) => File.WriteAllText(saveTo, JsonSerializer.Serialize(Tables));    

    void AssignIDs(string collection)
    {
        List<string> reserveIDs = new List<string>();
        foreach (var item in Tables[collection])
        {
            item.AddIdentity();
            while (reserveIDs.Contains(item["__id__"].ToString()))
                   item.AddIdentity(overwrite:true);       
            reserveIDs.Add(item["__id__"].ToString());     
        }
    }
}
