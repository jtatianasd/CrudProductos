import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductoService {

  private endpointProductos='https://localhost:44390/';
  private endpointApiProductos='api/Productos/';
  constructor(private http:HttpClient) { }

  GetProductos():Observable<any>
  {
    return this.http.get(this.endpointProductos+this.endpointApiProductos);
  }

  deleteProductos(id:number): Observable<any>{
    return this.http.delete(this.endpointProductos+this.endpointApiProductos+id);
  }

  saveProducto(nombre:string,descripcion:string,categoriaId:number,rutaImagen:string):Observable<any>{
    let prod={
      nombre: nombre,
      rutaImagen: rutaImagen,
      descripcion:descripcion,
      categoriaId: Number(categoriaId)
    }
    return this.http.post(this.endpointProductos+this.endpointApiProductos,prod);
  }

  updateProducto(id:number, nombre:string,descripcion:string,categoriaId:number,rutaImagen:string):Observable<any>{
    let prod={
      id:id,
      nombre: nombre,
      rutaImagen: rutaImagen,
      descripcion:descripcion,
      categoriaId: Number(categoriaId)
    }
    return this.http.patch(this.endpointProductos+this.endpointApiProductos+id,prod)
  }

  searchProductById(id:number):Observable<any>{
    console.log(id)
    return this.http.get(this.endpointProductos+this.endpointApiProductos+id);
  }
  searchProductByName(nombre:string):Observable<any>{
    return this.http.get(this.endpointProductos+this.endpointApiProductos+'Buscar?nombre='+nombre);
  }
}
