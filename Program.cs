using System.Text;
using CoreProject.Middlewares;
using firstProject.Models;
using firstProject.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = System
            .Text
            .Encodings
            .Web
            .JavaScriptEncoder
            .UnsafeRelaxedJsonEscaping;
    });
builder.Services.AddShoeConst();
builder.Services.AddUserConst();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();
builder
    .Services.AddAuthentication(options =>
    {
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer",cfg =>
    {
        cfg.RequireHttpsMetadata = false;
        cfg.TokenValidationParameters = UserTokenService.GetTokenValidationParameters();
    });
builder.Services.AddAuthorization(cfg =>
{
    cfg.AddPolicy("admin", policy => policy.RequireClaim("type", "admin"));
    cfg.AddPolicy("user", policy => policy.RequireClaim("type", "user","admin"));
});
builder.Services.AddScoped<ActiveUser>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Core", Version = "v1" });
    var SecurityScheme=new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter JWT with Bearer into field",
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme="bearer",
            BearerFormat = "JWT"
        };
    c.AddSecurityDefinition(
        "Bearer",SecurityScheme
        
    );
    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                SecurityScheme,
                new string[] { }
            },
        }
    );
});


Log.Logger = new LoggerConfiguration()
.MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) 
    .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day,
        fileSizeLimitBytes: 100 * 1024 * 1024,
        rollOnFileSizeLimit: true) 
    .CreateLogger();

// builder.Logging.ClearProviders();
builder.Logging.AddSerilog();



//c=>{
//c.SwaggerDoc("v1", new() { Title = "firstProject", Version = "v1" });}

// builder.Services.AddSingleton<IShoesService,ShoesServiceConst>();
var app = builder.Build();
UserTokenService.InitializeUserService(app.Environment);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core v1"));
    // app.MapScalarApiReference(option=>option.WithTheme())
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
app.UseMiddleware<RequestLoggingMiddleware>();
app.MapControllers();

app.Run();
