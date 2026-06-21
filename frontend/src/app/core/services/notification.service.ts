import { inject, Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

const DURATION_ERROR = 7_000;
const DURATION_SUCCESS = 4_000;

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly snackBar = inject(MatSnackBar);

  error(message: string): void {
    this.snackBar.open(message, 'Cerrar', {
      duration: DURATION_ERROR,
      panelClass: ['snack-error'],
      horizontalPosition: 'center',
      verticalPosition: 'top',
    });
  }

  success(message: string): void {
    this.snackBar.open(message, 'Cerrar', {
      duration: DURATION_SUCCESS,
      panelClass: ['snack-success'],
      horizontalPosition: 'center',
      verticalPosition: 'top',
    });
  }
}
