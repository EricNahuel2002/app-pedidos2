import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { Orden } from '@interfaces/orden.interface';
import { OrdenService } from '@servicios/orden/orden.service';
import { UsuarioService } from '@servicios/usuario/usuario.service';
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SplitButtonModule } from 'primeng/splitbutton';
import { DialogModule } from 'primeng/dialog';
import { RatingModule } from 'primeng/rating';
import { ButtonModule } from 'primeng/button';
import { MenuItem } from 'primeng/api';
import { MenuItemContent } from 'primeng/menu';
import { MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';


@Component({
  selector: 'app-ordenes',
  imports: [NgClass, SplitButtonModule, DialogModule, RatingModule, FormsModule, ButtonModule],
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

    dialogVisible = signal(false);
    ordenAcalificar = signal<Orden | null>(null);
    puntaje = signal(0);
    comentario = signal('');
  
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

    abrirDialogoResena(orden: Orden): void {
        this.ordenAcalificar.set(orden);
        this.puntaje.set(0);
        this.comentario.set('');
        this.dialogVisible.set(true);
    }

    enviarResena(): void {
        const orden = this.ordenAcalificar();

        if (!orden || this.puntaje() < 1 || this.puntaje() > 5) {
            this.messageService.add({
                severity: 'warn',
                summary: 'Calificación requerida',
                detail: 'Selecciona un puntaje de 1 a 5 estrellas',
            });
            return;
        }

        this.ordenService.crearResena(orden.idOrden, this.puntaje(), this.comentario()).subscribe({
            next: () => {
                this.dialogVisible.set(false);
                this.listarOrdenesDelUsuario();
                this.messageService.add({
                    severity: 'success',
                    summary: 'Reseña enviada',
                    detail: 'Gracias por calificar tu orden',
                });
            },
            error: (err: HttpErrorResponse) => {
                const status = err.status;
                const detail = err.error?.mensaje || err.error || 'Error desconocido del servidor.';

                let summary = 'Error al enviar reseña';
                if (status === 409) {
                    summary = 'Reseña existente';
                } else if (status === 404) {
                    summary = 'Orden no encontrada';
                } else if (status === 400) {
                    summary = 'Solicitud inválida';
                }

                this.messageService.add({
                    severity: 'error',
                    summary: summary,
                    detail: detail,
                });
            },
        });
    }

    
}
