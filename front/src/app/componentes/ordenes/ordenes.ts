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
import { Router } from '@angular/router';


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
    router = inject(Router);


    ordenes = signal<Orden[]>([]);
    ordenesListadas = computed(() => {
      return this.ordenes();
    })
    items:MenuItem[] = [MenuItemContent]
  
    ngOnInit(): void {
      this.listarOrdenesDelUsuario();
    }


    listarOrdenesDelUsuario() {

      this.ordenService.obtenerOrdenesDelCliente().subscribe({
        next: (data) => this.ordenes.set(data),
        error : (err) => {
          console.log(err);
        }
      })
    }

    cancelarOrden(idOrden:number){

        this.ordenService.cancelarOrden(idOrden).subscribe({
        next: (data:string) => {
          this.listarOrdenesDelUsuario();
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
      
    }

    
}
