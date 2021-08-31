import { HttpClient, HttpParams } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { APIResponse } from '../../Entity';
import { IAuthUser } from '../../Entity/Users/auth';

@Injectable({
  providedIn: 'root',
})
export class CommonService {
  public PAGE_SIZE: number = 10;
  public baseUrl: string;
  private readonly localStorageCurrentUserKey: string = 'currentUser';
  private readonly localStorageActivityPermissionKey: string =
    'activityPermission';

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    console.log('baseUrl: ', baseUrl);
    this.baseUrl = baseUrl + 'api/';
  }

  toQueryString(obj) {
    let parts = [];
    for (const property in obj) {
      const value = obj[property];
      if (value != null && value !== undefined) {
        if (Array.isArray(value)) {
          value.forEach((val, index) => {
            parts.push(
              encodeURIComponent(property) + '=' + encodeURIComponent(val)
            );
          });
        } else if (value instanceof Date) {
          const newValue = new Date(
            Date.UTC(
              value.getFullYear(),
              value.getMonth(),
              value.getDate(),
              value.getHours(),
              value.getMinutes(),
              value.getSeconds()
            )
          );
          parts.push(
            encodeURIComponent(property) + '=' + newValue.toISOString()
          );
        } else {
          parts.push(
            encodeURIComponent(property) + '=' + encodeURIComponent(value)
          );
        }
      }
    }

    return parts.join('&');
  }

  toFormData(obj) {
    let formData = new FormData();
    for (const property in obj) {
      const value = obj[property];
      if (value != null && value !== undefined) {
        if (value && Array.isArray(value)) {
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
    if (typeof value === 'object' && !(value instanceof File)) {
      return JSON.stringify(value);
    } else return value;
  }

  jsonToObject(value) {
    return JSON.parse(value);
  }

  booleanToText(
    obj: any,
    postFixText: string = 'Text',
    trueText: string = 'YES',
    falseText: string = 'NO'
  ) {
    let entries = Object.entries(obj) || [];
    // console.log(entries);
    entries.forEach(([key, value]) => {
      // let value = obj[key];
      let keyText = key + postFixText;
      if (typeof value === 'boolean') {
        obj[keyText] = value ? trueText : falseText;
      }
    });
  }

  insertSpaces(value) {
    return value
      .replace(/(_|-)/g, ' ')
      .trim()
      .replace(/\w\S*/g, function (str) {
        return str.charAt(0).toUpperCase() + str.substr(1);
      })
      .replace(/([a-z])([A-Z])/g, '$1 $2')
      .replace(/([A-Z])([A-Z][a-z])/g, '$1 $2');
  }

  renameKeys(obj, keysMap) {
    return Object.keys(obj).reduce(
      (acc, key) => ({
        ...acc,
        ...{ [keysMap[key] || key]: obj[key] },
      }),
      {}
    );
  }

  private appendFormDataNestedObject(
    formData,
    value,
    property,
    index: null | number
  ) {
    if (typeof value === 'object' && !(value instanceof File)) {
      for (let subKey in value) {
        formData.append(
          `${property}${index === null ? '' : '[' + index + ']'}[${subKey}]`,
          value[subKey]
        );
      }
    } else {
      formData.append(property, value);
    }
  }

  // setUserInfoToLocalStorage(value: IAuthUser) {
  //   localStorage.setItem(this.localStorageCurrentUserKey, JSON.stringify(value));
  // }

  getUserInfoFromLocalStorage(): IAuthUser | null {
    if (!localStorage.getItem(this.localStorageCurrentUserKey)) return null;
    return JSON.parse(
      localStorage.getItem(this.localStorageCurrentUserKey)
    ) as IAuthUser;
  }

  setActivityPermissionToSessionStorage(value) {
    localStorage.setItem(
      this.localStorageActivityPermissionKey,
      JSON.stringify(value)
    );
  }

  getActivityPermissionFromSessionStorage(): any | null {
    if (!localStorage.getItem(this.localStorageActivityPermissionKey))
      return null;
    return JSON.parse(
      localStorage.getItem(this.localStorageActivityPermissionKey)
    );
  }

  getSaleOfficeList() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/Common/getSaleOfficeList'
    );
  }

  getSaleGroupList() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/Common/getSaleGroupList'
    );
  }

  getTerritoryList() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/Common/getTerritoryList'
    );
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

  getUserInfoListByCurrentUser() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/Common/getUserInfoListByCurrentUser'
    );
  }

  getUserInfoListByCurrentUserWithoutZoUser() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/Common/getUserInfoListByCurrentUserWithoutZoUser'
    );
  }

  getUserInfoList() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/Common/getUserInfoList'
    );
  }

  getPainterList() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/Common/getPainterList'
    );
  }

  getMonthList() {
    return this.http.get<APIResponse>(this.baseUrl + 'v1/Common/getMonthList');
  }

  getYearList() {
    return this.http.get<APIResponse>(this.baseUrl + 'v1/Common/getYearList');
  }

  getCreditControlAreaList() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/AppCommon/GetCreditControlAreaList'
    );
  }

  getDivisionList() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/Common/GetDivisionList'
    );
  }

  getBrandDropDown() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/Common/GetBrandDropDown'
    );
  }

  getMaterialGroupOrBrand(filter?) {
    return this.http.get<APIResponse>(
      this.baseUrl + `v1/Common/GetMaterialGroupOrBrand?${this.toQueryString(filter)}`
    );
  }

  getActivitySummaryDropDown() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/Common/GetActivitySummaryDropDown'
    );
  }

  getEnumClubSupreme() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/Enum/GetEnumClubSupreme'
    );
  }

  public getDealerList(userCategory: string, userCategoryIds: string[]) {
    var params = new HttpParams();
    params = params.append('userCategory', userCategory);
    if (userCategoryIds) {
      userCategoryIds.forEach((v) => {
        params = params.append('userCategoryIds', v);
      });
    }

    return this.http.get<any>(this.baseUrl + `v1/AppDealer/getDealerList`, {
      params,
    });
  }

  getDealerByArea(filter) {
    return this.http.post<APIResponse>(
      this.baseUrl + `v1/Common/GetDealerByArea`,
      filter
    );
  }

  public getFiscalYear() {
    let curentDate = new Date();
    if (curentDate.getMonth() >= 3) {
      return curentDate.getFullYear();
    } else {
      return curentDate.getFullYear() - 1;
    }
  }
}
