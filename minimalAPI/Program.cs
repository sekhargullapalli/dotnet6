#nullable disable
using MinimalJsonServer;

var builder = WebApplication.CreateBuilder(args);
string directory = (args.Length < 1 || !Directory.Exists(args[0]))
        ? Directory.GetCurrentDirectory() : args[0];
builder.Services.AddSingleton<JsonService>(new JsonService(Directory.GetFiles(directory,"*.json")));
var app = builder.Build();

app.MapGet("/", ([FromServices]JsonService jsonService) 
    => new {Data=jsonService.Tables});
app.MapGet("/{controller}", 
    ([FromServices]JsonService jsonService, string controller) 
    => jsonService.Get(controller));
app.MapGet("/{controller}/{id}", 
    ([FromServices]JsonService jsonService, string controller, string id) 
    => jsonService.Get(controller,id));
app.MapPost("/{controller}", 
    ([FromServices]JsonService jsonService, string controller, [FromBody]JsonDictionary item) 
    => jsonService.Post(controller, item));
app.MapPut("/{controller}", 
    ([FromServices]JsonService jsonService, string controller, [FromBody]JsonDictionary item) 
    => jsonService.Upsert(controller, item));
app.MapDelete("/{controller}/{id}", 
    ([FromServices]JsonService jsonService, string controller, string id) 
    =>jsonService.Delete(controller,id)); 

app.Run();
