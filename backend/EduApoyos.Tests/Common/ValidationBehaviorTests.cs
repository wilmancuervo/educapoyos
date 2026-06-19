using EduApoyos.Application.Common.Behaviors;
using FluentValidation;
using MediatR;

namespace EduApoyos.Tests.Common;

public class ValidationBehaviorTests
{
    private record FakeRequest(string Valor) : IRequest<string>;

    private class FakeValidator : AbstractValidator<FakeRequest>
    {
        public FakeValidator()
        {
            RuleFor(r => r.Valor).NotEmpty().WithMessage("El valor es requerido.");
        }
    }

    [Fact]
    public async Task Handle_SinValidadores_LlamaNextYRetornaResultado()
    {
        var behavior = new ValidationBehavior<FakeRequest, string>([]);
        RequestHandlerDelegate<string> next = () => Task.FromResult("ok");

        var result = await behavior.Handle(new FakeRequest("test"), next, default);

        Assert.Equal("ok", result);
    }

    [Fact]
    public async Task Handle_ValidacionExitosa_LlamaNextYRetornaResultado()
    {
        var behavior = new ValidationBehavior<FakeRequest, string>([new FakeValidator()]);
        RequestHandlerDelegate<string> next = () => Task.FromResult("ok");

        var result = await behavior.Handle(new FakeRequest("valor válido"), next, default);

        Assert.Equal("ok", result);
    }

    [Fact]
    public async Task Handle_ValidacionFallida_LanzaValidationException()
    {
        var behavior = new ValidationBehavior<FakeRequest, string>([new FakeValidator()]);
        RequestHandlerDelegate<string> next = () => Task.FromResult("ok");

        await Assert.ThrowsAsync<ValidationException>(
            () => behavior.Handle(new FakeRequest(string.Empty), next, default));
    }
}
