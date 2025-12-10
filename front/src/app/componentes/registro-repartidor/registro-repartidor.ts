import { Component, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UsuarioService } from '@servicios/usuario/usuario.service';

@Component({
  selector: 'app-registro-repartidor',
  imports: [ReactiveFormsModule],
  templateUrl: './registro-repartidor.html',
  styleUrl: './registro-repartidor.css',
})
export class RegistroRepartidor {
    usuarioService = inject(UsuarioService);
    fb = inject(FormBuilder);
    router = inject(Router);
    
    registroForm!:FormGroup;
    preview_dni = signal<string | null>(null);
    preview_perfil = signal<string | null>(null);

    ngOnInit(): void {
      this.registroForm = this.fb.group({
        email: ['', [Validators.required, Validators.email]],
        nombre: ['',[Validators.required]],
        dni: ['',[Validators.required]],
        password: ['', Validators.required],
        direccion: ['',Validators.required],
        telefono: ''
      })
    }


    onSubmit():void{
      if(this.registroForm.valid){
        const {email,nombre,dni,contrasenia,direccion,telefono} = this.registroForm.value;

        this.usuarioService.registrarUsuario(nombre,email,dni,contrasenia,direccion).subscribe({
          next: (data) => this.router.navigate(['/iniciar-sesion']),
          error: (err) => console.log("ERROR AL REGISTRARSE")
        })
      }
    }
}
