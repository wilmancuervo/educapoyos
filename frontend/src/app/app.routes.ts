import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { roleGuard } from './core/guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () =>
      import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: 'asesor',
    canActivate: [authGuard, roleGuard('Asesor')],
    loadChildren: () =>
      import('./features/asesor/asesor.routes').then(m => m.ASESOR_ROUTES)
  },
  {
    path: 'estudiante',
    canActivate: [authGuard, roleGuard('Estudiante')],
    loadChildren: () =>
      import('./features/estudiante/estudiante.routes').then(m => m.ESTUDIANTE_ROUTES)
  },
  { path: '**', redirectTo: 'login' }
];
