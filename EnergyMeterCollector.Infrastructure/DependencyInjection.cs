using EnergyMeterCollector.Application.Abstractions;
using EnergyMeterCollector.Application.Leituras;
using EnergyMeterCollector.Infrastructure.Messaging;
using EnergyMeterCollector.Infrastructure.Modbus;
using EnergyMeterCollector.Infrastructure.Persistence;
using EnergyMeterCollector.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EnergyMeterCollector.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite(config.GetConnectionString("Default")));
        services.AddScoped<IMedidorRepository, MedidorRepository>();
        services.AddScoped<ILeituraRepository, LeituraRepository>();

        services.AddSingleton(config.GetSection("Modbus").Get<ModbusOptions>() ?? new ModbusOptions());
        services.AddScoped<IModbusLeitor, ModbusLeitor>();

        services.AddSingleton(config.GetSection("Mqtt").Get<MqttOptions>() ?? new MqttOptions());
        services.AddScoped<ILeituraPublisher, MqttLeituraPublisher>();
        services.AddScoped<RegistrarLeituraHandler>();
        return services;
    }
}
