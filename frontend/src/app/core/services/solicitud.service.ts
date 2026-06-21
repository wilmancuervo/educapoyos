import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CrearSolicitudRequest, SolicitudDetalleDto, SolicitudDto } from '../models/solicitud.models';

@Injectable({ providedIn: 'root' })
export class SolicitudService {
  private readonly http = inject(HttpClient);
  private readonly base = `${environment.apiUrl}/solicitudes`;

  listarPorEstudiante(usuarioId: string) {
    return this.http.get<SolicitudDto[]>(`${environment.apiUrl}/estudiantes/${usuarioId}/solicitudes`);
  }

  obtenerDetalle(id: string) {
    return this.http.get<SolicitudDetalleDto>(`${this.base}/${id}`);
  }

  crear(dto: CrearSolicitudRequest) {
    return this.http.post<SolicitudDto>(this.base, dto);
  }
}
