export interface SolicitudDto {
  id: string;
  estudianteId: string;
  nombreEstudiante: string;
  tipoApoyo: string;
  montoSolicitado: number;
  descripcion: string;
  estado: string;
  fechaSolicitud: string;
  fechaActualizacion: string;
  nombreAsesor?: string;
}

export interface HistorialEstadoDto {
  estadoAnterior: string;
  estadoNuevo: string;
  observacion: string;
  nombreUsuario: string;
  fechaCambio: string;
}

export interface SolicitudDetalleDto extends SolicitudDto {
  emailEstudiante: string;
  programaAcademico: string;
  semestre: number;
  asesorId?: string;
  historial: HistorialEstadoDto[];
}

export interface CrearSolicitudRequest {
  tipoApoyo: number;
  montoSolicitado: number;
  descripcion: string;
}

export const TIPOS_APOYO = [
  { label: 'Beca', value: 0 },
  { label: 'Crédito', value: 1 },
  { label: 'Subsidio', value: 2 },
];

export const ESTADOS_SOLICITUD = [
  { label: 'Pendiente', value: 'Pendiente' },
  { label: 'En Revisión', value: 'EnRevision' },
  { label: 'Aprobada', value: 'Aprobada' },
  { label: 'Rechazada', value: 'Rechazada' },
];

export const ESTADO_COLOR: Record<string, string> = {
  Pendiente: 'warn',
  EnRevision: 'accent',
  Aprobada: 'primary',
  Rechazada: 'warn',
};
