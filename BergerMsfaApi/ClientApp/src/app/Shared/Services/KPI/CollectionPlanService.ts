import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';
import { SaveCollectionPlan, SaveCollectionConfig } from '../../Entity/KPI/CollectionPlan';

@Injectable({ providedIn: 'root' })

export class CollectionPlanService {
    public baseUrl: string;
    public CollectionConfigsEndpoint: string;
    public CollectionPlansEndpoint: string;

    constructor(private http: HttpClient,@Inject('BASE_URL') baseUrl: string,
        private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
        this.CollectionConfigsEndpoint = this.baseUrl + 'v1/CollectionConfig';
        this.CollectionPlansEndpoint = this.baseUrl + 'v1/CollectionPlan';
    }

    //#region Collection Config
    getCollectionConfigs() {
        return this.http.get<APIResponse>(`${this.CollectionConfigsEndpoint}`);
    }

    public getCollectionConfigById(id) {
        return this.http.get<APIResponse>(`${this.CollectionConfigsEndpoint}/${id}`);
    }

    public updateCollectionConfig(model: SaveCollectionConfig) {
        return this.http.put<APIResponse>(`${this.CollectionConfigsEndpoint}/${model.id}`, model);
    }
    //#endregion

    //#region Collection Plan
    getCollectionPlans(filter?) {
        return this.http.get<APIResponse>(`${this.CollectionPlansEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    public getCollectionPlanById(id) {
        return this.http.get<APIResponse>(`${this.CollectionPlansEndpoint}/${id}`);
    }

    public createCollectionPlan(model: SaveCollectionPlan) {
        model.id = 0;
        return this.http.post<APIResponse>(`${this.CollectionPlansEndpoint}`, model);
    }

    public updateCollectionPlan(model: SaveCollectionPlan) {
        return this.http.put<APIResponse>(`${this.CollectionPlansEndpoint}/${model.id}`, model);
    }

    public deleteCollectionPlan(id: number) {
        return this.http.delete<any>(`${this.CollectionPlansEndpoint}/${id}`);
    }
    //#endregion
}
