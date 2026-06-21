import { Component, inject, OnInit, signal } from '@angular/core';
import { PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog } from '@angular/material/dialog';
import { EstudianteService } from '../../../core/services/estudiante.service';
import { EstudianteDto } from '../../../core/models/estudiante.models';
import { PagedResult } from '../../../core/models/solicitud.models';
import { NuevoEstudianteDialog } from './nuevo-estudiante.dialog';

@Component({
  selector: 'app-estudiantes',
  imports: [
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './estudiantes.html',
  styleUrl: './estudiantes.scss',
})
export class Estudiantes implements OnInit {
  private readonly estudianteService = inject(EstudianteService);
  private readonly dialog = inject(MatDialog);

  readonly columnas = ['nombreCompleto', 'email', 'tipoDocumento', 'numeroDocumento', 'programaAcademico', 'semestre'];

  resultado = signal<PagedResult<EstudianteDto>>({ items: [], total: 0, page: 1, pageSize: 10, totalPages: 0 });
  loading = signal(false);

  page = 1;
  pageSize = 10;

  ngOnInit(): void {
    this.cargar();
  }

  cargar(): void {
    this.loading.set(true);
    this.estudianteService.listar(this.page, this.pageSize).subscribe({
      next: (data) => { this.resultado.set(data); this.loading.set(false); },
      error: () => this.loading.set(false),
    });
  }

  onPage(event: PageEvent): void {
    this.page = event.pageIndex + 1;
    this.pageSize = event.pageSize;
    this.cargar();
  }

  abrirNuevo(): void {
    const ref = this.dialog.open(NuevoEstudianteDialog, { width: '520px' });
    ref.afterClosed().subscribe(creado => {
      if (creado) this.cargar();
    });
  }
}
