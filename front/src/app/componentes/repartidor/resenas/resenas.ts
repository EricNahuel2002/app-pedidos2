import { Component, inject, OnInit, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ResenasRepartidor } from '@interfaces/resena.interface';
import { OrdenService } from '@servicios/orden/orden.service';
import { RatingModule } from 'primeng/rating';
import { MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-repartidor-resenas',
  imports: [RatingModule, DatePipe, FormsModule],
  templateUrl: './resenas.html',
  styleUrl: './resenas.css',
})
export class RepartidorResenas implements OnInit {
  ordenService = inject(OrdenService);
  messageService = inject(MessageService);

  resenas = signal<ResenasRepartidor | null>(null);
  cargando = signal(true);

  ngOnInit(): void {
    this.cargarResenas();
  }

  cargarResenas(): void {
    this.cargando.set(true);
    this.ordenService.obtenerResenasMias().subscribe({
      next: (data) => {
        this.resenas.set(data);
        this.cargando.set(false);
      },
      error: (err: HttpErrorResponse) => {
        this.cargando.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Error',
          detail: err.error || 'No se pudieron cargar tus reseñas',
        });
      },
    });
  }
}
