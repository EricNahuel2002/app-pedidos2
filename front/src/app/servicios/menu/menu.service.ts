import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {Menu} from '@interfaces/menu.interface'
import { environment } from '@environment/environment.development';

@Injectable({
  providedIn: 'root',
})
export class MenuService {
    private httpclient = inject(HttpClient);

    listarMenus(): Observable<Menu[]>{
        return this.httpclient.get<Menu[]>(`${environment.BACKEND_URL}/menus`);
    }

    listarMenu(id:number): Observable<Menu>{
      return this.httpclient.get<Menu>(`${environment.BACKEND_URL}/menus/${id}`)
    }

    crearMenu(menu: Menu): Observable<Menu> {
      return this.httpclient.post<Menu>(`${environment.BACKEND_URL}/menus/crear`, menu, { withCredentials: true });
    }

    actualizarMenu(id: number, menu: Menu): Observable<Menu> {
      return this.httpclient.put<Menu>(`${environment.BACKEND_URL}/admin/menus/${id}`, menu, { withCredentials: true });
    }

    eliminarMenu(id: number): Observable<{ mensaje: string }> {
      return this.httpclient.delete<{ mensaje: string }>(`${environment.BACKEND_URL}/admin/menus/${id}`, { withCredentials: true });
    }
}
