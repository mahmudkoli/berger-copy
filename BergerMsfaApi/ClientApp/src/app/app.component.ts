import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
})

export class AppComponent {
    title = 'ArchitectUI - Angular 7 Bootstrap 4 & Material Design Admin Dashboard Template';
    isIframe: boolean = false;

    constructor(
        private router: Router
    ) { }

}
