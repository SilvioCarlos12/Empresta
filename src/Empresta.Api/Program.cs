using Empresta.Api.Api;
using Empresta.Aplicacao.Commands;
using Empresta.Ioc;
using Empresta.Ioc.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMapeamento();
builder.Services.AddDbContext();
builder.Services.AddRepositorio();
builder.Services.AddMediator();
builder.Services.AddValidation();
var config = builder.Configuration.GetSection("MongoDB");
builder.Services.AddConfigMongo(config["ConnectionString"]!, config["Database"]!);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapRotas();
app.UseMiddleware<ValidationMiddleware>();
app.Run();

