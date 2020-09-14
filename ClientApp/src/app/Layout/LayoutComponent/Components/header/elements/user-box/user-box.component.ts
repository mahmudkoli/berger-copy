import {Component, OnInit} from '@angular/core';
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
  constructor(
    public globals: ThemeOptions, 
      
      
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
       
    }


  logout() {
    
  }

 

}
