import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginService, LoginRequest } from '../../services/login.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  credentials: LoginRequest = {
    jmeno: '',
    prijmeni: '',
    heslo: ''
  };

  errorMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private loginService: LoginService,
    private router: Router
  ) {}

  onSubmit(): void {
    if (!this.credentials.jmeno || !this.credentials.prijmeni || !this.credentials.heslo) {
      this.errorMessage = 'Prosím vyplňte všechna pole';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    this.loginService.login(this.credentials).subscribe({
      next: (response) => {
        console.log('Přihlášení úspěšné:', response);
        this.isLoading = false;
        // Přesměrovat na hlavní stránku nebo dashboard
        this.router.navigate(['/teams']);
      },
      error: (error) => {
        console.error('Chyba při přihlášení:', error);
        this.isLoading = false;
        this.errorMessage = error.error?.error || 'Přihlášení se nezdařilo. Zkontrolujte své údaje.';
      }
    });
  }
}
