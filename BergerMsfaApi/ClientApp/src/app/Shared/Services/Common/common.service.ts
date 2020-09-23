import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor() { }

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
}
