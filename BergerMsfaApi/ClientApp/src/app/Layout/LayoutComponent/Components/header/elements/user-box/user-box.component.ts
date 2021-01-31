import {Component, OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { IAuthUser } from 'src/app/Shared/Entity/Users/auth';
import { AuthService } from 'src/app/Shared/Services/Users';
import {ThemeOptions} from '../../../../../../theme-options';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-user-box',
  templateUrl: './user-box.component.html',
})
export class UserBoxComponent implements OnInit {
  user: IAuthUser;
  // production = false;

  constructor(
    public globals: ThemeOptions,
    private authService: AuthService,
    private router: Router) { 
  }

  ngOnInit() {
    // this.production = environment.production;
    this.authService.currentUser.subscribe(user => this.user = user);
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/auth/login']);
  }
}
