export interface User {
  userId: string;
  email: string;
  fullName: string;
}

export interface AuthResponse {
  userId: string;
  email: string;
  fullName: string;
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
}

export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  email: string;
  password: string;
  confirmPassword: string;
  fullName: string;
}
