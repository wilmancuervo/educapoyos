using EduApoyos.Application;
using EduApoyos.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace EduApoyos.Tests.Common;

public class DependencyInjectionTests
{
    [Fact]
    public void AddApplication_RegistraMediatR()
    {
        var services = new ServiceCollection();
        services.AddApplication();

        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IMediator));

        Assert.NotNull(descriptor);
    }

    [Fact]
    public void AddApplication_RegistraValidationBehavior()
    {
        var services = new ServiceCollection();
        services.AddApplication();

        var descriptor = services.FirstOrDefault(d => d.ServiceType == typeof(IPipelineBehavior<,>));

        Assert.NotNull(descriptor);
        Assert.Equal(typeof(ValidationBehavior<,>), descriptor.ImplementationType);
    }
}
