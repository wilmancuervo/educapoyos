import { Routes } from '@angular/router';
import { AuthLayout } from '../../layouts/auth-layout/auth-layout';

export const AUTH_ROUTES: Routes = [
  {
    path: '',
    component: AuthLayout,
    children: [
      { path: '', redirectTo: 'login', pathMatch: 'full' },
      {
        path: 'login',
        loadComponent: () => import('./login/login').then(m => m.Login)
      },
      {
        path: 'registro',
        loadComponent: () => import('./register/register').then(m => m.Register)
      }
    ]
  }
];
