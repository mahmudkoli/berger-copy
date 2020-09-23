import { Component } from '@angular/core';
import { LoginService } from './Shared/Services/Users/login.service';
import { Router } from '@angular/router';



@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
})

export class AppComponent {
    title = 'ArchitectUI - Angular 7 Bootstrap 4 & Material Design Admin Dashboard Template';
    profile;
    isIframe = false;
    loggedIn = false;
    public adTokenFromCache = false;
    adtoken: any;
    constructor(
        private loginService: LoginService,
        private router: Router
    ) { }

    // tslint:disable-next-line:use-life-cycle-interface
    ngOnInit(): void {
        
    }

}
