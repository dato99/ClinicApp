using ClinicApp.Auth;
using ClinicApp.Filter;
using ClinicApp.Packages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Oracle.ManagedDataAccess.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers(config =>
//{
//    config.Filters.Add(new UnhandledExceptionFilter());
//});





builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
var connectionString = builder.Configuration.GetConnectionString("OrclConnStr");

builder.Services.AddSingleton(new OracleConnection(connectionString));

builder.Services.AddControllers();




builder.Services.Configure<FormOptions>(options =>
{
    // Set maximum size of form data (including file uploads). Default is 128 MB.
    options.MultipartBodyLengthLimit = 100 * 1024 * 1024; // 100 MB
    options.ValueLengthLimit = int.MaxValue; // Set the maximum length of values in the form data
    options.MultipartHeadersLengthLimit = int.MaxValue; // Set max length of headers
});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IJwtManager, JwtManager>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllCors", config =>
    {
        config.AllowAnyOrigin().AllowAnyMethod().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
});
builder.Services.AddScoped<GlobalExceptionFilter>();
builder.Services.AddControllers(config =>
{
    config.Filters.AddService(typeof(GlobalExceptionFilter));
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
});
builder.Services.AddAuthentication(/* your authentication setup */);

builder.Services.AddScoped<IPKG_USERS_D, PKG_USERS_D>();
builder.Services.AddScoped<IPKG_DOCTORS_D, PKG_DOCTORS_D>();
builder.Services.AddScoped<IPKG_DOCTORS_D, PKG_DOCTORS_D>();
builder.Services.AddScoped<IPKG_DSH_CATEGORY, PKG_DSH_CATEGORY>();
//builder.Services.AddScoped<UserRepository>(); // Register UserRepository

builder.Services.AddScoped<IPKG_LOGS, PKG_LOGS>();



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
