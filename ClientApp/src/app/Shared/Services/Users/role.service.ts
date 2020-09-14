import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
@Injectable({
    providedIn: 'root'
})
export class RoleService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    getRoleList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/role');
    }

    public getRole(id: number) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/role/' + id);
    }

    public postRole(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/role/save', model);
    }

    public putRole(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/role/update', model);
    }

    public deleteRole(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/role/delete/${id}`);
    }
}