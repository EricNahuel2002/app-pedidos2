import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Cliente } from '@interfaces/cliente.interface';
import { Menu } from '@interfaces/menu.interface';
import { MenuService } from '@servicios/menu/menu.service';
import { OrdenService } from '@servicios/orden/orden.service';
import { UsuarioService } from '@servicios/usuario/usuario.service';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { MessageService } from 'primeng/api';
import { SplitButtonModule } from 'primeng/splitbutton';

@Component({
  selector: 'app-formulario-orden',
  imports: [ReactiveFormsModule,ProgressSpinnerModule,SplitButtonModule],
  templateUrl: './formulario-orden.html',
  styleUrl: './formulario-orden.css',
})
export class FormularioOrden implements OnInit {

  route = inject(ActivatedRoute);
  usuarioService = inject(UsuarioService);
  menuService = inject(MenuService);
  ordenService = inject(OrdenService);
  formBuilder = inject(FormBuilder);
  router = inject(Router);
  messageService = inject(MessageService);

  idMenu!:number;
  idUsuario!:number;
  menu:Menu | null = null;
  formOrden !: FormGroup;
  contenidoCargado = signal<boolean>(false);

  ngOnInit(): void {
    this.idMenu = Number(this.route.snapshot.paramMap.get('id'));
    this.idUsuario = this.usuarioService.obtenerUsuarioDeSesion() ?? 0;

    this.formOrden = this.formBuilder.group(
      {
        idMenu : [this.idMenu,Validators.required]
      }
    )

    this.listarFormulario(this.idMenu);
  }


  listarFormulario(idMenu:number){

    this.menuService.listarMenu(idMenu).subscribe({
      next : (data)  => {
        this.menu = data;
        this.formOrden.patchValue({ idMenu: this.menu?.id ?? this.idMenu });
        this.contenidoCargado.set(true);
      },
      error : (error) => {
        console.log(error)
      }
    });
  }


  confirmarOrden():void{

    if(!this.formOrden) return;

    if(this.formOrden.valid){
      const {idMenu} = this.formOrden.value;
      if(!this.idUsuario || this.idUsuario == 0){
        this.router.navigate(['/iniciar-sesion', { queryParams: { returnUrl: this.router.url }}])
        return;
      }else{
        this.ordenService.ConfirmarOrden(idMenu,this.idUsuario).subscribe({
        next: (data) => {
        this.messageService.add({
          severity: 'success',
          summary: 'Orden realizada',
          detail: 'Tu orden esta pendiente de ser tomada por un repartidor'
        })
          this.router.navigate(['/'])
        },
        error : (err) => console.log(err)
      })
      }

      
    }
  }

  volverADetalle():void{
    this.router.navigate(['/detalle-menu/',this.menu?.id]);
  }

}
