using Taxually.TechnicalTest;
using Taxually.TechnicalTest.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<IVATRegisterService, VATRegisterService>();
builder.Services.AddScoped<IFormatService, FormatService>();
builder.Services.AddScoped<TaxuallyHttpClient>();
builder.Services.AddScoped<TaxuallyQueueClient>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
