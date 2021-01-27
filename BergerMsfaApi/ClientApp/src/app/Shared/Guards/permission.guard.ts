import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ActivityPermissionService } from '../Services/Activity-Permission/activity-permission.service';
import { AuthService } from '../Services/Users';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard implements CanActivate {
  
  constructor(
    private router: Router,
    private activityPermissionService: ActivityPermissionService,
    private authService: AuthService
  ) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      const permissionType = next.data.permissionType || '';
      const permissionGroup = next.data.permissionGroup || '';
      const isLoggedIn = this.authService.isLoggedIn && !this.authService.isTokenExpired;
      if (isLoggedIn) {
        if (this.authService.currentUserValue.roleName == 'Admin' && (permissionGroup.startsWith('/menu') || permissionGroup.startsWith('menu'))) return true;

        if (permissionGroup && permissionType && !this.activityPermissionService.hasPermission(permissionType, permissionGroup)) {
          this.router.navigate(['/access-denied/access-denied']);
          return false;
        }
        
        return true;
      }
      
      this.router.navigate(['/auth/login'], { queryParams: { returnUrl: state.url } });
      return false;
  }

  getErrorMsg(value) : string {
    let msg = "You don't have permission"
    switch (value) {
        case "view":
            msg += " to view."
            break;
        case "create":
            msg += " to create."
            break;
        case "update":
            msg += " to update."
            break;
        case "delete":
            msg += " to delete."
            break;
        default:
            msg += ".";
            break;
    }
    return msg;
  }

}

