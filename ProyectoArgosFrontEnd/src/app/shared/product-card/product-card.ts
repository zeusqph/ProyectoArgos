import { Component, computed, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Producto } from '../../models/producto';

@Component({
  selector: 'app-product-card',
  imports: [RouterLink],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css',
})
export class ProductCard {
  producto = input.required<Producto>();

  imagen = computed(() => {
    const p = this.producto();

    if (!p.imagenUrl) {
      return 'https://placehold.co/300x400?text=' + encodeURIComponent(p.nombre);
    }

    return p.imagenUrl;   // sin anteponer el host: la imagen viene del frontend (carpeta public)
  });
}