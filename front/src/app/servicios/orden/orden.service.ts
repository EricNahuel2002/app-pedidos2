import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '@environment/environment.development';
import { Orden } from '@interfaces/orden.interface';
import { Resena, ResenasRepartidor } from '@interfaces/resena.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class OrdenService {
  
  private http = inject(HttpClient);
  
  
  ConfirmarOrden(idMenu: number) {
    return this.http.get(`${environment.BACKEND_URL}/ordenes/confirmarOrden/${idMenu}`,{ withCredentials: true, responseType: 'text' as 'json' });
  }
  
  obtenerOrdenesDelCliente():Observable<Orden[]> {
    
    return this.http.get<Orden[]>(`${environment.BACKEND_URL}/ordenes/cliente`,{withCredentials: true}); 
  }

  obtenerOrdenesPendientes(): Observable<Orden[]> {
    return this.http.get<Orden[]>(
      `${environment.BACKEND_URL}/ordenes/ordenesPendientes`,
      { withCredentials: true }
    );
  }

  cancelarOrden(idOrden:number){
    return this.http.patch(
        `${environment.BACKEND_URL}/ordenes/cancelar`, 
        {idOrden},
        { 
            responseType: 'text',
            withCredentials : true
        }
    );
  }

  tomarOrdenDelCliente(id:number){
      return this.http.get(`${environment.BACKEND_URL}/ordenes/tomarOrden/${id}`,{withCredentials:true,responseType: 'text' as 'json' });
  }

  obtenerOrdenesTomadasPorRepartidor(): Observable<Orden[]> {
    return this.http.get<Orden[]>(
      `${environment.BACKEND_URL}/ordenes/repartidor`,
      { withCredentials: true }
    );
  }
  

  finalizarOrden(id:number){
    return this.http.patch(`${environment.BACKEND_URL}/ordenes/marcarOrdenFinalizada`,id,{withCredentials : true, responseType: 'text' as 'json'})
  }

  crearResena(idOrden: number, puntaje: number, comentario: string) {
    return this.http.post(
      `${environment.BACKEND_URL}/resenas`,
      { idOrden, puntaje, comentario },
      { withCredentials: true }
    );
  }

  obtenerResenasMias(): Observable<ResenasRepartidor> {
    return this.http.get<ResenasRepartidor>(
      `${environment.BACKEND_URL}/resenas/mias`,
      { withCredentials: true }
    );
  }

  obtenerResenasAdministracion(): Observable<Resena[]> {
    return this.http.get<Resena[]>(
      `${environment.BACKEND_URL}/admin/resenas`,
      { withCredentials: true }
    );
  }

  eliminarResena(id: number) {
    return this.http.delete(
      `${environment.BACKEND_URL}/admin/resenas/${id}`,
      { withCredentials: true }
    );
  }
}
