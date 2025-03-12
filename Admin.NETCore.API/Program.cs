
using Admin.NETCore.API.Identity.filters;
using Admin.NETCore.API.ServiceExtensions;
using Admin.NETCore.Core.Interfaces;
using Admin.NETCore.Infrastructure.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseMySql(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
//        x => x.MigrationsAssembly("Admin.NETCore.Infrastructure") // ָ��Ǩ�Ƴ���
//    ));

//builder.Services.AddScoped<IUserService, UserService>();

//// ������ÿ���
//builder.Services.AddCors(c => c.AddPolicy("any", p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));

//// ����Ĭ�ϵ�ģ����֤��Ӧ
//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.SuppressModelStateInvalidFilter = true;
//});

//builder.Services.AddControllers(options =>
//{
//    options.Filters.Add<ValidationFilterAttribute>();
//    options.Filters.Add<CustomValidationFilter>();
//});

//// Add services to the container.
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();


//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseAuthorization();

//app.MapControllers();

//app.Run();


// Services ����
builder.Services.AddBasicServices(builder.Configuration);
builder.Services.AddDataBaseServices(builder.Configuration);
builder.Services.AddControllerWithValidation();
//builder.Services.AddSwaggerConfig();

var app = builder.Build();

app.ConfigureMiddlewarePipeline();

//// Swagger
//app.UseCustomSwaggerUI();

app.Run();

/* 
  ���ݿ�����
 
 
 // ������ݿ�������
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseMySql(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
//    ));

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
//        new MySqlServerVersion(new Version(8, 0, 21))));

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//  ָ��Ǩ�Ƴ���

//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseMySql(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
//        x => x.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)
//    ));
 
 */