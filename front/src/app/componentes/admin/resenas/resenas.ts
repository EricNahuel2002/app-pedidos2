import { Component, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Resena } from '@interfaces/resena.interface';
import { OrdenService } from '@servicios/orden/orden.service';
import { RatingModule } from 'primeng/rating';
import { ButtonModule } from 'primeng/button';
import { MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-admin-resenas',
  imports: [RatingModule, ButtonModule, DatePipe, FormsModule],
  templateUrl: './resenas.html',
  styleUrl: './resenas.css',
})
export class AdminResenas implements OnInit {
  ordenService = inject(OrdenService);
  messageService = inject(MessageService);

  resenas = signal<Resena[]>([]);
  cargando = signal(true);

  ngOnInit(): void {
    this.cargarResenas();
  }

  cargarResenas(): void {
    this.cargando.set(true);
    this.ordenService.obtenerResenasAdministracion().subscribe({
      next: (data) => {
        this.resenas.set(data);
        this.cargando.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.cargando.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: err.error || 'No se pudieron cargar las reseñas',
        });
      },
    });
  }

  eliminarResena(id: number): void {
    this.ordenService.eliminarResena(id).subscribe({
      next: () => {
        this.cargarResenas();
        this.messageService.add({
          severity: 'success',
          summary: 'Reseña eliminada',
          detail: 'La reseña fue eliminada correctamente',
        });
      },
      error: (err: HttpErrorResponse) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Error al eliminar',
          detail: err.error?.mensaje || 'No se pudo eliminar la reseña',
        });
      },
    });
  }
}
