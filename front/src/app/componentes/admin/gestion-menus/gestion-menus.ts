import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { Menu } from '@interfaces/menu.interface';
import { MenuService } from '@servicios/menu/menu.service';
import { MessageService } from 'primeng/api';
import { DialogModule } from 'primeng/dialog';

@Component({
  selector: 'app-gestion-menus',
  imports: [ReactiveFormsModule, DialogModule, CurrencyPipe],
  templateUrl: './gestion-menus.html',
  styleUrl: './gestion-menus.css',
})
export class GestionMenus implements OnInit {
  menus = signal<Menu[]>([]);
  menusListados = computed(() => this.menus());

  menuForm!: FormGroup;
  dialogVisible = signal(false);
  editandoId: number | null = null;

  menuService = inject(MenuService);
  messageService = inject(MessageService);
  fb = inject(FormBuilder);

  ngOnInit(): void {
    this.menuForm = this.fb.group({
      nombre: ['', Validators.required],
      descripcion: [''],
      precio: [0, [Validators.required, Validators.min(1)]],
      imagen: [''],
    });
    this.cargarMenus();
  }

  cargarMenus(): void {
    this.menuService.listarMenus().subscribe({
      next: (data) => this.menus.set(data),
      error: (err) => console.log(err),
    });
  }

  abrirNuevo(): void {
    this.editandoId = null;
    this.menuForm.reset({ nombre: '', descripcion: '', precio: 0, imagen: '' });
    this.dialogVisible.set(true);
  }

  abrirEdicion(menu: Menu): void {
    this.editandoId = menu.id;
    this.menuForm.patchValue({
      nombre: menu.nombre,
      descripcion: menu.descripcion,
      precio: menu.precio,
      imagen: menu.imagen ?? '',
    });
    this.dialogVisible.set(true);
  }

  guardar(): void {
    if (this.menuForm.invalid) return;

    const menu = this.menuForm.value as Menu;

    if (this.editandoId === null) {
      this.menuService.crearMenu(menu).subscribe({
        next: () => {
          this.dialogVisible.set(false);
          this.cargarMenus();
          this.messageService.add({
            severity: 'success',
            summary: 'Menú creado',
            detail: 'El menú se creó correctamente',
          });
        },
        error: (err) =>
          this.messageService.add({
            severity: 'error',
            summary: 'Error al crear',
            detail: `${err}`,
          }),
      });
    } else {
      this.menuService.actualizarMenu(this.editandoId, menu).subscribe({
        next: () => {
          this.dialogVisible.set(false);
          this.cargarMenus();
          this.messageService.add({
            severity: 'success',
            summary: 'Menú actualizado',
            detail: 'El menú se actualizó correctamente',
          });
        },
        error: (err) =>
          this.messageService.add({
            severity: 'error',
            summary: 'Error al actualizar',
            detail: `${err}`,
          }),
      });
    }
  }

  eliminar(id: number): void {
    this.menuService.eliminarMenu(id).subscribe({
      next: (data) => {
        this.cargarMenus();
        this.messageService.add({
          severity: 'success',
          summary: 'Menú eliminado',
          detail: `${data.mensaje}`,
        });
      },
      error: (err) =>
        this.messageService.add({
          severity: 'error',
          summary: 'Error al eliminar',
          detail: `${err}`,
        }),
    });
  }
}
