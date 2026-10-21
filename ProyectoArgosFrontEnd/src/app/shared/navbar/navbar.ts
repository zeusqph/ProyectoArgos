import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink],
  template: `
    <nav>
      <a routerLink="/">Argos</a>
      <a routerLink="/catalogo">Catálogo</a>
      <a routerLink="/carrito">Carrito</a>
    </nav>
  `,
})  
export class Navbar {}