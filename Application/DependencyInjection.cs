using Application.Mapper;
using Application.Service;
using Application.Validate;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters(); 
        services.AddValidatorsFromAssemblyContaining<TransactionValidator>();
        MapConfig.RegisterMappings();
        return services;
    }
}