import { Component, inject, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { CurrencyPipe, DatePipe } from '@angular/common';
import { SolicitudService } from '../../../core/services/solicitud.service';
import { AuthService } from '../../../core/services/auth.service';
import { etiquetaEstado, FiltrosSolicitud, PagedResult, SolicitudDto, TIPOS_APOYO, ESTADOS_FILTRO } from '../../../core/models/solicitud.models';

@Component({
  selector: 'app-panel',
  imports: [
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatChipsModule,
    CurrencyPipe,
    DatePipe,
  ],
  templateUrl: './panel.html',
  styleUrl: './panel.scss'
})
export class Panel implements OnInit {
  private readonly solicitudService = inject(SolicitudService);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);
  private readonly fb = inject(FormBuilder);

  readonly nombreUsuario = this.authService.getUser()?.nombreCompleto ?? '';
  readonly tiposApoyo = TIPOS_APOYO;
  readonly estadosFiltro = ESTADOS_FILTRO;
  readonly columnas = ['nombreEstudiante', 'tipoApoyo', 'montoSolicitado', 'estado', 'fechaSolicitud', 'acciones'];
  readonly etiquetaEstado = etiquetaEstado;

  resultado = signal<PagedResult<SolicitudDto>>({ items: [], total: 0, page: 1, pageSize: 10, totalPages: 0 });
  loading = signal(false);

  page = 1;
  pageSize = 10;

  filtros = this.fb.group({
    estado: [null as number | null],
    tipo: [null as number | null],
    desde: [''],
    hasta: [''],
  });

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    const f = this.filtros.getRawValue();
    const params: FiltrosSolicitud = {
      page: this.page,
      pageSize: this.pageSize,
      estado: f.estado,
      tipo: f.tipo,
      desde: f.desde || null,
      hasta: f.hasta || null,
    };

    this.loading.set(true);
    this.solicitudService.listar(params).subscribe({
      next: (data) => { this.resultado.set(data); this.loading.set(false); },
      error: () => this.loading.set(false),
    });
  }

  aplicarFiltros(): void {
    this.page = 1;
    this.cargar();
  }

  limpiarFiltros(): void {
    this.filtros.reset();
    this.page = 1;
    this.cargar();
  }

  onPage(event: PageEvent): void {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.cargar();
  }

  verDetalle(id: string): void {
    this.router.navigate(['/asesor/solicitudes', id]);
  }

}
