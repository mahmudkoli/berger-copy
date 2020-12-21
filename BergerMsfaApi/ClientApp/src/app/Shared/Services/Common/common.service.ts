import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity';
import { HttpClient, HttpParams } from '@angular/common/http';

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
  
  toQueryString(obj) {
    let parts = [];
    for (const property in obj) {
      const value = obj[property];
      if (value != null && value !== undefined) {
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
      }
    }

    return parts.join('&');
  }

  toFormData(obj) {
    let formData = new FormData();
    for (const property in obj) {
      const value = obj[property];
      if (value != null && value !== undefined) {
        if(value && Array.isArray(value)) {
          value.forEach((element, index) => {
            this.appendFormDataNestedObject(formData, element, property, index);
          });
        } else {
            this.appendFormDataNestedObject(formData, value, property, null);
        }
      }
    }

    return formData;
  }

  objectToJson(value) {
    if (typeof(value) === 'object' && !(value instanceof File)) {
      return JSON.stringify(value);
    }
    else
      return value;
  }

  jsonToObject(value) {
      return JSON.parse(value);;
  }

  private appendFormDataNestedObject(formData, value, property, index: null | number) {
    if(typeof(value) === 'object' && !(value instanceof File)) {
      for (let subKey in value) {
        formData.append(`${property}${index===null?'':'['+index+']'}[${subKey}]`, value[subKey]);
      }
    }
    else {
      formData.append(property, value);
    }
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

    getDepotList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/Common/getDepotList');
    }
    getUserInfoList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/Common/getUserInfoList');
    }
    public getDealerList(userCategory: string, userCategoryIds: string[]) {
        var params = new HttpParams();
        params= params.append("userCategory", userCategory);
        if (userCategoryIds)
            userCategoryIds.forEach(v => {
                params=params.append("userCategoryIds", v)
            });
    
        return this.http.get<any>(this.baseUrl + `v1/AppDealer/getDealerList`, { params });
    }

}
