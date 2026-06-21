import { Routes } from '@angular/router';
import { AsesorLayout } from '../../layouts/asesor-layout/asesor-layout';

export const ASESOR_ROUTES: Routes = [
  {
    path: '',
    component: AsesorLayout,
    children: [
      { path: '', redirectTo: 'panel', pathMatch: 'full' },
      {
        path: 'panel',
        loadComponent: () => import('./panel/panel').then(m => m.Panel)
      },
      {
        path: 'estudiantes',
        loadComponent: () => import('./estudiantes/estudiantes').then(m => m.Estudiantes)
      },
      {
        path: 'solicitudes/:id',
        loadComponent: () => import('./detalle/detalle').then(m => m.Detalle)
      }
    ]
  }
];
