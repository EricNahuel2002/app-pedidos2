// auth-guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UsuarioService } from './usuario/usuario.service';
import { of } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';

export const authGuard: CanActivateFn = (route, state) => {
  const usuarioService = inject(UsuarioService);
  const router = inject(Router);

  const idUsuario = sessionStorage.getItem('idUsuario');
  console.log("ESTOY EN EL AUTH GUARD: Y EL ID DE USUARIO ES",idUsuario)

  if (!idUsuario) {
    return router.createUrlTree(['/iniciar-sesion'], { queryParams: { returnUrl: state.url } });
  }

  return usuarioService.checkAuthStatus(idUsuario).pipe(
    map(isAuthenticated => {
      console.log("ESTOY AUTENTICADO???",isAuthenticated)
      return isAuthenticated ? true : router.createUrlTree(['/iniciar-sesion'], { queryParams: { returnUrl: state.url }});
    }),
    catchError(err => {
      console.error('AuthGuard: error validando sesión', err);
      return of(router.createUrlTree(['/iniciar-sesion'], { queryParams: { returnUrl: state.url }}));
    })
  );
};
