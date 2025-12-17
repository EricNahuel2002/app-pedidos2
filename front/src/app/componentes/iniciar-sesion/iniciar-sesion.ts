import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UsuarioService } from '@servicios/usuario/usuario.service';

@Component({
  selector: 'app-iniciar-sesion',
  imports: [ReactiveFormsModule],
  templateUrl: './iniciar-sesion.html',
  styleUrl: './iniciar-sesion.css',
})
export class IniciarSesion implements OnInit{
  

  router = inject(Router);
  usuarioService = inject(UsuarioService);
  fb = inject(FormBuilder);


  loginForm!: FormGroup;
  
  ngOnInit(): void {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  onSubmit(): void {
  if (this.loginForm.valid) {
    const { email, password } = this.loginForm.value;

    this.usuarioService.iniciarSesion(email, password).subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (err) => {
        console.error('Error al iniciar sesión:', err);
      }
    });
  }
}

  registrarse(){
    this.router.navigate(['/registro-usuario']);
  }


}
