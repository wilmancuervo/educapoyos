import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { NotificationService } from '../services/notification.service';
import { AuthService } from '../services/auth.service';
import { ProblemDetails } from '../models/problem-details.model';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notification = inject(NotificationService);
  const auth = inject(AuthService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401 && auth.getToken()) {
        notification.error('Tu sesión ha expirado. Por favor, inicia sesión nuevamente.');
        auth.logout();
        return throwError(() => error);
      }

      let body: (ProblemDetails & { errors?: Record<string, string[]> }) | null = null;

      try {
        body = typeof error.error === 'string' ? JSON.parse(error.error) : error.error;
      } catch {
        body = null;
      }

      let message: string | undefined = body?.detail;

      if (!message && body?.errors) {
        message = Object.values(body.errors).flat()[0];
      }

      notification.error(message ?? 'Ocurrió un error inesperado.');

      return throwError(() => error);
    })
  );
};
