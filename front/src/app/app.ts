import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavBar } from '@shared/componentes/nav-bar/nav-bar';
import { MessageService } from 'primeng/api';
import { ToastModule } from 'primeng/toast';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet,NavBar,ToastModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
  providers : [MessageService]
})
export class App {
  protected readonly title = signal('front');

  messageService = inject(MessageService);
}
