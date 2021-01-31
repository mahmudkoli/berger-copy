import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';

@Injectable({
    providedIn: 'root'
})

export class UserService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
        private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }
    
    createUserInfo(userInfoModel) {
        userInfoModel.id=0;
        console.log(userInfoModel);
        return this.http.post<APIResponse>(this.baseUrl + 'v1/userinfo/create', userInfoModel);
    }

    getAdUserInfo(adUserName:any) {
        console.log(adUserName);
        return this.http.get<APIResponse>(this.baseUrl + `v1/userinfo/getaduser/${adUserName}`);
    }
    
    updateUserInfo(userInfoModel) {
        console.log(userInfoModel);
        return this.http.put<APIResponse>(this.baseUrl + 'v1/userinfo/update', userInfoModel);
    }

    getAllUserInfo(filter?) {
        return this.http.get<APIResponse>(this.baseUrl + `v1/userinfo?${this.commonService.toQueryString(filter)}`);
    }

    getUserInfoById(id) {
        return this.http.get<APIResponse>(`${this.baseUrl}v1/userinfo/getUserById/${id}`);
    }

    deleteUserInfo(id) {
        return this.http.delete<APIResponse>(`${this.baseUrl}v1/userinfo/delete/${id}`);
    }

    public postRoleLinkWithUser(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/userinfo/rolelinkwithuser', model);
    }
}