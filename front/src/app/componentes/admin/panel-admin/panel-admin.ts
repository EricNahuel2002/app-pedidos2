import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { EstadisticasOrdenes } from '@interfaces/estadisticas-ordenes.interface';
import { AdminService } from '@servicios/admin/admin.service';
import { MenuService } from '@servicios/menu/menu.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-panel-admin',
  imports: [RouterLink],
  templateUrl: './panel-admin.html',
  styleUrl: './panel-admin.css',
})
export class PanelAdmin implements OnInit {
  adminService = inject(AdminService);
  menuService = inject(MenuService);

  totalUsuarios = signal(0);
  repartidoresPendientes = signal(0);
  totalMenus = signal(0);
  estadisticas = signal<EstadisticasOrdenes>({
    total: 0,
    pendientes: 0,
    enCurso: 0,
    finalizadas: 0,
    canceladas: 0,
  });

  ngOnInit(): void {
    forkJoin({
      usuarios: this.adminService.listarUsuarios(),
      repartidoresPendientes: this.adminService.listarRepartidoresPendientes(),
      menus: this.menuService.listarMenus(),
      estadisticas: this.adminService.obtenerEstadisticasOrdenes(),
    }).subscribe({
      next: ({ usuarios, repartidoresPendientes, menus, estadisticas }) => {
        this.totalUsuarios.set(usuarios.length);
        this.repartidoresPendientes.set(repartidoresPendientes.length);
        this.totalMenus.set(menus.length);
        this.estadisticas.set(estadisticas);
      },
      error: (err) => console.log(err),
    });
  }
}
