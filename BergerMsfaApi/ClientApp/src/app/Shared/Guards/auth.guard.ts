import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../Services/Users';
@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  
  constructor(
    private authService: AuthService,
    private router: Router,
    private activatedRoute: ActivatedRoute) { 
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      if (this.authService.isLoggedIn && !this.authService.isTokenExpired) {     
        return true;
      }
      else {       
        // this.router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });
        this.router.navigate(['/auth/login'], { relativeTo: this.activatedRoute });
        return false;
      }
  }
}

