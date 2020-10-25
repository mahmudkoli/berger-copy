import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AlertService } from '../Modules/alert/alert.service';
import { ActivityPermissionService } from '../Services/Activity-Permission/activity-permission.service';
import { CommonService } from '../Services/Common/common.service';

@Injectable({
  providedIn: 'root'
})
export class PermissionGuard implements CanActivate {
  
  constructor(
    private router: Router, private alertService: AlertService,
    private activityPermissionService: ActivityPermissionService,
    private commonService: CommonService
  ) { }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      debugger;
      const permissionType = next.data.permissionType || '';
      const permissionGroup = next.data.permissionGroup || '';
      const hasPermission = this.activityPermissionService.hasPermission(permissionType, permissionGroup);
      debugger;
      // if (!hasPermission) {

      //   const user = this.commonService.getUserInfoFromLocalStorage();
      //   if(user && user.roleName == 'Admin' && (permissionGroup.startsWith('/menu') || permissionGroup.startsWith('menu'))) return true;

      //   //const errorMsg = this.getErrorMsg(permissionType);
      //  // this.alertService.titleTosterDanger(errorMsg);

      //  this.router.navigate(['/access-denied/access-denied']);

      //   // setTimeout(() => {
      //   //     this.alertService.fnLoading(false);
      //   // }, 1000);
      // }

      return true;
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

