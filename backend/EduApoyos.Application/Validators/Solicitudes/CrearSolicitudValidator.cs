using EduApoyos.Application.DTOs.Solicitudes;
using FluentValidation;

namespace EduApoyos.Application.Validators.Solicitudes;

public class CrearSolicitudValidator : AbstractValidator<CrearSolicitudDto>
{
    public CrearSolicitudValidator()
    {
        RuleFor(x => x.TipoApoyo)
            .IsInEnum().WithMessage("El tipo de apoyo indicado no es válido.");

        RuleFor(x => x.MontoSolicitado)
            .GreaterThan(0).WithMessage("El monto solicitado debe ser mayor a cero.")
            .LessThanOrEqualTo(100_000_000).WithMessage("El monto solicitado supera el límite permitido.");

        RuleFor(x => x.Descripcion)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(500).WithMessage("La descripción no puede superar los 500 caracteres.");
    }
}
