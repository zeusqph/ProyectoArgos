import { Routes } from '@angular/router';
import { Home } from './pages/home/home';
import { Catalogo } from './pages/catalogo/catalogo';
import { Detalle } from './pages/detalle/detalle';
import { Carrito } from './pages/carrito/carrito';
import { Navbar } from './shared/navbar/navbar';

export const routes: Routes = [
  { path: '', component: Home },
  { path: 'catalogo', component: Catalogo },
  { path: 'producto/:id', component: Detalle },
  { path: 'carrito', component: Carrito },
  { path: '**', redirectTo: '' },
  { path: 'app-navbar', component: Navbar },
];