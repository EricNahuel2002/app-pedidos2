import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '@environment/environment.development';
import { Orden } from '@interfaces/orden.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class OrdenService {
  
  private http = inject(HttpClient);
  
  
  ConfirmarOrden(idMenu: number, idUsuario: number) {
    return this.http.post(`${environment.BACKEND_URL}/ordenes/confirmarOrden`,{idMenu,idUsuario},{ responseType: 'text' as 'json' });
  }
  
  obtenerOrdenesDelCliente(idCliente: number | null):Observable<Orden[]> {
    
    return this.http.get<Orden[]>(`${environment.BACKEND_URL}/ordenes/cliente/${idCliente}`); 
  }

  cancelarOrden(idCliente:number,idOrden:number){
    return this.http.patch(
        `${environment.BACKEND_URL}/ordenes/cancelar/cliente/${idCliente}/orden/${idOrden}`, 
        null,
        { 
            responseType: 'text'
        }
    );
  }
}
