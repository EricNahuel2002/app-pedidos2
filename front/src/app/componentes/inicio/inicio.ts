import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Menu } from '@interfaces/menu.interface';
import { MenuService } from '@servicios/menu/menu.service';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { Subject, takeUntil } from 'rxjs';
import { Router } from "@angular/router";





@Component({
  selector: 'app-inicio',
  imports: [CommonModule, ProgressSpinnerModule],
  templateUrl: './inicio.html',
  styleUrls: ['./inicio.css'],
})
export class Inicio implements OnInit,OnDestroy{

  private destroy$ = new Subject<void>();

  menus = signal<Menu[]>([]);
  contenidoCargado = signal<boolean>(false);

  menuService = inject(MenuService);
  router = inject(Router);

  ngOnInit(): void {
    this.listarMenus();
  }

  listarMenus(){
    this.menuService.listarMenus().pipe(
      takeUntil(this.destroy$)
    ).subscribe({
      next: (data) => {
        this.menus.set(data);
        this.contenidoCargado.set(true);
        
      },
      error: (data) => {
        console.log("ERROR AL TRAER LOS MENUS",data)
      }
    })
  }

  verDetalle(id:number){
    this.router.navigate(['/detalle-menu/',id]);
  }

  seleccionar(id: number){

  }


  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
