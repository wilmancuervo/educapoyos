using EduApoyos.Domain.Entities;
using EduApoyos.Domain.Enums;

namespace EduApoyos.Domain.Interfaces;

public interface ISolicitudApoyoRepository : IRepository<SolicitudApoyo>
{
    Task<SolicitudApoyo?> GetByIdWithHistorialAsync(Guid id);
    Task<IEnumerable<SolicitudApoyo>> GetByEstudianteIdAsync(Guid estudianteId);
    Task<(IEnumerable<SolicitudApoyo> Items, int Total)> GetPagedAsync(
        int page,
        int pageSize,
        EstadoSolicitud? estado = null,
        TipoApoyo? tipo = null,
        DateTime? desde = null,
        DateTime? hasta = null);
}
