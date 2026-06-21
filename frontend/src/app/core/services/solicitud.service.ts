import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CrearSolicitudRequest, FiltrosSolicitud, PagedResult, SolicitudDetalleDto, SolicitudDto } from '../models/solicitud.models';

@Injectable({ providedIn: 'root' })
export class SolicitudService {
  private readonly http = inject(HttpClient);
  private readonly base = `${environment.apiUrl}/solicitudes`;

  listarPorEstudiante(usuarioId: string) {
    return this.http.get<SolicitudDto[]>(`${environment.apiUrl}/estudiantes/${usuarioId}/solicitudes`);
  }

  listar(filtros: FiltrosSolicitud) {
    let params = new HttpParams()
      .set('page', filtros.page)
      .set('pageSize', filtros.pageSize);

    if (filtros.estado != null) params = params.set('estado', filtros.estado);
    if (filtros.tipo != null) params = params.set('tipo', filtros.tipo);
    if (filtros.desde) params = params.set('desde', filtros.desde);
    if (filtros.hasta) params = params.set('hasta', filtros.hasta);

    return this.http.get<PagedResult<SolicitudDto>>(this.base, { params });
  }

  obtenerDetalle(id: string) {
    return this.http.get<SolicitudDetalleDto>(`${this.base}/${id}`);
  }

  crear(dto: CrearSolicitudRequest) {
    return this.http.post<SolicitudDto>(this.base, dto);
  }

  cambiarEstado(id: string, accion: 'aprobar' | 'rechazar', observacion?: string) {
    return this.http.patch<void>(`${this.base}/${id}/estado`, { accion, observacion });
  }
}
