import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { NotificationService } from '../services/notification.service';
import { ProblemDetails } from '../models/problem-details.model';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notification = inject(NotificationService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
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
