using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Paybliss.Consume;
using Paybliss.Data;
using Paybliss.Models;
using Paybliss.Repository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IAuthRepo, AuthConsume>();
builder.Services.AddSingleton<IServiceLogicHelper, ServiceLogicHelper>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<DataContext>(o =>
{
    o.UseNpgsql(builder.Configuration.GetConnectionString("ApiDB"));
});

builder.Services.AddCors(o => o.AddPolicy("policy", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

//JWT
var jwtSettings = builder.Configuration.GetSection("JWTSettings");
builder.Services.Configure<JWTSettings>(jwtSettings);

var authKey = builder.Configuration.GetValue<string>("JWTSettings:Secret");

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    o.RequireHttpsMetadata = true;
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration.GetValue<string>("JWTSettings:Issuer"),
        ValidAudience = builder.Configuration.GetValue<string>("JWTSettings:Audience"),
        ClockSkew = TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("policy");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
