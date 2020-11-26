import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FacebookLoginProvider, SocialAuthService, AmazonLoginProvider } from 'angularx-social-login';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { KeysApp } from '../_utils/KeysApp';
import { AuthenticationSocial } from '../_models/AuthenticationSocial';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  urlBase = 'http://localhost:41966/api/user/';
  jwtHelper = new JwtHelperService();



  constructor(private http: HttpClient, private authService: SocialAuthService) { }


  loginWhitFB(): void {
    this.authService.signIn(FacebookLoginProvider.PROVIDER_ID).finally();
    this.getAuthState();
  }

  loginWhitAmazon(): void {
    this.authService.signIn(AmazonLoginProvider.PROVIDER_ID).finally();
    this.getAuthState();
  }

  logout(): void {
    this.authService.signOut();
    localStorage.removeItem(KeysApp.localstorageJWT);
  }

  checkLogin(): boolean {
    const token = JSON.parse(localStorage.getItem(KeysApp.localstorageJWT)!);

    if (!token) {
      return false;
    }

    const isTokenExpired = this.jwtHelper.isTokenExpired(token);

    if (isTokenExpired) {
      this.logout();
      return false;
    }

    return true;
  }

  private getAuthState(): void {

    this.authService.authState.subscribe((response) => {
      console.log(response);

      this.loginSocial({
        provider: response.provider,
        authToken: response.authToken
      });
    });
  }

  private loginSocial(auth: AuthenticationSocial): void {
    let actionAPI = '';
    switch (auth.provider) {
      case 'FACEBOOK':
        actionAPI = 'auth-facebook';
        break;
      case 'AMAZON':
        actionAPI = 'auth-amazon';
        break;
      default:
        break;
    }

    const url = `${this.urlBase}${actionAPI}?authToken=${auth.authToken}`;
    this.http.post(url, null).pipe(
      map((responseAPI: any) => {
        const user = responseAPI;
        console.log(responseAPI);
        if (user) {
          localStorage.setItem(KeysApp.localstorageJWT, user.token);
        }
      })).subscribe();
  }
}
