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
  
  
  ConfirmarOrden(idMenu: number) {
    return this.http.post(`${environment.BACKEND_URL}/ordenes/confirmarOrden`,{idMenu},{ withCredentials: true, responseType: 'text' as 'json' });
  }
  
  obtenerOrdenesDelCliente():Observable<Orden[]> {
    
    return this.http.get<Orden[]>(`${environment.BACKEND_URL}/ordenes/cliente`,{withCredentials: true}); 
  }

  cancelarOrden(idOrden:number){
    return this.http.patch(
        `${environment.BACKEND_URL}/ordenes/cancelar/${idOrden}`, 
        null,
        { 
            responseType: 'text',
            withCredentials : true
        }
    );
  }
}
