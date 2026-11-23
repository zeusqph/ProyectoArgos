import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, RouterLinkActive],
  template: `
    <nav class="navbar">
      <a routerLink="/" class="marca">Argos</a>
      <div class="enlaces">
        <a routerLink="/catalogo" routerLinkActive="activo">Catálogo</a>
        <a routerLink="/carrito" routerLinkActive="activo">Carrito</a>
      </div>
    </nav>
  `,
  styleUrl: './navbar.css',
})
export class Navbar {}