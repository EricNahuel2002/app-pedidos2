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
}
