import { Routes } from '@angular/router';
import { MainLayout } from '../../layouts/main-layout/main-layout';

export const ESTUDIANTE_ROUTES: Routes = [
  {
    path: '',
    component: MainLayout,
    children: []
  }
];
