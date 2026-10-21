export interface Producto {
  idProducto: number;
  idCategoria: number;
  categoria: string;
  nombre: string;
  descripcion: string | null;
  precio: number;
  stock: number;
  imagenUrl: string | null;
  franquicia: string | null;
  isbn: string | null;
  editorial: string | null;
  autor: string | null;
}

export interface Categoria {
  idCategoria: number;
  nombre: string;
}

export interface RespuestaPaginada<T> {
  total: number;
  pagina: number;
  tamano: number;
  items: T[];
}

// Para crear (el backend genera idProducto, categoria viene de idCategoria)
export type ProductoCrear = Omit<Producto, 'idProducto' | 'categoria'>;