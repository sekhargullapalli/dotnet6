#nullable disable
using MinimalJsonServer;

var builder = WebApplication.CreateBuilder(args);
string directory = (args.Length < 1 || !Directory.Exists(args[0]))
        ? Directory.GetCurrentDirectory() : args[0];
builder.Services.AddSingleton<JsonService>(new JsonService(Directory.GetFiles(directory,"*.json")));
var app = builder.Build();

app.MapGet("/", (JsonService jsonService) 
    => new {Data=jsonService.Tables});
app.MapGet("/{controller}", 
    (JsonService jsonService, string controller) 
    => jsonService.Get(controller));
app.MapGet("/{controller}/{id}", 
    (JsonService jsonService, string controller, string id) 
    => jsonService.Get(controller,id));
app.MapPost("/{controller}", 
    (JsonService jsonService, string controller, JsonDictionary item) 
    => jsonService.Post(controller, item));
app.MapPut("/{controller}", 
    (JsonService jsonService, string controller, JsonDictionary item) 
    => jsonService.Upsert(controller, item));
app.MapDelete("/{controller}/{id}", 
    (JsonService jsonService, string controller, string id) 
    => jsonService.Delete(controller,id)); 

app.Run();
