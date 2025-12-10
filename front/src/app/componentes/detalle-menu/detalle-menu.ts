import { Component, inject, OnDestroy, OnInit, signal } from '@angular/core';
import { Menu } from '@interfaces/menu.interface';
import { CurrencyPipe } from '@angular/common';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { MenuService } from '@servicios/menu/menu.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Subject, takeUntil } from 'rxjs';
import { OrdenService } from '@servicios/orden/orden.service';

@Component({
  selector: 'app-detalle-menu',
  imports: [CurrencyPipe,ProgressSpinnerModule],
  templateUrl: './detalle-menu.html',
  styleUrl: './detalle-menu.css',
})
export class DetalleMenu implements OnInit, OnDestroy{
  
  menu = signal<Menu | undefined>(undefined);
  idMenu!: number;
  contenidoCargado = signal<boolean>(false);
  private destroy$ = new Subject<void>();

  menuService = inject(MenuService);
  route = inject(ActivatedRoute);
  ordenService = inject(OrdenService);
  router = inject(Router);

  ngOnInit(): void {
    this.idMenu = Number(this.route.snapshot.paramMap.get('id'));
    this.listarMenu(this.idMenu);
  }

  listarMenu(idMenu:number){
    this.menuService.listarMenu(idMenu).pipe(
      takeUntil(this.destroy$)
    ).subscribe({
      next: (data) => {
        this.menu.set(data);
        this.contenidoCargado.set(true);
      },
      error : (data) => console.log("ERROR AL TRAER UN MENU",data)
    })
  }

  hacerOrden(idMenu:number){
    this.router.navigate(['/formulario-orden',idMenu]);
  }
  



  ngOnDestroy(): void {
      this.destroy$.next();
      this.destroy$.complete();
    }
  
}
