import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/_services/Auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, private toastr: ToastrService) {

  }

  LoggedIn: boolean = this.authService.checkLogin();

  ngOnInit(): void {
  }

  loginWhitFacebook(): void{
    this.authService.loginWhitFB();
    this.checkLoginSucess();
  }

  loginWhitAmazon(): void {
    this.authService.loginWhitAmazon();
    this.checkLoginSucess();
  }

  logout(): void {
    this.authService.logout();
  }

  checkLoginSucess(): void {
    if (this.authService.checkLogin()) {
      this.toastr.success('Logado com sucesso!', 'Bem vindo');
    } else {
      this.toastr.error('Verifique se você digitou corretamente seus dados.', 'Ops! Login falhou.');
    }
  }

}

