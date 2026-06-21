export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  nombreCompleto: string;
  email: string;
  password: string;
  rol: number;
}

export interface AuthResponse {
  token: string;
}
