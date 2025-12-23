// auth-guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { UsuarioService } from './usuario/usuario.service';
import { map} from 'rxjs/operators';

export const authGuard: CanActivateFn = (route, state) => {
  const usuarioService = inject(UsuarioService);
  const router = inject(Router);

  const rolesPermitidos = route.data?.['roles'] as string[] | undefined;

  return usuarioService.checkAuthStatus().pipe(
      map((credenciales) => {
        if(!credenciales){
          return router.createUrlTree(['/iniciar-sesion'],{
            queryParams : {returnUrl : state.url}
          })
        }

        if(rolesPermitidos && !rolesPermitidos.includes(credenciales.rol)){
            console.log("No tenes permisos para acceder a la vista")
            return router.createUrlTree(['/'],{
              queryParams: {returnUrl: state.url}
            })
        }
        return true;
      })
    ) 
  
  
};
