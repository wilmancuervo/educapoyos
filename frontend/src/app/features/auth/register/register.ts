import { Component, inject, OnInit } from '@angular/core';
import { finalize } from 'rxjs';
import { AbstractControl, FormBuilder, ValidationErrors, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AuthService } from '../../../core/services/auth.service';
import { RegisterDto } from '../../../core/models/auth.models';

function confirmPasswordValidator(control: AbstractControl): ValidationErrors | null {
  const password = control.parent?.get('password')?.value;
  return control.value && control.value !== password ? { passwordMismatch: true } : null;
}

@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss'
})
export class Register implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  form = this.fb.group({
    nombreCompleto: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/^(?=.*[A-Z])(?=.*[0-9])/)]],
    confirmPassword: ['', [Validators.required, confirmPasswordValidator]],
    rol: [null as number | null, Validators.required],
  });

  loading = false;
  showPassword = false;
  showConfirm = false;

  readonly roles = [
    { label: 'Estudiante', value: 1 },
    { label: 'Asesor', value: 0 },
  ];

  ngOnInit(): void {
    this.form.get('password')!.valueChanges.subscribe(() => {
      this.form.get('confirmPassword')!.updateValueAndValidity({ emitEvent: false });
    });
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    const raw = this.form.getRawValue();

    const dto: RegisterDto = {
      nombreCompleto: raw.nombreCompleto!,
      email: raw.email!,
      password: raw.password!,
      rol: raw.rol!,
    };

    this.authService.register(dto).pipe(
      finalize(() => this.loading = false)
    ).subscribe({
      next: () => {
        const rol = this.authService.getRole();
        this.router.navigate([rol === 'Asesor' ? '/asesor' : '/estudiante']);
      }
    });
  }
}
