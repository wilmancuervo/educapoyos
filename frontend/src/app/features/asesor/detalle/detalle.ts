import { Component, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { SolicitudService } from '../../../core/services/solicitud.service';
import { AuthService } from '../../../core/services/auth.service';
import { NotificationService } from '../../../core/services/notification.service';
import { etiquetaEstado, SolicitudDetalleDto } from '../../../core/models/solicitud.models';

@Component({
  selector: 'app-detalle',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatFormFieldModule,
    MatInputModule,
    MatDividerModule,
    CurrencyPipe,
    DatePipe,
  ],
  templateUrl: './detalle.html',
  styleUrl: './detalle.scss'
})
export class Detalle implements OnInit {
  private readonly solicitudService = inject(SolicitudService);
  private readonly authService = inject(AuthService);
  private readonly notification = inject(NotificationService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  readonly etiquetaEstado = etiquetaEstado;

  solicitud = signal<SolicitudDetalleDto | null>(null);
  loading = signal(true);
  procesando = signal(false);
  tomando = signal(false);

  observacionForm = this.fb.group({
    observacion: ['', Validators.maxLength(500)],
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id')!;
    this.solicitudService.obtenerDetalle(id).subscribe({
      next: (data) => { this.solicitud.set(data); this.loading.set(false); },
      error: () => this.loading.set(false),
    });
  }

  tomarSolicitud(): void {
    const id = this.solicitud()?.id;
    const asesorId = this.authService.getUser()?.sub;
    if (!id || !asesorId) return;

    this.tomando.set(true);
    this.solicitudService.asignarAsesor(id, asesorId, 'Solicitud tomada para revisión.').pipe(
      finalize(() => this.tomando.set(false))
    ).subscribe({
      next: () => {
        this.notification.success('Solicitud tomada. Ahora está En Revisión.');
        this.solicitudService.obtenerDetalle(id).subscribe(data => this.solicitud.set(data));
      }
    });
  }

  cambiarEstado(accion: 'aprobar' | 'rechazar'): void {
    const id = this.solicitud()?.id;
    if (!id) return;

    const observacion = this.observacionForm.get('observacion')?.value ?? '';
    this.procesando.set(true);

    this.solicitudService.cambiarEstado(id, accion, observacion || undefined).pipe(
      finalize(() => this.procesando.set(false))
    ).subscribe({
      next: () => {
        const msg = accion === 'aprobar' ? 'Solicitud aprobada.' : 'Solicitud rechazada.';
        this.notification.success(msg);
        this.router.navigate(['/asesor/panel']);
      }
    });
  }

  volver(): void {
    this.router.navigate(['/asesor/panel']);
  }

  get puedeTomar(): boolean {
    return this.solicitud()?.estado === 'Pendiente';
  }

  get puedeGestionar(): boolean {
    return this.solicitud()?.estado === 'EnRevision';
  }
}
