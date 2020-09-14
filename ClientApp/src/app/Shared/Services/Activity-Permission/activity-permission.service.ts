import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { MenuActivityPermission } from '../../Entity/Menu/menu-activity-permission.model';
import { MenuActivity } from '../../Entity/Menu/menu-activity';
import { CommonService } from '../Common/common.service';

@Injectable({
    providedIn: 'root'
})

export class ActivityPermissionService {
    public url: string;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
        private commonService: CommonService) {
        this.url = `${baseUrl}api/v1/menuactivitypermission`;
    }
    
    public getActivityPermissions(roleId) {
        return this.http.get<APIResponse>(`${this.url}/get-activity-permission/${roleId}`);
    }

    setActivityPermissionToSession() { 
        let roleId = 0;
        const userInfo = this.commonService.getUserInfoFromLocalStorage();
        if(userInfo) {
            roleId = userInfo.roleId;
        }
        this.getActivityPermissions(roleId).subscribe(
            (res: any) => {
                console.log(res.data);  
                this.commonService.setActivityPermissionToSessionStorage(res.data);   
            },
            (err: any) => {
                console.log(err);
            }
        );
    }

    getActivityPermissionFromSession(): any[] {
        if(!this.commonService.getActivityPermissionToSessionStorage()) {
            this.setActivityPermissionToSession();
        }
        const sessionStorageActPer = this.commonService.getActivityPermissionToSessionStorage();
        const activityPermission = sessionStorageActPer ? sessionStorageActPer as any[] : [];
        return activityPermission;
    }
  
    hasPermission(permissionType, permissionGroup): boolean {
      let activityPermission = this.getActivityPermissionFromSession();
      let hasPermission = false;
      
      console.log(activityPermission);
      if (activityPermission && activityPermission.length > 0) {
          const filterActPer = activityPermission.filter(per => { 
            const actPer:any[] = per.url.split('/').filter(e => e);
            const perCode:any[] = permissionGroup.split('/').filter(e => e);
            if (actPer.length != perCode.length) { return false; }
            for (let i = 0; i < perCode.length; i++) {
              if (perCode[i].toLowerCase() != actPer[i].toLowerCase()) { return false; }
            }
            return true; 
          });
  
          console.log(filterActPer);
          if (filterActPer && filterActPer.length > 0) {
              if ((permissionType == "view" && filterActPer[0].canView) || 
                  (permissionType == "create" && filterActPer[0].canInsert) || 
                  (permissionType == "update" && filterActPer[0].canUpdate) || 
                  (permissionType == "delete" && filterActPer[0].canDelete)) {
                      hasPermission = true;
              }
          }
      }
  
      return hasPermission;
  }
  
  getPermission(permissionGroup): PermissionGroup {
    let perGroup:PermissionGroup = {canView: false, canCreate: false, canUpdate: false, canDelete: false};
    let activityPermission = this.getActivityPermissionFromSession();
    
    console.log(activityPermission);
    if (activityPermission && activityPermission.length > 0) {
        const filterActPer = activityPermission.filter(per => { 
          const actPer:any[] = per.url.split('/').filter(e => e);
          const perCode:any[] = permissionGroup.split('/').filter(e => e);
          if (actPer.length != perCode.length) { return false; }
          for (let i = 0; i < perCode.length; i++) {
            if (perCode[i].toLowerCase() != actPer[i].toLowerCase()) { return false; }
          }
          return true; 
        });

        console.log(filterActPer);
        if (filterActPer && filterActPer.length > 0) {
            const grp = filterActPer[0];
            perGroup.canView = grp.canView;
            perGroup.canCreate = grp.canInsert;
            perGroup.canUpdate = grp.canUpdate;
            perGroup.canDelete = grp.canDelete;
        }
    }

    return perGroup;
  }

}

export class PermissionGroup {
    canView: boolean = false;
    canCreate: boolean = false;
    canUpdate: boolean = false;
    canDelete: boolean = false;
}