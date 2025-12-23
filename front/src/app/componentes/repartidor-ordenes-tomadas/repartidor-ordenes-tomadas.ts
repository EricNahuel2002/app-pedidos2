import { CurrencyPipe, NgClass } from '@angular/common';
import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { Orden } from '@interfaces/orden.interface';
import { OrdenService } from '@servicios/orden/orden.service';
import { MessageService } from 'primeng/api';

@Component({
  selector: 'app-repartidor-ordenes-tomadas',
  imports: [CurrencyPipe,NgClass],
  templateUrl: './repartidor-ordenes-tomadas.html',
  styleUrl: './repartidor-ordenes-tomadas.css',
})
export class RepartidorOrdenesTomadas implements OnInit{
  ordenes = signal<Orden[]>([]);

  ordenesListadas = computed(()=> {
    return this.ordenes();
  })

  ordenService = inject(OrdenService);
  messageService = inject(MessageService);

  ngOnInit(): void {
      this.cargarOrdenes();
  }

  cargarOrdenes(): void {
    this.ordenService.obtenerOrdenesTomadasPorRepartidor().subscribe(
      {
        next: (data) => this.ordenes.set(data),
        error : (err) => console.log(err)
      }
    );
  }


  finalizarOrden(id: number): void {

    this.ordenService.finalizarOrden(id).subscribe({
      next: (data) => {
          this.cargarOrdenes();
          this.messageService.add({
          severity: 'success',
          summary: 'Orden finalizada',
          detail: `${data}`
        });

      },
      error : (err) => {
        this.messageService.add({
          severity: 'danger',
          summary: 'Hubo un error al finalizar la orden',
          detail: `${err}`
        });
      }
    });
  }

  verOrden(id:number){
    
  }
}
