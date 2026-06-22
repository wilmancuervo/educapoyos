import { Component, inject, OnInit, signal } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { EstudianteService } from '../../../core/services/estudiante.service';
import { EstudianteDto, PagedResult } from '../../../core/models/estudiante.models';
import { NuevoEstudianteDialog } from './nuevo-estudiante.dialog';
import { ActivarEstudianteDialog } from './activar-estudiante.dialog';

const PAGED_INICIAL: PagedResult<EstudianteDto> = {
  items: [], total: 0, page: 1, pageSize: 10, totalPages: 0,
};

@Component({
  selector: 'app-estudiantes',
  imports: [
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatProgressSpinnerModule,
    MatChipsModule,
  ],
  templateUrl: './estudiantes.html',
  styleUrl: './estudiantes.scss',
})
export class Estudiantes implements OnInit {
  private readonly estudianteService = inject(EstudianteService);
  private readonly dialog = inject(MatDialog);

  readonly columnas = ['nombreCompleto', 'email', 'tipoDocumento', 'numeroDocumento', 'programaAcademico', 'semestre', 'acciones'];

  data = signal<PagedResult<EstudianteDto>>(PAGED_INICIAL);
  loading = signal(false);

  page = 1;
  pageSize = 10;

  tienePerfil(e: EstudianteDto): boolean {
    return !!e.estudianteId;
  }

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    this.loading.set(true);
    this.estudianteService.listar(this.page, this.pageSize).subscribe({
      next: (res) => { this.data.set(res); this.loading.set(false); },
      error: () => this.loading.set(false),
    });
  }

  onPage(event: PageEvent): void {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.cargar();
  }

  abrirNuevo(): void {
    this.dialog.open(NuevoEstudianteDialog, { width: '520px' })
      .afterClosed().subscribe(ok => { if (ok) this.cargar(); });
  }

  completarPerfil(estudiante: EstudianteDto): void {
    this.dialog.open(ActivarEstudianteDialog, { width: '480px', data: estudiante })
      .afterClosed().subscribe(ok => { if (ok) this.cargar(); });
  }
}
