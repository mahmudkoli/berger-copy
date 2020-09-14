import { Component, OnInit } from '@angular/core';

import { LoginService } from 'src/app/Shared/Services/Users';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment.prod';
import { Location } from '@angular/common'
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
const requestObj = {
  scopes: ['user.read.all']
};

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  adtoken: string;
  adTokenFromCache: boolean;
  profile;
  loggedIn: boolean;
  error: string;

  constructor(private loginService: LoginService,

    private router: Router,
    private commonService: CommonService,
    private location: Location) { }

  ngOnInit() {
  
  }
  checkoutAccount() {
    this.loggedIn = !!this.loginService.IsLoggedIn();
}

getProfile() {
    
}


login(): any {

    const loginModel = '{ "AdGuid" : "' + this.profile.id + '" , "email": "' + this.profile.givenName + '" }';
    const credentials = loginModel;
    this.loginService.Login(credentials).subscribe(
        (response: any) => {
            if (response.data.result !== null) {
                var data = response.data.result;
                const token = data.token;
                localStorage.setItem('fmapptoken', token);
                localStorage.setItem('fmapptoken.expiry', data.expiration);
                this.loginService.getAdUser().subscribe((res: any) => {
                    debugger;
                    console.log("User Data.......+++", res.data);
                    // localStorage.setItem('userinfo', JSON.stringify(res.data));
                    this.commonService.setUserInfoToLocalStorage(res.data);
                    return this.router.navigate(['/']);              
                });
                
            }
            else {
                return this.router.navigate(['/login/unauthorized']);
            }

        },
        error => {

            
            localStorage.clear();
            return this.router.navigate(['/login/unauthorized']);

        }

    );
    }

    Reload() {
        window.location.href = environment.redirectUri;
    }

}
