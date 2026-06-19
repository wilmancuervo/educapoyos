using EduApoyos.Application.DTOs.Solicitudes;
using FluentValidation;

namespace EduApoyos.Application.Validators.Solicitudes;

public class CambiarEstadoValidator : AbstractValidator<CambiarEstadoDto>
{
    private static readonly string[] AccionesValidas = ["aprobar", "rechazar"];

    public CambiarEstadoValidator()
    {
        RuleFor(x => x.SolicitudId)
            .NotEmpty().WithMessage("El ID de la solicitud es obligatorio.");

        RuleFor(x => x.Accion)
            .NotEmpty().WithMessage("La acción es obligatoria.")
            .Must(a => AccionesValidas.Contains(a.ToLower()))
            .WithMessage("La acción debe ser 'aprobar' o 'rechazar'.");

        RuleFor(x => x.Observacion)
            .NotEmpty().WithMessage("La observación es obligatoria.")
            .MaximumLength(500).WithMessage("La observación no puede superar los 500 caracteres.");
    }
}
