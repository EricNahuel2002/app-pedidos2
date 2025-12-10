import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { environment } from '@environment/environment.development';
import { AuthStatus } from '@interfaces/auth-status.interface';
import { catchError, map, Observable, of, tap } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {

  private http = inject(HttpClient);

  private readonly _authStatus = signal<AuthStatus>({ isAuthenticated: false });
  public readonly authStatus = computed(() => this._authStatus());


  checkAuthStatus(idUsuario:string): Observable<boolean> {

    console.log("ID del usuario en checkstatus",idUsuario)

    console.log("ESTOY EN EL CHECKAUTHSTATUS")
  const current = this._authStatus();
  if (current?.isAuthenticated) {
    console.log("CHECKAUTHSTATUS ESTOY AUTENTICADO")
    return of(true);
  }

  if (!idUsuario) {
    console.log("CHECKAUTHSTATUS NO ESTOY AUTENTICADO")
    this._authStatus.set({ isAuthenticated: false });
    return of(false);
  }

  return this.http.get<boolean>(`${environment.BACKEND_URL}/usuarios/validarSesion/${idUsuario}`)
    .pipe(
      map(response => {
        return !!response;
      }),
      tap(isValid => {
        console.log("CHECKAUTHSTATUS ESTOY AUTENTICADO???: ",isValid)
        this._authStatus.set({ isAuthenticated: isValid, idUsuario: isValid ? idUsuario : undefined });
        console.log("ID DEL USUARIO EN CHECK:",idUsuario)
      }),
      catchError(() => {
        this._authStatus.set({ isAuthenticated: false });
        return of(false);
      })
    );
}


   iniciarSesion(email: string, contrasenia: string) {
      return this.http.post<number>(`${environment.BACKEND_URL}/usuarios/iniciar-sesion`,{email,contrasenia});
   }

    registrarUsuario(nombre:string,email:string,contrasenia:string,direccion:string,telefono:string){
      return this.http.post(`${environment.BACKEND_URL}/usuarios/crear-cliente`,{nombre,email,contrasenia,direccion,telefono});
  }

   obtenerUsuarioDeSesion(): number | null {
    if (typeof window === 'undefined') {
      return null;
    }
    const id = Number(sessionStorage.getItem('idUsuario'));
    return id;
  }

  guardarUsuarioEnSesion(id:number){
    try {
      sessionStorage.setItem('idUsuario', id.toString());
    } catch(e) {
      console.error(e);
    }
  
  }


  
}
