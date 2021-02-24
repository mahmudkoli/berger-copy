import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';
import { SaveSchemeDetail, SaveSchemeMaster } from '../../Entity/Scheme/SchemeMaster';

@Injectable({ providedIn: 'root' })

export class SchemeService {
    public baseUrl: string;

    constructor(private http: HttpClient,@Inject('BASE_URL') baseUrl: string,
        private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    //#region Scheme Master
    public getSchemeMasterList(index, pageSize, search="") {
        return this.http.get<APIResponse>(this.baseUrl + `v1/SchemeMaster/getSchemeMasterList/${index}/${pageSize}?search=${search}`);
    }

    public getSchemeMasterById(id) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/SchemeMaster/GetSchemeMasterById/' + id);
    }
    public createSchemeMaster(model: SaveSchemeMaster) {
        model.id = 0;
        return this.http.post<APIResponse>(this.baseUrl + 'v1/SchemeMaster/createSchemeMaster', model);
    }

    public UpdateSchemeMaster(model: SaveSchemeMaster) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/SchemeMaster/updateSchemeMaster', model);
    }

    public DeleteSchemeMaster(id) {
        return this.http.delete<any>(this.baseUrl + 'v1/SchemeMaster/DeleteSchemeMaster/' + id);
    }
  
    getAllSchemeMastersForSelect() {
      return this.http.get<APIResponse>(this.baseUrl + 'v1/SchemeMaster/getAllSchemeMastersForSelect');
    }
    //#endregion

    //#region Scheme Detail
    public getSchemeDetailList(index,pageSize,search="") {
        return this.http.get<APIResponse>(this.baseUrl + `v1/SchemeDetail/getSchemeDetailList/${index}/${pageSize}?search=${search}`);
    } 

    public getSchemeDetailById(id) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/SchemeDetail/getSchemeDetailById/' + id);
    }

    public createSchemeDetail(model: SaveSchemeDetail) {
        model.id = 0;
        return this.http.post<APIResponse>(this.baseUrl + 'v1/SchemeDetail/createSchemeDetail', model);
    }

    public updateSchemeDetail(model: SaveSchemeDetail) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/SchemeDetail/updateSchemeDetail', model);
    }

    public deleteSchemeDetail(id: number) {
        return this.http.delete<any>(this.baseUrl + 'v1/SchemeDetail/deleteSchemeDetail/' + id);
    }
    //#endregion
}
