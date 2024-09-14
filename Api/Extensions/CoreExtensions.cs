using Core.Interfaces.Services;
using Core.Services;
using Core.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Api.Extensions
{
    public static class CoreExtensions
    {
            public static IServiceCollection ConfigureServices(this IServiceCollection services)
            {
                 return services
                .AddServices()
                .AddValidators();
            }

            public static IServiceCollection AddServices(this IServiceCollection services)
            {
            return services.AddScoped<IScheduleService, ScheduleService>();
            }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddValidatorsFromAssemblyContaining<ScheduleValidator>();
        }

    }
    }
