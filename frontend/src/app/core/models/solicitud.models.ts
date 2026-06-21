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
  observacion: string | null;
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


export interface PagedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface FiltrosSolicitud {
  page: number;
  pageSize: number;
  estado?: number | null;
  tipo?: number | null;
  desde?: string | null;
  hasta?: string | null;
}

export const ESTADOS_FILTRO = [
  { label: 'Pendiente', value: 0 },
  { label: 'En Revisión', value: 1 },
  { label: 'Aprobada', value: 2 },
  { label: 'Rechazada', value: 3 },
];

const ETIQUETAS_ESTADO: Record<string, string> = {
  Pendiente: 'Pendiente',
  EnRevision: 'En Revisión',
  Aprobada: 'Aprobada',
  Rechazada: 'Rechazada',
};

export function etiquetaEstado(estado: string): string {
  return ETIQUETAS_ESTADO[estado] ?? estado;
}
