import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';
import { SaveSchemeDetail, SaveSchemeMaster } from '../../Entity/Scheme/SchemeMaster';

@Injectable({ providedIn: 'root' })

export class SchemeService {
    public baseUrl: string;
    public SchemeMastersEndpoint: string;
    public SchemeDetailsEndpoint: string;

    constructor(private http: HttpClient,@Inject('BASE_URL') baseUrl: string,
        private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
        this.SchemeMastersEndpoint = this.baseUrl + 'v1/SchemeMaster';
        this.SchemeDetailsEndpoint = this.baseUrl + 'v1/SchemeDetail';
    }

    //#region Scheme Master
    public getSchemeMasterList(index, pageSize, search="") {
        return this.http.get<APIResponse>(`${this.SchemeMastersEndpoint}/getSchemeMasterList/${index}/${pageSize}?search=${search}`);
    }

    getSchemeMasters(filter?) {
        return this.http.get<APIResponse>(`${this.SchemeMastersEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    public getSchemeMasterById(id) {
        return this.http.get<APIResponse>(`${this.SchemeMastersEndpoint}/${id}`);
    }

    public createSchemeMaster(model: SaveSchemeMaster) {
        model.id = 0;
        return this.http.post<APIResponse>(`${this.SchemeMastersEndpoint}`, model);
    }

    public updateSchemeMaster(model: SaveSchemeMaster) {
        return this.http.put<APIResponse>(`${this.SchemeMastersEndpoint}/${model.id}`, model);
    }

    public deleteSchemeMaster(id) {
        return this.http.delete<any>(`${this.SchemeMastersEndpoint}/${id}`);
    }

    getAllSchemeMastersForSelect() {
        return this.http.get<APIResponse>(`${this.SchemeMastersEndpoint}/select`);
    }
    //#endregion

    //#region Scheme Detail
    public getSchemeDetailList(index, pageSize, search="") {
        return this.http.get<APIResponse>(`${this.SchemeDetailsEndpoint}/getSchemeDetailList/${index}/${pageSize}?search=${search}`);
    }

    getSchemeDetails(filter?) {
        return this.http.get<APIResponse>(`${this.SchemeDetailsEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    public getSchemeDetailById(id) {
        return this.http.get<APIResponse>(`${this.SchemeDetailsEndpoint}/${id}`);
    }

    public createSchemeDetail(model: SaveSchemeDetail) {
        model.id = 0;
        return this.http.post<APIResponse>(`${this.SchemeDetailsEndpoint}`, model);
    }

    public updateSchemeDetail(model: SaveSchemeDetail) {
        return this.http.put<APIResponse>(`${this.SchemeDetailsEndpoint}/${model.id}`, model);
    }

    public deleteSchemeDetail(id: number) {
        return this.http.delete<any>(`${this.SchemeDetailsEndpoint}/${id}`);
    }
    //#endregion
}
