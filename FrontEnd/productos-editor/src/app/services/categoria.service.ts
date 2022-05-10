import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CategoriaService {
  private endpointProductos='https://localhost:44390/';
  private endpointApiCategorias='api/Categorias/';

  constructor(private http:HttpClient) { }

  getCategorias():Observable<any>
  {
    return this.http.get(this.endpointProductos+this.endpointApiCategorias);
  }
}
