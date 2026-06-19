using EduApoyos.Domain.Common;

namespace EduApoyos.Application.Common.Errors;

public static class ApplicationErrors
{
    public static class Auth
    {
        public static readonly Error CredencialesInvalidas = new("Auth.CredencialesInvalidas", "Email o contraseña incorrectos.");
        public static readonly Error EmailYaRegistrado = new("Auth.EmailYaRegistrado", "Ya existe un usuario registrado con ese email.");
        public static readonly Error AccesoDenegado = new("Auth.AccesoDenegado", "No tiene permisos para realizar esta acción.");
    }

    public static class Estudiante
    {
        public static readonly Error NoEncontrado = new("Estudiante.NoEncontrado", "El estudiante no fue encontrado.");
        public static readonly Error YaExiste = new("Estudiante.YaExiste", "Ya existe un perfil de estudiante para este usuario.");
        public static readonly Error DocumentoDuplicado = new("Estudiante.DocumentoDuplicado", "Ya existe un estudiante con ese número de documento.");
    }

    public static class Solicitud
    {
        public static readonly Error NoEncontrada = new("Solicitud.NoEncontrada", "La solicitud de apoyo no fue encontrada.");
        public static readonly Error AccionInvalida = new("Solicitud.AccionInvalida", "La acción indicada no es válida. Use 'aprobar' o 'rechazar'.");
    }

    public static class Usuario
    {
        public static readonly Error NoEncontrado = new("Usuario.NoEncontrado", "El usuario no fue encontrado.");
        public static readonly Error NoEsAsesor = new("Usuario.NoEsAsesor", "El usuario indicado no tiene rol de Asesor.");
    }
}
