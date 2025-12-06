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
  isRegisterMode: boolean = false;

  constructor(
    private loginService: LoginService,
    private router: Router
  ) {}

  toggleMode(): void {
    this.isRegisterMode = !this.isRegisterMode;
    this.errorMessage = '';
    this.credentials = {
      jmeno: '',
      prijmeni: '',
      heslo: ''
    };
  }

  onSubmit(): void {
    if (!this.credentials.jmeno || !this.credentials.prijmeni || !this.credentials.heslo) {
      this.errorMessage = 'Prosím vyplňte všechna pole';
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';

    const authObservable = this.isRegisterMode 
      ? this.loginService.register(this.credentials)
      : this.loginService.login(this.credentials);

    authObservable.subscribe({
      next: (response) => {
        console.log(this.isRegisterMode ? 'Registrace úspěšná:' : 'Přihlášení úspěšné:', response);
        this.isLoading = false;
        this.router.navigate(['/teams']);
      },
      error: (error) => {
        console.error(this.isRegisterMode ? 'Chyba při registraci:' : 'Chyba při přihlášení:', error);
        this.isLoading = false;
        this.errorMessage = error.error?.error || 
          (this.isRegisterMode ? 'Registrace se nezdařila.' : 'Přihlášení se nezdařilo. Zkontrolujte své údaje.');
      }
    });
  }
}
