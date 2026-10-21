import { Component, input } from '@angular/core';
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
}