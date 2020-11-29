import {Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { Route } from 'src/app/Shared/Entity';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import {ThemeOptions} from '../../../../../../theme-options';

  const requestObj = {
    scopes: ['user.read']
  };

@Component({
  selector: 'app-user-box',
  templateUrl: './user-box.component.html',
})
export class UserBoxComponent implements OnInit {
    profile;
    adguid: any;
  isIframe = false;
  loggedIn = false;
  production = false;
  constructor(
    public globals: ThemeOptions, public commonService: CommonService , public route : Router
      
      
     ) { }

  ngOnInit() {
    //this.isIframe = window !== window.parent && !window.opener;
    //// this.login();
    // this.checkoutAccount();

    //   this.broadcastService.subscribe('msal:loginSuccess', () => {
    //     this.checkoutAccount();               
    //   });
      this.getProfile();  
  }

 
  //login() {
  //  const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

  //  if (!isIE) {
  //    this.authService.loginRedirect();
  //  } else {
  //    this.authService.loginRedirect();
  //  }
  //}

    getProfile() {
     this.profile = this.commonService.getUserInfoFromLocalStorage();
     console.log("Profile View", this.profile);
    }


  logout() {
    localStorage.clear();
    this.route.navigate['/login/loginboxed'];
  }

 

}
