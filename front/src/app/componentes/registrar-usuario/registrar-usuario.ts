import { Component, inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UsuarioService } from '@servicios/usuario/usuario.service';

@Component({
  selector: 'app-registrar-usuario',
  imports: [ReactiveFormsModule],
  templateUrl: './registrar-usuario.html',
  styleUrl: './registrar-usuario.css',
})
export class RegistrarUsuario implements OnInit{
    

    usuarioService = inject(UsuarioService);
    fb = inject(FormBuilder);
    router = inject(Router);

    registroForm!:FormGroup;

    ngOnInit(): void {
      this.registroForm = this.fb.group({
        nombre: ['',[Validators.required]],
        email: ['', [Validators.required, Validators.email]],
        password: ['', Validators.required],
        direccion: ['',Validators.required],
        telefono: ''
      })
    }


    onSubmit():void{
      if(this.registroForm.valid){
        const {nombre,email,password,direccion,telefono} = this.registroForm.value;

        this.usuarioService.registrarCliente(nombre,email,password,direccion,telefono).subscribe({
          next: (data) => this.router.navigate(['/iniciar-sesion']),
          error: (err) => console.log("ERROR AL REGISTRARSE")
        })
      }
    }
}
