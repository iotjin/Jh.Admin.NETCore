
using Admin.NETCore.API.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);

// Services ≈‰÷√
builder.Services.AddBasicServices(builder.Configuration);
builder.Services.AddDataBaseServices(builder.Configuration);
builder.Services.AddControllerWithValidation();

//if (builder.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("EnableSwagger", false))
//{
//    builder.Services.AddSwaggerConfig();
//}

var app = builder.Build();

app.ConfigureMiddlewarePipeline();

// Swagger
//if (builder.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("EnableSwagger", false))
//{
//    app.UseCustomSwaggerUI();
//}


app.Run();