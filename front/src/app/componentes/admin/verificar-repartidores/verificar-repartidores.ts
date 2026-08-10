import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { RepartidorPendiente } from '@interfaces/repartidor-pendiente.interface';
import { AdminService } from '@servicios/admin/admin.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-verificar-repartidores',
  imports: [],
  templateUrl: './verificar-repartidores.html',
  styleUrl: './verificar-repartidores.css',
})
export class VerificarRepartidores implements OnInit {
  repartidores = signal<RepartidorPendiente[]>([]);
  repartidoresListados = computed(() => this.repartidores());

  adminService = inject(AdminService);
  messageService = inject(MessageService);

  ngOnInit(): void {
    this.cargarRepartidores();
  }

  cargarRepartidores(): void {
    this.adminService.listarRepartidoresPendientes().subscribe({
      next: (data) => this.repartidores.set(data),
      error: (err) => console.log(err),
    });
  }

  verificar(id: number): void {
    this.adminService.verificarRepartidor(id).subscribe({
      next: (data) => {
        this.cargarRepartidores();
        this.messageService.add({
          severity: 'success',
          summary: 'Repartidor verificado',
          detail: `${data.mensaje}`,
        });
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error al verificar',
          detail: `${err}`,
        });
      },
    });
  }
}
