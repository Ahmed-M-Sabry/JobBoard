using JobBoard.Application;
using JobBoard.Domain.AuthenticationHepler;
using JobBoard.Infrastructure;
using JobBoard.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .InfrastructureLayerServices()
    .ApplicationLayerServices();

builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection(nameof(JwtSetting)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedAsync(scope.ServiceProvider);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
