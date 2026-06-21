using EduApoyos.Application.Features.Solicitudes.Commands.AsignarAsesor;
using FluentValidation;

namespace EduApoyos.Application.Validators.Solicitudes;

public class AsignarAsesorValidator : AbstractValidator<AsignarAsesorCommand>
{
    public AsignarAsesorValidator()
    {
        RuleFor(x => x.SolicitudId)
            .NotEmpty().WithMessage("El ID de la solicitud es obligatorio.");

        RuleFor(x => x.AsesorId)
            .NotEmpty().WithMessage("El ID del asesor es obligatorio.");

        RuleFor(x => x.Observacion)
            .NotEmpty().WithMessage("La observación es obligatoria.")
            .MaximumLength(500).WithMessage("La observación no puede superar los 500 caracteres.");
    }
}
