import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/Auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  loggedIn!: boolean;

  constructor(private authService: AuthService) {

  }

  ngOnInit() {

  }

   login(){
    this.authService.loginWhitFB();
  }


}

