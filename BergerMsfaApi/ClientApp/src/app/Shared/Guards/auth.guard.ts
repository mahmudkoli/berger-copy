import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { LoginService } from '../Services/Users';
import { MsalService } from '@azure/msal-angular';

const requestObj = {
  scopes: ['user.read.all']
};


@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  profile;
  isIframe = false;
  loggedIn = false;
  public adTokenFromCache = false;
  adtoken: any;
  constructor(
    private loginService: LoginService,
    private router: Router
  ) { }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      
   
    if (!this.loginService.IsLoggedIn()) {
       this.router.navigate(['/login/loginboxed']);
       return false;
    }
    else {
     return true;
    }
  }



  getProfile() {
    
  }

  login(): any {

    // const loginModel = '{ "AdGuid" : "' + this.profile.id + '" , "email": "' + this.profile.givenName + '" }';
    // const credentials = loginModel;
    // this.loginService.Login(credentials).subscribe(
    //   (response: any) => {
    //     debugger;
    //     if (response.data.result !== null) {
    //       var data = response.data.result;
    //       const token = data.token;
    //       localStorage.setItem('fmapptoken', token);
    //       localStorage.setItem('fmapptoken.expiry', data.expiration);
    //       this.loginService.getAdUser().subscribe(res => {
    //         localStorage.setItem('fmapptoken.expiry', data.expiration);
    //       })
    //       return true;
    //     }
    //     else {
    //       return false;
    //     }

    //   },
    //   error => {

    //     this.authService.clearCacheForScope(this.adtoken);
    //     localStorage.clear();
    //     return false;

    //   }

    //);
  }

  checkValidUser() {
    this.loggedIn = !!this.loginService.IsLoggedIn();
  }

}

