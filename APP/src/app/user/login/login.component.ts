import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/_services/Auth.service';
import { FacebookLoginProvider, SocialAuthService, AmazonLoginProvider } from 'angularx-social-login';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, private toastr: ToastrService, private socialAuthService: SocialAuthService) {

  }

  LoggedIn = false;

  ngOnInit(): void {
    this.LoggedIn = this.authService.checkLogin();

  }

  loginWhitFacebook(): void{
    this.LoginSocial(FacebookLoginProvider.PROVIDER_ID);
  }

  loginWhitAmazon(): void{
    this.LoginSocial(AmazonLoginProvider.PROVIDER_ID);
  }

  LoginSocial(providerId: string): void {
    console.log(providerId);
    this.socialAuthService.signIn(providerId).finally(() => {
      this.socialAuthService.authState.subscribe((user) => {
        this.authService.loginSocial({
          provider: user.provider,
          authToken: user.authToken
        }).subscribe(() => {
            this.toastr.success('Logado com sucesso!', 'Bem vindo');
            this.LoggedIn = this.authService.checkLogin();
        }, (error) => {
          this.toastr.error('Houve um erro inesperado ao tentar fazer login. Detalhes: ' + error, 'Ops! Login falhou.');
          this.LoggedIn = this.authService.checkLogin();
        });
      });
    });
  }

  logout(): void {
    this.authService.logout();
    this.LoggedIn = this.authService.checkLogin();
  }
}

