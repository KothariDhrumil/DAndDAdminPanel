using Infrastructure;
using Application;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddHttpContextAccessor();


builder.Services.AddCors(options =>
{
    options.AddPolicy(MyAllowSpecificOrigins,
        builder =>
        {
            builder.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            //builder.WithOrigins("http://localhost:4200/").AllowAnyMethod();
            //builder.WithOrigins("https://localhost:4200").AllowAnyMethod();
            //builder.WithOrigins("https://localhost:4200/").AllowAnyMethod();
        });
});

builder.Services.AddInfrastructure(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);
app.UseInfrastructure(builder.Configuration);
app.MapEndpoints();
app.Run();
