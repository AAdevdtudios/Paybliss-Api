using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Paybliss.Consume;
using Paybliss.Data;
using Paybliss.Models;
using Paybliss.Repository;
using System.Text;
using Paybliss.Controllers;
using Reloadly.Airtime;
using Reloadly.Core.Enums;
using Hangfire;
using Hangfire.PostgreSql;
using Paybliss.Repository.ServicesRepo;
using System.Text.Json.Serialization;
using Paybliss.Repository.BlocVTU;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IAuthRepo, AuthConsume>();
builder.Services.AddSingleton<IServiceLogicHelper, ServiceLogicHelper>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<IVtuService, VtuServices>();
builder.Services.AddScoped<IBLOCService, BLOCServiice>();
builder.Services.AddScoped<IBlocVTUService, BlocVTUService>();
//builder.Services.AddTransient<IBlocService, BlocService>();

//Third perty
/*GlobalConfiguration.Configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings();*/
builder.Services.AddHangfire(x => x.UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
    .UsePostgreSqlStorage(builder.Configuration.GetConnectionString("ApiDB")));

builder.Services.AddHangfireServer();

/*builder.Services.AddScoped<IVtuService, VtuServices>();*/
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IAirtimeApi, AirtimeApi>(
    sp => new AirtimeApi(
        sp.GetRequiredService<IHttpClientFactory>(),
        "CvKhJSeo5jlANLbGgRC8bdWksf606Pjp", "FIF7E35vNA-enOyxiSVLELMvD16a87-oOAqEVBt93g8w9r4jNnDACG1sSbgi7Hj",
        ReloadlyEnvironment.Sandbox,
        disableTelemetry: true));


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
var anyPolicy = "_MyAllowSubdomainPolicy";

var authKey = builder.Configuration.GetValue<string>("JWTSettings:Secret");
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: anyPolicy,
        policy =>
        {
            policy.WithOrigins("*")
                .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
});


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
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/
app.UseCors();
//app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("policy");
app.UseHttpsRedirection();
app.UseHangfireDashboard();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
