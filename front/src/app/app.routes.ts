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
        canActivate : [authGuard],
        data : { roles:['cliente']}
    },
    {
        path : 'ordenes',
        loadComponent : () => import("@componentes/ordenes/ordenes")
        .then(c => c.Ordenes),
        canActivate : [authGuard],
        data : { roles:['cliente']}
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
        path : 'registro-repartidor',
        loadComponent : () => import("@componentes/registro-repartidor/registro-repartidor")
        .then(c => c.RegistroRepartidor)
    },
    {
        path : 'repartidor',
        loadComponent : () => import("@componentes/repartidor/repartidor")
        .then(c => c.Repartidor),
        canActivate : [authGuard],
        data : { roles:['repartidor']}
    },
    {
        path : 'ordenes-tomadas',
        loadComponent : () => import("@componentes/repartidor-ordenes-tomadas/repartidor-ordenes-tomadas")
        .then(c => c.RepartidorOrdenesTomadas),
        canActivate : [authGuard],
        data: {roles:['repartidor']}
    },
    {
        path : 'admin',
        loadComponent : () => import("@componentes/admin/panel-admin/panel-admin")
        .then(c => c.PanelAdmin),
        canActivate : [authGuard],
        data : { roles:['administrador']}
    },
    {
        path : 'admin/repartidores',
        loadComponent : () => import("@componentes/admin/verificar-repartidores/verificar-repartidores")
        .then(c => c.VerificarRepartidores),
        canActivate : [authGuard],
        data : { roles:['administrador']}
    },
    {
        path : 'admin/menus',
        loadComponent : () => import("@componentes/admin/gestion-menus/gestion-menus")
        .then(c => c.GestionMenus),
        canActivate : [authGuard],
        data : { roles:['administrador']}
    }
];
