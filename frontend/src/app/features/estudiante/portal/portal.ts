import { Component, inject, OnInit, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { CurrencyPipe, DatePipe } from '@angular/common';
import jsPDF from 'jspdf';
import { SolicitudService } from '../../../core/services/solicitud.service';
import { AuthService } from '../../../core/services/auth.service';
import { SolicitudDto } from '../../../core/models/solicitud.models';
import { NuevaSolicitudDialog } from './nueva-solicitud.dialog';

@Component({
  selector: 'app-portal',
  imports: [
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    CurrencyPipe,
    DatePipe,
  ],
  templateUrl: './portal.html',
  styleUrl: './portal.scss'
})
export class Portal implements OnInit {
  private readonly solicitudService = inject(SolicitudService);
  private readonly authService = inject(AuthService);
  private readonly dialog = inject(MatDialog);

  solicitudes = signal<SolicitudDto[]>([]);
  loading = signal(true);

  readonly nombreUsuario = this.authService.getUser()?.nombreCompleto ?? '';

  ngOnInit(): void {
    this.cargarSolicitudes();
  }

  cargarSolicitudes(): void {
    const userId = this.authService.getUser()?.sub;
    if (!userId) return;

    this.loading.set(true);
    this.solicitudService.listarPorEstudiante(userId).subscribe({
      next: (data) => {
        this.solicitudes.set(data);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  abrirNuevaSolicitud(): void {
    const ref = this.dialog.open(NuevaSolicitudDialog, { width: '480px' });
    ref.afterClosed().subscribe(creada => {
      if (creada) this.cargarSolicitudes();
    });
  }

  colorEstado(estado: string): string {
    const colores: Record<string, string> = {
      Pendiente: 'estado-pendiente',
      EnRevision: 'estado-revision',
      Aprobada: 'estado-aprobada',
      Rechazada: 'estado-rechazada',
    };
    return colores[estado] ?? '';
  }

  etiquetaEstado(estado: string): string {
    const etiquetas: Record<string, string> = {
      Pendiente: 'Pendiente',
      EnRevision: 'En Revisión',
      Aprobada: 'Aprobada',
      Rechazada: 'Rechazada',
    };
    return etiquetas[estado] ?? estado;
  }

  descargarConstancia(s: SolicitudDto): void {
    const doc = new jsPDF();
    const margin = 20;
    const pageWidth = doc.internal.pageSize.getWidth();
    let y = 20;

    doc.setFillColor(25, 118, 210);
    doc.rect(0, 0, pageWidth, 35, 'F');
    doc.setTextColor(255, 255, 255);
    doc.setFont('helvetica', 'bold');
    doc.setFontSize(16);
    doc.text('EduApoyos', margin, 15);
    doc.setFontSize(10);
    doc.setFont('helvetica', 'normal');
    doc.text('Sistema de Gestión de Apoyos Económicos', margin, 24);
    y = 50;

    doc.setTextColor(0, 0, 0);
    doc.setFont('helvetica', 'bold');
    doc.setFontSize(14);
    doc.text('CONSTANCIA DE SOLICITUD DE APOYO ECONÓMICO', pageWidth / 2, y, { align: 'center' });
    y += 4;

    doc.setDrawColor(25, 118, 210);
    doc.setLineWidth(0.5);
    doc.line(margin, y, pageWidth - margin, y);
    y += 12;

    const campo = (label: string, valor: string) => {
      doc.setFont('helvetica', 'bold');
      doc.setFontSize(10);
      doc.setTextColor(100, 100, 100);
      doc.text(label, margin, y);
      doc.setFont('helvetica', 'normal');
      doc.setTextColor(0, 0, 0);
      doc.text(valor, margin + 55, y);
      y += 8;
    };

    campo('Estudiante:', s.nombreEstudiante);
    campo('Tipo de apoyo:', s.tipoApoyo);
    campo('Monto solicitado:', `$ ${s.montoSolicitado.toLocaleString('es-CO')} COP`);
    campo('Estado actual:', this.etiquetaEstado(s.estado));
    campo('Fecha de solicitud:', new Date(s.fechaSolicitud).toLocaleDateString('es-CO', { day: '2-digit', month: 'long', year: 'numeric' }));
    if (s.nombreAsesor) campo('Asesor asignado:', s.nombreAsesor);
    y += 2;

    doc.setFont('helvetica', 'bold');
    doc.setFontSize(10);
    doc.setTextColor(100, 100, 100);
    doc.text('Descripción:', margin, y);
    y += 6;
    doc.setFont('helvetica', 'normal');
    doc.setTextColor(0, 0, 0);
    const lines = doc.splitTextToSize(s.descripcion, pageWidth - margin * 2) as string[];
    doc.text(lines, margin, y);
    y += lines.length * 6 + 10;

    doc.setDrawColor(200, 200, 200);
    doc.line(margin, y, pageWidth - margin, y);
    y += 8;
    doc.setFont('helvetica', 'italic');
    doc.setFontSize(8);
    doc.setTextColor(130, 130, 130);
    doc.text(`Documento generado el ${new Date().toLocaleString('es-CO')}`, pageWidth / 2, y, { align: 'center' });
    doc.text(`ID de solicitud: ${s.id}`, pageWidth / 2, y + 5, { align: 'center' });

    doc.save(`constancia-${s.id.slice(0, 8)}.pdf`);
  }
}
