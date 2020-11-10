﻿import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';



@Injectable({ providedIn: 'root' })

export class SchemeService {

    public baseUrl: string;

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') baseUrl: string
    ) {

        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public getSchemeDetailWithMaster() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/SchemeDetail/getSchemeDetailWithMaster');
    } public getSchemeMasterList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/SchemeMaster/getSchemeMasterList');
    }
    public createSchemeMaster(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/SchemeMaster/createSchemeMaster', model);

    }
    public UpdateSchemeMaster(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/SchemeMaster/updateSchemeMaster', model);

    }

    public DeleteSchemeMaster(id: number) {
        return this.http.delete<any>(this.baseUrl + 'v1/SchemeMaster/DeleteSchemeMaster/' + id);
    }

    public getSchemeMasterById(id) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/SchemeMaster/GetSchemeMasterById/' + id);
    }

    //scheme detail
    public getSchemeDetailList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/SchemeDetail/getSchemeDetailList');
    }

    public getSchemeDetailById(id) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/SchemeDetail/getSchemeDetailById/' + id);
    }

    public createSchemeDetail(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/SchemeDetail/createSchemeDetail', model);

    }
    public updateSchemeDetail(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/SchemeMaster/updateSchemeDetail', model);

    }

    public deleteSchemeDetail(id: number) {
        return this.http.delete<any>(this.baseUrl + 'v1/SchemeDetail/deleteSchemeDetail/' + id);
    }

}