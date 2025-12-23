import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '@environment/environment.development';
import { AuthStatus } from '@interfaces/auth-status.interface';
import { CredencialUsuario } from '@interfaces/credencial-usuario.interface';
import { catchError, map, Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {

  private http = inject(HttpClient);

  private readonly _authStatus = signal<AuthStatus>({ isAuthenticated: false });
  public readonly authStatus = computed(() => this._authStatus());


  checkAuthStatus(): Observable<CredencialUsuario | null> {

    return this.http.get<CredencialUsuario>(`${environment.BACKEND_URL}/auth/haySesionValida`,{withCredentials: true}).pipe(
        tap((credenciales) => {
          this._authStatus.set({isAuthenticated : true});
        }),
        catchError(() =>{
          this._authStatus.set({isAuthenticated : false});
          console.log("No hay sesion valida");
          return of(null);
      })
    )
}


    iniciarSesion(email: string, contrasenia: string) {
        return this.http.post(`${environment.BACKEND_URL}/auth/login`,
          {email,contrasenia},{withCredentials: true});
    }

    registrarCliente(nombre:string,email:string,contrasenia:string,direccion:string,telefono:string){
      return this.http.post(`${environment.BACKEND_URL}/usuarios/registrarCliente`,{nombre,email,contrasenia,direccion,telefono},{withCredentials: true});
    }

    registrarRepartidor(nombre:string,email:string,contrasenia:string,direccion:string,telefono:string){
      return this.http.post(`${environment.BACKEND_URL}/usuarios/registrarCliente`,{nombre,email,contrasenia,direccion,telefono},{withCredentials: true});
    }
  
}
