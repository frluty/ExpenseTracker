using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace TT.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        
        services.AddMediatR(option =>
        {
          option.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });
        services.AddScoped<IMediator, Mediator>();
    }
}