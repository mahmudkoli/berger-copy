import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { MenuActivityPermission } from '../../Entity/Menu/menu-activity-permission.model';
import { MenuActivity } from '../../Entity/Menu/menu-activity';
import { MenuActivityPermissionVm } from '../../Entity/Menu/menu-activity-permission-vm.model';

@Injectable({
    providedIn: 'root'
})

export class MenuActivityPermissionService {
    public url: string;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = `${baseUrl}api/v1/menuactivitypermission`;
    }


    public createActivityPermission(activityPermissionModel: any) {
        return this.http.post<any>(`${this.url}/create`, activityPermissionModel);
    }

    public getAllActivityPermission() {
        return this.http.get<APIResponse>(`${this.url}`);
    }
    public getActivityPermissionById(id: number) {
        return this.http.get(`${this.url}/${id}`);
    }
    
    public getAllMenuActivityPermissionByRoleId(id: number) {
        // debugger
        return this.http.get<APIResponse>(`${this.url}/activity_permission_by_role_id/${id}`);
    }

    public deleteActivityPermission(id: number) {
        return this.http.delete(`${this.url}/delete/${id}`);
    }

    public updateActivityPermission(activityPermissionModel: any) {
        return this.http.put(`${this.url}/update/`, activityPermissionModel);
    }

    public createOrUpdateAllActivityPermission(model : MenuActivityPermissionVm[]) {

        console.log("Post Model : ", model);

        return this.http.post<APIResponse>(`${this.url}/createorupdateall/`, model);
    }

}