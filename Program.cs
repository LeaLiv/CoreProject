using System.Text;
using CoreProject.Middlewares;
using firstProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
});
builder.Services.AddShoeConst();
builder.Services.AddUserConst();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.TokenValidationParameters = UserTokenService.GetTokenValidationParameters();

    });
builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy("admin",
    policy => policy.RequireClaim("type", "admin"));
    cfg.AddPolicy("user",
    policy => policy.RequireClaim("type", "user"));

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Core", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
    { new OpenApiSecurityScheme
    {
     Reference = new OpenApiReference { 
        Type = ReferenceType.SecurityScheme,
         Id = "Bearer"}
    },
    new string[] {}
    }
});
});
//c=>{
//c.SwaggerDoc("v1", new() { Title = "firstProject", Version = "v1" });}

// builder.Services.AddSingleton<IShoesService,ShoesServiceConst>();
var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core v1"));
}
//------------------------------------------
// app.UseMailMiddleware();
// app.UseLogMiddleware();

// app.UseMyErrorMiddleware();
//-----------------------------------------


app.UseDefaultFiles();

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
