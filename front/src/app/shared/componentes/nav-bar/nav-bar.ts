import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from "@angular/router";
import { NgIf } from '@angular/common';
import { UsuarioService } from '@servicios/usuario/usuario.service';

@Component({
  selector: 'app-nav-bar',
  imports: [RouterLink, RouterLinkActive, NgIf],
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.css',
})
export class NavBar {
  usuarioService = inject(UsuarioService);
  router = inject(Router);

  get isLoggedIn(): boolean {
    return this.usuarioService.authStatus().isAuthenticated;
  }

  get role(): string | null {
    return this.usuarioService.currentUser() ? this.usuarioService.currentUser()!.rol : null;
  }

  get logoLink(): string {
    return this.role === 'repartidor' ? '/repartidor' : '/';
  }

  logout(): void {
    this.usuarioService.logout().subscribe({
      next: () => this.router.navigate(['/iniciar-sesion']),
      error: () => this.router.navigate(['/iniciar-sesion'])
    });
  }
}
