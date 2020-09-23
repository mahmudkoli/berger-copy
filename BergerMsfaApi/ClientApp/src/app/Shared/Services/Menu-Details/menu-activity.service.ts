import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';

@Injectable({
    providedIn: 'root'
})

export class MenuActivityService {
    public url: string;
    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.url = `${baseUrl}api/v1/menuactivity`;
    }
    public createActivity(activityModel:any) {
        return this.http.post<any>(`${this.url}/create`, activityModel);
    }

    public getAllActivity() {
        return this.http.get<APIResponse>(`${this.url}`);
    }
    public getActivityById(id: number) {
        return this.http.get(`${this.url}/${id}`);
    }

    public getAllActivityById(id: number) {
        return this.http.get(`${this.url}/get-all/${id}`);
    }

    public deleteActivity(id:number) {
        return this.http.delete(`${this.url}/delete/${id}`);
    }

    public updateActivity(activityModel:any) {
        return this.http.put(`${this.url}/update/`, activityModel);
    }

    public getAllMenuActivityPermissionByRoleId(id: number)
    {
        return this.http.get<APIResponse>(`${this.url}/get-all-menu-activity-permissions-by-roleid/${id}`)
    }
}