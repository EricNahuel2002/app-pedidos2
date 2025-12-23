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
  constructor(){
    this.checkAuthStatus().subscribe();
  }

  private readonly _authStatus = signal<AuthStatus>({ isAuthenticated: false });
  public readonly authStatus = computed(() => this._authStatus());
  private readonly _currentUser = signal<CredencialUsuario | null>(null);
  public readonly currentUser = computed(() => this._currentUser());


  checkAuthStatus(): Observable<CredencialUsuario | null> {

    return this.http.get<CredencialUsuario>(`${environment.BACKEND_URL}/auth/haySesionValida`,{withCredentials: true}).pipe(
        tap((credenciales) => {
          this._authStatus.set({isAuthenticated : true});
          this._currentUser.set(credenciales);
        }),
        catchError(() =>{
          this._authStatus.set({isAuthenticated : false});
          this._currentUser.set(null);
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
    
    logout(){
      return this.http.post(`${environment.BACKEND_URL}/auth/logout`,{}, {withCredentials: true}).pipe(
        tap(() =>{
          this._authStatus.set({isAuthenticated: false});
          this._currentUser.set(null);
        })
      )
    }
  
}
