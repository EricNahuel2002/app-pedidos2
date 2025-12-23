import { CurrencyPipe } from '@angular/common';
import { Component ,OnInit, signal, computed, effect } from '@angular/core';
import { Orden } from '@interfaces/orden.interface';
import { OrdenService } from '@servicios/orden/orden.service';
@Component({
  selector: 'app-repartidor',
  imports: [CurrencyPipe],
  templateUrl: './repartidor.html',
  styleUrl: './repartidor.css',
})
export class Repartidor implements OnInit {
// Inicializamos el arreglo como Signal
  public ordenes = signal<Orden[]>([]);
  private reloadTrigger = signal(0);
  private reloadComputed = computed(() => this.reloadTrigger());

  constructor(private ordenService: OrdenService) { 
    // Efecto que escucha el computed y recarga las órdenes cuando cambia
    effect(() => {
      this.reloadComputed();
      this.cargarOrdenes();
    });
  }

  ngOnInit(): void {
    // El effect en el constructor ya dispara la primera carga
  }

  cargarOrdenes(): void {
    this.ordenService.obtenerOrdenesPendientes().subscribe(
      (ordenes) => {
        this.ordenes.set(ordenes);
      },
      (err) => {
        console.error('Error cargando órdenes pendientes', err);
      }
    );
  }

  tomarOrden(id: number): void {
    console.log('Tomando la orden con ID:', id);
    alert(`Has tomado la orden #${id}. El estado debería cambiar a 'En Proceso'.`);

    // Trigger para recargar órdenes usando el computed -> effect
    this.reloadTrigger.update(n => n + 1);
  }
}
