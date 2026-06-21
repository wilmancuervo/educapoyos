import { Component, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { SolicitudService } from '../../../core/services/solicitud.service';
import { TIPOS_APOYO } from '../../../core/models/solicitud.models';

@Component({
  selector: 'app-nueva-solicitud-dialog',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './nueva-solicitud.dialog.html',
  styleUrl: './nueva-solicitud.dialog.scss'
})
export class NuevaSolicitudDialog {
  private readonly fb = inject(FormBuilder);
  private readonly solicitudService = inject(SolicitudService);
  private readonly dialogRef = inject(MatDialogRef<NuevaSolicitudDialog>);

  readonly tipos = TIPOS_APOYO;
  loading = false;

  form = this.fb.group({
    tipoApoyo: [null as number | null, Validators.required],
    montoSolicitado: [null as number | null, [Validators.required, Validators.min(1)]],
    descripcion: ['', [Validators.required, Validators.minLength(10)]],
  });

  enviar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.solicitudService.crear(this.form.getRawValue() as any).pipe(
      finalize(() => this.loading = false)
    ).subscribe({
      next: () => this.dialogRef.close(true)
    });
  }
}
