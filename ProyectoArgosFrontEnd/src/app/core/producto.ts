import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Categoria, Producto, RespuestaPaginada } from '../models/producto';

@Injectable({ providedIn: 'root' })
export class ProductoService {
  private http = inject(HttpClient);
  private apiUrl = 'https://localhost:7049/api/productos';

  private productosData = signal<Producto[]>([]);
  productos = this.productosData.asReadonly();

  private categoriasData = signal<Categoria[]>([]);
  categorias = this.categoriasData.asReadonly();

  cargando = signal(false);
  error = signal<string | null>(null);

  cargarProductos() {
    this.cargando.set(true);
    this.error.set(null);

    // tamano=50 es el máximo que permite tu backend
    this.http
      .get<RespuestaPaginada<Producto>>(`${this.apiUrl}?tamano=50`)
      .subscribe({
        next: data => {
          this.productosData.set(data.items);
          this.cargando.set(false);
        },
        error: () => {
          this.error.set('No se pudieron cargar los productos.');
          this.cargando.set(false);
        },
      });
  }

  cargarCategorias() {
    this.http.get<Categoria[]>(`${this.apiUrl}/categorias`).subscribe({
      next: data => this.categoriasData.set(data),
    });
  }

  obtenerPorId(id: number) {
    return this.http.get<Producto>(`${this.apiUrl}/${id}`);
  }
}