using Microsoft.EntityFrameworkCore;
using Social.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using Social.Application.Interfaces;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Social.Application.Services;
using Social.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

//  Configuration JWT 
var jwtSecret = builder.Configuration["JwtSettings:Secret"] 
    ?? throw new InvalidOperationException("JWT Secret non configuré dans la configuration");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero 
    };
});


// les postes
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<CreatePostService>();
builder.Services.AddScoped<GetAllPostsService>();
builder.Services.AddScoped<GetUserPostsService>();
builder.Services.AddScoped<UpdatePostService>();  
builder.Services.AddScoped<DeletePostService>();  

//les users
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<RegisterUserService>();



// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Configure HTTPS redirection port (matches Properties/launchSettings.json https profile)
builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 7225;
});

// Configure le logging pour afficher les logs dans la console
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Affiche les logs dans la console

var app = builder.Build();
using (var scope = app.Services.CreateScope())                                      // Apply migrations at startup
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


// Only enable HTTPS redirection when the server is actually configured with an HTTPS address
var server = app.Services.GetService<IServer>();
var addresses = server?.Features.Get<IServerAddressesFeature>()?.Addresses;
if (addresses != null && addresses.Any(a => a.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
{
    app.UseHttpsRedirection();
}

// Enable authentication middleware before authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
