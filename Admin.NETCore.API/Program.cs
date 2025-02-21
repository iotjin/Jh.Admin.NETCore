using Admin.NETCore.BLL.Interfaces;
using Admin.NETCore.BLL;

var builder = WebApplication.CreateBuilder(args);



//∫Û∂À≈‰÷√øÁ”Ú
builder.Services.AddCors(c => c.AddPolicy("any", p => p.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
builder.Services.AddSingleton<IUserBLL, UserBLL>(); //“¿¿µ◊¢»Î IUsersBLL

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
