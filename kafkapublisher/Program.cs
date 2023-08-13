using kafkapublisher;
using kafkapublisher.Kafka;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureAppConfiguration((context, config) =>
{
    //config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
    //config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: false);
    config.AddJsonFile("config/appsettings.k8s.json", optional: true, reloadOnChange: false);
});

//var kafkaSettings = builder.Configuration.GetSection("KafkaSettings").Get<producerSettings>();
//if (kafkaSettings == null)
//{
//    throw new ArgumentNullException(nameof(kafkaSettings));
//}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<IProducer, Producer>(x => new Producer(kafkaSettings));
builder.Services.AddSingleton<IProducer, Producer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
