// auth-guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UsuarioService } from './usuario/usuario.service';
import { map} from 'rxjs/operators';

export const authGuard: CanActivateFn = (route, state) => {
  const usuarioService = inject(UsuarioService);
  const router = inject(Router);

  return usuarioService.checkAuthStatus().pipe(
    map(isAuth => 
      isAuth ? true:
      router.createUrlTree(['/iniciar-sesion'],{queryParams : {returnUrl : state.url}}))
    );
  
};
