import { Component, OnInit, inject, signal } from '@angular/core';
import { ProductoService } from '../../core/producto';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.html',
})
export class Admin implements OnInit {
  productoService = inject(ProductoService);
  mensaje = signal('');

  ngOnInit() {
    this.productoService.cargarProductos();
  }

  onArchivo(event: Event, id: number) {
    const archivo = (event.target as HTMLInputElement).files?.[0];
    if (!archivo) return;

    this.productoService.subirImagen(id, archivo).subscribe({
      next: () => {
        this.mensaje.set('Imagen subida correctamente');
        this.productoService.cargarProductos();   // refresca la lista
      },
      error: () => this.mensaje.set('No se pudo subir la imagen'),
    });
  }
}