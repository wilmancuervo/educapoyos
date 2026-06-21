import { Component, inject, OnInit } from '@angular/core';
import { finalize } from 'rxjs';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { EstudianteService } from '../../../core/services/estudiante.service';
import { TIPOS_DOCUMENTO } from '../../../core/models/estudiante.models';

function passwordFortaleza(control: AbstractControl): ValidationErrors | null {
  const v = control.value as string;
  if (!v) return null;
  const ok = /[A-Z]/.test(v) && /[0-9]/.test(v);
  return ok ? null : { passwordDebil: true };
}

function confirmarPasswordValidator(control: AbstractControl): ValidationErrors | null {
  const password = control.parent?.get('password')?.value;
  return control.value && control.value !== password ? { passwordMismatch: true } : null;
}

@Component({
  selector: 'app-nuevo-estudiante-dialog',
  imports: [
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './nuevo-estudiante.dialog.html',
  styleUrl: './nuevo-estudiante.dialog.scss',
})
export class NuevoEstudianteDialog implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly estudianteService = inject(EstudianteService);
  private readonly dialogRef = inject(MatDialogRef<NuevoEstudianteDialog>);

  readonly tiposDocumento = TIPOS_DOCUMENTO;
  loading = false;

  form = this.fb.group({
    nombreCompleto: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(200)]],
    password: ['', [Validators.required, Validators.minLength(8), passwordFortaleza]],
    confirmarPassword: ['', [Validators.required, confirmarPasswordValidator]],
    numeroDocumento: ['', [Validators.required, Validators.maxLength(20)]],
    tipoDocumento: [null as number | null, Validators.required],
    programaAcademico: ['', [Validators.required, Validators.maxLength(200)]],
    semestre: [null as number | null, [Validators.required, Validators.min(1), Validators.max(12)]],
  });

  ngOnInit(): void {
    this.form.get('password')!.valueChanges.subscribe(() => {
      this.form.get('confirmarPassword')!.updateValueAndValidity({ emitEvent: false });
    });
  }

  enviar(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    const raw = this.form.getRawValue();

    this.estudianteService.registrarYCrear({
      nombreCompleto: raw.nombreCompleto!,
      email: raw.email!,
      password: raw.password!,
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
