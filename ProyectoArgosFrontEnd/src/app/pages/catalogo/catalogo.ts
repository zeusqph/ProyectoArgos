import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { ProductoService } from '../../core/producto';
import { ProductCard } from '../../shared/product-card/product-card';

@Component({
  selector: 'app-catalogo',
  imports: [ProductCard],
  templateUrl: './catalogo.html',
  styleUrl: './catalogo.css',
})
export class Catalogo implements OnInit {
  productoService = inject(ProductoService);

  categoriaActiva = signal<number | null>(null); // null = todos

  productosFiltrados = computed(() => {
    const cat = this.categoriaActiva();
    const todos = this.productoService.productos();
    return cat === null ? todos : todos.filter(p => p.idCategoria === cat);
  });

  ngOnInit() {
    this.productoService.cargarProductos();
    this.productoService.cargarCategorias();
  }
}