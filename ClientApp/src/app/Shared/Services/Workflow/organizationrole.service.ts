import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity/Response/api-response';

@Injectable({
  providedIn: 'root'
})

export class OrganizationRoleService {

    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    getOrganizationRoleList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/organizationrole');
    }




    getOrgUserList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/organizationrole/orgusertree');
    }
    public getOrganizationRole(id: number) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/organizationrole/' + id);
    }
    public getOrgnazationuserByRole(orgId: number) {
        //getuserbyrol/{orgroleId}
        return this.http.get<APIResponse>(this.baseUrl + 'v1/organizationrole/getuserbyrol/' + orgId);
    }
    public postOrganizationRole(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/organizationrole/save', model);
    }

    public putOrganizationRole(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/organizationrole/update', model);
    }

    public deleteOrganizationRole(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/organizationrole/delete/${id}`);
    }

    public postOrganizationRoleLinkWithUser(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/organizationrole/rolelinkwithuser', model);
    }

}


