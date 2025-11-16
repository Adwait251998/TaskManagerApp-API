using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Core.Entities;
using TaskManager.Filters;
using TaskManager.Infastructure;
using TaskManager.Infastructure.Repositories;
using Serilog;
using TaskManager.Infastructure.Services;
var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // read from appsettings.json
    .CreateLogger();

builder.Host.UseSerilog();
//Adding JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // check who created the token
        ValidateAudience = true,// check intended recipient
        ValidateLifetime = true,// check expiration
        ValidateIssuerSigningKey = true,// check signature

        //For Appsettings.json
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


// Add services to the container.
builder.Services.AddAuthorization();
    
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TaskManagerDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
sql => sql.MigrationsAssembly("TaskManager.Infastructure")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
builder.Services.AddScoped<TaskItemService>();
builder.Services.AddScoped<LoginAuth>();
builder.Services.AddScoped<JwtTokenService>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<StatusService>();
builder.Services.AddHostedService<TaskNotificationService>();

//builder.Services.AddScoped<TwilioService>(); //
//builder.Services.AddScoped<SmsService>();
//builder.Services.AddScoped<UserSession>();
builder.Services.AddScoped<IEmailService, TaskManagerEmailService>();

builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<UserSessionFilter>();
builder.Services.AddDistributedMemoryCache();


builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
//builder.Services.AddSession(option =>
//{
//    option.Cookie.Name = "UserSession";
//    option.Cookie.HttpOnly = true;
//    option.Cookie.IsEssential = true;  // Required for GDPR compliance
//    option.IdleTimeout = TimeSpan.FromMinutes(30); // session lifetime
//});

//builder.Services.AddCors(options =>{
//    options.AddPolicy("AllowReactApp", policy => { policy.WithOrigins("https://task-manager-app-blush-six.vercel.app").AllowAnyHeader().AllowAnyMethod(); });
//});
builder.Services.AddCors(options => {
    options.AddPolicy("AllowReactApp", policy => { policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod(); });
});
var app = builder.Build();
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Unhandled exception occurred");
        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("Internal Server Error");
    }
});
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

//// ? Session must be before auth
//app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();