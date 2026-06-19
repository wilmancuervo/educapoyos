using EduApoyos.Application.DTOs.Estudiantes;
using FluentValidation;

namespace EduApoyos.Application.Validators.Estudiantes;

public class CrearEstudianteValidator : AbstractValidator<CrearEstudianteDto>
{
    public CrearEstudianteValidator()
    {
        RuleFor(x => x.UsuarioId)
            .NotEmpty().WithMessage("El ID de usuario es obligatorio.");

        RuleFor(x => x.NumeroDocumento)
            .NotEmpty().WithMessage("El número de documento es obligatorio.")
            .MaximumLength(20).WithMessage("El número de documento no puede superar los 20 caracteres.");

        RuleFor(x => x.TipoDocumento)
            .IsInEnum().WithMessage("El tipo de documento indicado no es válido.");

        RuleFor(x => x.ProgramaAcademico)
            .NotEmpty().WithMessage("El programa académico es obligatorio.")
            .MaximumLength(150).WithMessage("El programa académico no puede superar los 150 caracteres.");

        RuleFor(x => x.Semestre)
            .InclusiveBetween(1, 12).WithMessage("El semestre debe estar entre 1 y 12.");
    }
}
