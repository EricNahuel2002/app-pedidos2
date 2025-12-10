import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { Orden } from '@interfaces/orden.interface';
import { OrdenService } from '@servicios/orden/orden.service';
import { UsuarioService } from '@servicios/usuario/usuario.service';
import { NgClass } from '@angular/common';
import { SplitButtonModule } from 'primeng/splitbutton';
import { MenuItem } from 'primeng/api';
import { MenuItemContent } from 'primeng/menu';
import { MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';


@Component({
  selector: 'app-ordenes',
  imports: [NgClass,SplitButtonModule],
  templateUrl: './ordenes.html',
  styleUrl: './ordenes.css',
})
export class Ordenes implements OnInit{
    ordenService = inject(OrdenService);
    usuarioService = inject(UsuarioService);
    messageService = inject(MessageService)


    ordenes = signal<Orden[]>([]);
    ordenesListadas = computed(() => {
      return this.ordenes();
    })
    items:MenuItem[] = [MenuItemContent]
  
    ngOnInit(): void {
      const idCliente = this.usuarioService.obtenerUsuarioDeSesion();

      this.listarOrdenesDelUsuario(idCliente);
    }


    listarOrdenesDelUsuario(idCliente: number | null) {

      this.ordenService.obtenerOrdenesDelCliente(idCliente).subscribe({
        next: (data) => this.ordenes.set(data),
        error : (err) => console.log("ERROR AL OBTENER ORDENES DEL CLIENTE",err)
      })
    }

    cancelarOrden(idOrden:number){

      const idCliente = this.usuarioService.obtenerUsuarioDeSesion();

      if(idCliente){
        this.ordenService.cancelarOrden(idCliente,idOrden).subscribe({
        next: (data:string) => {
          this.listarOrdenesDelUsuario(idCliente);
            this.messageService.add({
        severity: 'success',
        summary: 'Orden cancelada',
        detail: `${data}`
        })
        },
        error: (err: HttpErrorResponse) => { 
                const status = err.status;

                const errorMessage = err.error || 'Error desconocido del servidor.';

                let summary = 'Error al cancelar';
                if (status === 409) {
                    summary = 'Conflicto de Estado';
                } else if (status === 404) {
                    summary = 'Orden no encontrada';
                }

                this.messageService.add({
                    severity: 'error',
                    summary: summary,
                    detail: errorMessage,
                });
            }
      })
      }else{
        console.log("No se encontro el id del cliente")
      }


      
    }

    
}
