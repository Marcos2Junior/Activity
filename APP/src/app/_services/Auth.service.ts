import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FacebookLoginProvider, SocialAuthService } from 'angularx-social-login';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private authService: SocialAuthService) { }

  urlBase = 'http://localhost:41966/api/user/';

  loginWhitFB(): void {
    this.authService.signIn(FacebookLoginProvider.PROVIDER_ID);

    this.authService.authState.subscribe((response) => {
      const url = `${this.urlBase}auth-facebook`;
      console.log(url);
      this.http
    .post(url + '/' + response.authToken, null).pipe(
      map((response: any) => {
        const user = response;
        console.log(response);
        if (user) {

        }
      })
    ).subscribe();
    });
  }
}
