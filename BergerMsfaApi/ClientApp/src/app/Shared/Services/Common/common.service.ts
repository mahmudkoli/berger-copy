import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

    public baseUrl: string;
    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') baseUrl: string
    ) {

        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

  toFormData(obj) {
    let formData = new FormData();
    for (const property in obj) {
      const value = obj[property];
      if (value != null && value !== undefined) {
        formData.append(property, value);
      }
    }

    return formData;
  }

  setUserInfoToLocalStorage(value) {
    localStorage.setItem('userinfo', JSON.stringify(value));
  }

  getUserInfoFromLocalStorage(): any {
    if(!localStorage.getItem('userinfo')) return null;
    return JSON.parse(localStorage.getItem('userinfo'));
  }

  setActivityPermissionToSessionStorage(value) {
    localStorage.setItem('activitypermission', JSON.stringify(value));
  }

  getActivityPermissionToSessionStorage(): any {
    if(!localStorage.getItem('activitypermission')) return null;
    return JSON.parse(localStorage.getItem('activitypermission'));
    }

    getSaleOfficeList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/Common/getSaleOfficeList');
    }
    getSaleGroupList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/Common/getSaleGroupList');
    }

    getTerritoryList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/Common/getTerritoryList');
    }

    getZoneList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/Common/getZoneList');
    }
    getRoleList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/Common/getRoleList');
    }

    getEmployeeList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/Common/getEmployeeList');
    }
    public getDealerList() {
        return this.http.get<any>(this.baseUrl + 'v1/Common/getDealerList');
    }

}
