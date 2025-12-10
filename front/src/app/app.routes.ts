import { Routes } from '@angular/router';
import { Inicio } from './componentes/inicio/inicio';
import { authGuard } from '@servicios/auth-guard';

export const routes: Routes = [
    {
        path : '',
        component : Inicio
    },
    {
        path : 'detalle-menu/:id',
        loadComponent : () => import("@componentes/detalle-menu/detalle-menu")
        .then(c => c.DetalleMenu)
    },
    {
        path : 'formulario-orden/:id',
        loadComponent : () => import("@componentes/formulario-orden/formulario-orden")
        .then(c => c.FormularioOrden),
        canActivate : [authGuard]
    },
    {
        path : 'ordenes',
        loadComponent : () => import("@componentes/ordenes/ordenes")
        .then(c => c.Ordenes),
        canActivate : [authGuard]
    },
    {
        path : 'iniciar-sesion',
        loadComponent : () => import("@componentes/iniciar-sesion/iniciar-sesion")
        .then(c => c.IniciarSesion)
    },
    {
        path : 'registro-usuario',
        loadComponent : () => import("@componentes/registrar-usuario/registrar-usuario")
        .then(c => c.RegistrarUsuario)
    },
    {
        path : 'repartidor',
        loadComponent : () => import("@componentes/repartidor/repartidor")
        .then(c => c.Repartidor),
        canActivate : [authGuard]
    }
];
