using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Application.UseCases.Identity;
using RentalPlatform.Application.UseCases.Kyc;
using RentalPlatform.Application.UseCases.Reservations;
using RentalPlatform.Infrastructure.Persistence;
using RentalPlatform.Infrastructure.Persistence.Repositories;
using RentalPlatform.Infrastructure.Kyc;
using RentalPlatform.Infrastructure.Identity;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<RentalDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IKycRepository, KycRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<INotificationService, NoOpNotificationService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IOcrService, TesseractOcrService>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<RegisterUserUseCase>();
builder.Services.AddScoped<ProcessKycUseCase>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var key = builder.Configuration["Jwt:Key"] ?? "dev-secret-change-me";
        var issuer = builder.Configuration["Jwt:Issuer"] ?? "RentalPlatform";
        var audience = builder.Configuration["Jwt:Audience"] ?? "RentalPlatformClients";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RentalDbContext>();

    if (app.Environment.IsDevelopment())
    {
        db.Database.Migrate();
    }

    await SeedData.InitializeAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
