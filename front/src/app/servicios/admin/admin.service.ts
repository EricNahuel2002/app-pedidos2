import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '@environment/environment.development';
import { EstadisticasOrdenes } from '@interfaces/estadisticas-ordenes.interface';
import { RepartidorPendiente } from '@interfaces/repartidor-pendiente.interface';
import { UsuarioAdministracion } from '@interfaces/usuario-administracion.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  private http = inject(HttpClient);

  listarUsuarios(): Observable<UsuarioAdministracion[]> {
    return this.http.get<UsuarioAdministracion[]>(
      `${environment.BACKEND_URL}/admin/usuarios`,
      { withCredentials: true }
    );
  }

  listarRepartidoresPendientes(): Observable<RepartidorPendiente[]> {
    return this.http.get<RepartidorPendiente[]>(
      `${environment.BACKEND_URL}/admin/repartidores/pendientes`,
      { withCredentials: true }
    );
  }

  verificarRepartidor(id: number): Observable<{ mensaje: string }> {
    return this.http.patch<{ mensaje: string }>(
      `${environment.BACKEND_URL}/admin/repartidores/${id}/verificar`,
      {},
      { withCredentials: true }
    );
  }

  obtenerEstadisticasOrdenes(): Observable<EstadisticasOrdenes> {
    return this.http.get<EstadisticasOrdenes>(
      `${environment.BACKEND_URL}/admin/ordenes/estadisticas`,
      { withCredentials: true }
    );
  }
}
