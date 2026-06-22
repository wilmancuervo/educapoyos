-- ============================================================
-- EduApoyos - Scripts SQL requeridos
-- ============================================================


-- ------------------------------------------------------------
-- 1. Solicitudes pendientes con mas de 5 dias sin actualizacion
--    ordenadas por antiguedad (la mas antigua primero)
-- ------------------------------------------------------------
SELECT
    s.Id,
    u.NombreCompleto        AS Estudiante,
    s.TipoApoyo,
    s.MontoSolicitado,
    s.Descripcion,
    s.FechaSolicitud,
    s.FechaActualizacion,
    DATEDIFF(DAY, s.FechaActualizacion, GETUTCDATE()) AS DiasInactivo
FROM SolicitudesApoyo s
INNER JOIN Estudiantes e ON e.Id = s.EstudianteId
INNER JOIN Usuarios    u ON u.Id = e.UsuarioId
WHERE s.Estado = 'Pendiente'
  AND DATEDIFF(DAY, s.FechaActualizacion, GETUTCDATE()) > 5
ORDER BY s.FechaActualizacion ASC;


-- ------------------------------------------------------------
-- 2. Total de solicitudes agrupadas por estado y tipo de apoyo
--    en el ultimo mes
-- ------------------------------------------------------------
SELECT
    s.Estado,
    s.TipoApoyo,
    COUNT(*) AS Total
FROM SolicitudesApoyo s
WHERE s.FechaSolicitud >= DATEADD(MONTH, -1, GETUTCDATE())
GROUP BY s.Estado, s.TipoApoyo
ORDER BY s.Estado, s.TipoApoyo;


-- ------------------------------------------------------------
-- 3. Indice no agrupado sobre la tabla SolicitudesApoyo
--
--    Justificacion:
--    El filtro mas frecuente en el panel del asesor combina
--    Estado + FechaSolicitud (listar solicitudes pendientes
--    ordenadas por fecha). Un indice compuesto sobre estas dos
--    columnas permite que SQL Server resuelva la consulta sin
--    escanear toda la tabla (Index Seek en lugar de Table Scan),
--    lo que reduce el tiempo de respuesta especialmente cuando
--    el volumen de solicitudes crece.
-- ------------------------------------------------------------
CREATE NONCLUSTERED INDEX IX_SolicitudesApoyo_Estado_Fecha
ON SolicitudesApoyo (Estado, FechaSolicitud DESC)
INCLUDE (EstudianteId, TipoApoyo, MontoSolicitado, AsesorId);
