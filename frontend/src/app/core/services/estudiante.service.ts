import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { switchMap } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthResponse } from '../models/auth.models';
import { CrearEstudianteRequest, EstudianteDto, NuevoEstudianteForm } from '../models/estudiante.models';
import { PagedResult } from '../models/solicitud.models';

@Injectable({ providedIn: 'root' })
export class EstudianteService {
  private readonly http = inject(HttpClient);
  private readonly base = `${environment.apiUrl}/estudiantes`;

  listar(page: number, pageSize: number) {
    return this.http.get<PagedResult<EstudianteDto>>(this.base, {
      params: { page, pageSize },
    });
  }

  crear(dto: CrearEstudianteRequest) {
    return this.http.post<EstudianteDto>(this.base, dto);
  }

  registrarYCrear(form: NuevoEstudianteForm) {
    return this.http.post<AuthResponse>(`${environment.apiUrl}/auth/register`, {
      nombreCompleto: form.nombreCompleto,
      email: form.email,
      password: form.password,
      rol: 1,
    }).pipe(
      switchMap(res => {
        const payload = JSON.parse(
          atob(res.token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/'))
        );
        const usuarioId: string = payload['sub'];
        return this.crear({
          usuarioId,
          numeroDocumento: form.numeroDocumento,
          tipoDocumento: form.tipoDocumento,
          programaAcademico: form.programaAcademico,
          semestre: form.semestre,
        });
      })
    );
  }
}
