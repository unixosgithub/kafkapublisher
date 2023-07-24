using kafkapublisher;
using kafkapublisher.Kafka;

var builder = WebApplication.CreateBuilder(args);

var kafkaSettings = builder.Configuration.GetSection("KafkaSettings").Get<producerSettings>();
if (kafkaSettings == null)
{
    throw new ArgumentNullException(nameof(kafkaSettings));
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IProducer, Producer>(x => new Producer(kafkaSettings));

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
