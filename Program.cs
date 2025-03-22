using CoreProject.Middlewares;
using firstProject.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options=>{
    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});
builder.Services.AddShoeConst();
builder.Services.AddUserConst();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(//c=>{
    //c.SwaggerDoc("v1", new() { Title = "firstProject", Version = "v1" });}
);
// builder.Services.AddSingleton<IShoesService,ShoesServiceConst>();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
//------------------------------------------
app.UseMailMiddleware();
app.UseLogMiddleware();

app.UseMyErrorMiddleware();
//-----------------------------------------

app.UseDefaultFiles();

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
