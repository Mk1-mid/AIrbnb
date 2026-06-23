using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using RentalPlatform.Application.Interfaces;
using RentalPlatform.Application.UseCases.Identity;
using RentalPlatform.Application.UseCases.Kyc;
using RentalPlatform.Application.UseCases.Reports;
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
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ExportReportUseCase>();
builder.Services.AddScoped<INotificationService, PersistentNotificationService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddHttpClient<IOcrService, GeminiVisionService>();
builder.Services.AddScoped<IOcrService, GeminiVisionService>();
builder.Services.AddScoped<LoginUserUseCase>();
builder.Services.AddScoped<RegisterUserUseCase>();
builder.Services.AddScoped<ProcessKycUseCase>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("OwnerOnly", policy =>
        policy.RequireClaim("role", "Owner"));
});
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

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                if (context.Request.Cookies.TryGetValue("jwt", out var token))
                {
                    context.Token = token;
                }

                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RentalDbContext>();

    db.Database.Migrate();

    if (app.Environment.IsDevelopment())
    {
        db.Database.EnsureCreated();
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
