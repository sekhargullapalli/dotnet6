#nullable disable
namespace  MinimalJsonServer;
internal class JsonService
{
    JsonDataSet db = new JsonDataSet();

    internal JsonService(string[] files) => files.ToList().ForEach(f => db.ReadJson(f));
    internal IEnumerable<object> Tables => db.Tables.Keys.Select(k => new { Controller = k, Route = @$"/{k}" });
    internal object Get(string controller) 
    {
        var res =db.Tables.ContainsKey(controller) ?
        db.Tables[controller] :
        new List<JsonDictionary>();
        return new {Count=res.Count(), Data=res};

    }
    internal object Get(string controller, string id) 
    {
        var res = db.Tables.ContainsKey(controller) ?
        db.Tables[controller].Where(d => (d["__id__"].ToString() == id)).FirstOrDefault() :
        null;
        if (res!=null)
            return new {Count=1, Data=res}; 
        else 
            return new {Count=0, Data=new JsonDictionary()}; 
    }      
    internal object Post(string controller, JsonDictionary item)
    {
        if (db.Tables.ContainsKey(controller))
        {
            item.AddIdentity();
            while (db.Tables[controller]
                    .Where(v => v["__id__"].ToString() == item["__id__"].ToString())
                    .Count() > 0)
                    item.AddIdentity(overwrite:true);
            db.Tables[controller].Add(item);
            return new {Add=1, Data=item};
        }
        else
            return new {Add=0, Data=new JsonDictionary()};
    }
    internal object Update(string controller, JsonDictionary item)
    {
        if (!item.ContainsKey("__id__")) return new JsonDictionary();
        if (db.Tables.ContainsKey(controller))
        {
            var itemtoUpdate = db.Tables[controller].Where(d => d["__id__"].ToString() == item["__id__"].ToString()).FirstOrDefault();
            if (itemtoUpdate != null)
            {
                db.Tables[controller].Remove(itemtoUpdate);
                db.Tables[controller].Add(item);
                return new {Update=1, Data=item}; 
            }
        }
        return new {Update=0, Data=new JsonDictionary()}; 
    }
    internal object Upsert(string controller, JsonDictionary item)
    {
        item.AddIdentity();
        if (db.Tables.ContainsKey(controller))
        {
            var itemtoUpdate = db.Tables[controller].Where(d => d["__id__"].ToString() == item["__id__"].ToString()).FirstOrDefault();
            if (itemtoUpdate != null)
            {
                db.Tables[controller].Remove(itemtoUpdate);
                db.Tables[controller].Add(item);
                return new {Add=0,Update=1, Data=item}; 
            }
            else
            {                
                while (db.Tables[controller]
                    .Where(v => v["__id__"].ToString() == item["__id__"].ToString())
                    .Count() > 0)
                    item.AddIdentity(overwrite:true);
                db.Tables[controller].Add(item);
                return new {Add=1,Update=0, Data=item}; 
            }
            
        }
        return new {Add=0,Update=0, Item=new JsonDictionary()};
    }
    internal object Delete(string controller, string id)
    {
        if (db.Tables.ContainsKey(controller))
        {
            var itemtoDelete = db.Tables[controller].Where(d => d["__id__"].ToString() == id).FirstOrDefault();
            if (itemtoDelete != null)
            {
                db.Tables[controller].Remove(itemtoDelete);
                return new {Delete=1, Data=itemtoDelete};                
            }
        }
        return new {Delete=0, Data=new JsonDictionary()};
    }
    internal void Save(string path) => db.WriteJson(path);
}
