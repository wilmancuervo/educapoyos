import { Component, inject } from '@angular/core';
import { finalize } from 'rxjs';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { EstudianteService } from '../../../core/services/estudiante.service';
import { EstudianteDto, TIPOS_DOCUMENTO } from '../../../core/models/estudiante.models';

@Component({
  selector: 'app-activar-estudiante-dialog',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './activar-estudiante.dialog.html',
  styleUrl: './activar-estudiante.dialog.scss',
})
export class ActivarEstudianteDialog {
  private readonly fb = inject(FormBuilder);
  private readonly estudianteService = inject(EstudianteService);
  private readonly dialogRef = inject(MatDialogRef<ActivarEstudianteDialog>);
  readonly usuario = inject<EstudianteDto>(MAT_DIALOG_DATA);

  readonly tiposDocumento = TIPOS_DOCUMENTO;
  loading = false;

  form = this.fb.group({
    numeroDocumento: ['', [Validators.required, Validators.maxLength(20)]],
    tipoDocumento: [null as number | null, Validators.required],
    programaAcademico: ['', [Validators.required, Validators.maxLength(200)]],
    semestre: [null as number | null, [Validators.required, Validators.min(1), Validators.max(12)]],
  });

  activar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    const raw = this.form.getRawValue();

    this.estudianteService.crear({
      usuarioId: this.usuario.usuarioId,
      numeroDocumento: raw.numeroDocumento!,
      tipoDocumento: raw.tipoDocumento!,
      programaAcademico: raw.programaAcademico!,
      semestre: raw.semestre!,
    }).pipe(
      finalize(() => this.loading = false)
    ).subscribe({
      next: () => this.dialogRef.close(true),
    });
  }
}
