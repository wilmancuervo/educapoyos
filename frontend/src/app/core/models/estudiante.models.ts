export interface EstudianteDto {
  usuarioId: string;
  nombreCompleto: string;
  email: string;
  estudianteId?: string;
  numeroDocumento?: string;
  tipoDocumento?: string;
  programaAcademico?: string;
  semestre?: number;
}

export interface PagedResult<T> {
  items: T[];
  total: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface CrearEstudianteRequest {
  usuarioId: string;
  numeroDocumento: string;
  tipoDocumento: number;
  programaAcademico: string;
  semestre: number;
}

export interface NuevoEstudianteForm {
  nombreCompleto: string;
  email: string;
  password: string;
  numeroDocumento: string;
  tipoDocumento: number;
  programaAcademico: string;
  semestre: number;
}

export const TIPOS_DOCUMENTO = [
  { label: 'Cédula de Ciudadanía', value: 0 },
  { label: 'Tarjeta de Identidad', value: 1 },
  { label: 'Pasaporte', value: 2 },
  { label: 'Cédula de Extranjería', value: 3 },
];
