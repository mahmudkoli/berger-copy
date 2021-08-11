import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';
import { SaveUniverseReachAnalysis } from '../../Entity/KPI/UniverseReachAnalysis';

@Injectable({ providedIn: 'root' })

export class UniverseReachAnalysisService {
    public baseUrl: string;
    public UniverseReachAnalysissEndpoint: string;

    constructor(private http: HttpClient,@Inject('BASE_URL') baseUrl: string,
        private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
        this.UniverseReachAnalysissEndpoint = this.baseUrl + 'v1/UniverseReachAnalysis';
    }

    //#region Collection Plan
    getUniverseReachAnalysiss(filter?) {
        return this.http.get<APIResponse>(`${this.UniverseReachAnalysissEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    public getUniverseReachAnalysisById(id) {
        return this.http.get<APIResponse>(`${this.UniverseReachAnalysissEndpoint}/${id}`);
    }

    public createUniverseReachAnalysis(model: SaveUniverseReachAnalysis) {
        model.id = 0;
        return this.http.post<APIResponse>(`${this.UniverseReachAnalysissEndpoint}`, model);
    }

    public updateUniverseReachAnalysis(model: SaveUniverseReachAnalysis) {
        return this.http.put<APIResponse>(`${this.UniverseReachAnalysissEndpoint}/${model.id}`, model);
    }

    public deleteUniverseReachAnalysis(id: number) {
        return this.http.delete<any>(`${this.UniverseReachAnalysissEndpoint}/${id}`);
    }
    //#endregion
}
