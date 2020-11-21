import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class JourneyPlanService {

    public baseUrl: string;

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') baseUrl: string
    ) {

        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }
    public getJourneyPlanListPaging(index: number, pageSize: number, planDate: string) {
        return this.http.get<APIResponse>(this.baseUrl + `v1/journeyPlan/GetJourneyPlanListPaging/${index}/${pageSize}?planDate=${planDate}`);
    }
 
    public getJourneyPlanList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/journeyPlan/GetJourneyPlanList');
    }
    public getLinerManagerJourneyPlanList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/journeyPlan/GetLineManagerJourneyPlanDetail');
    }
    
    public getJourneyPlanDetailById(id) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/journeyPlan/GetJourneyPlanDetailById/' + id);
    }

    public getJourneyPlanById(id) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/journeyPlan/getJourneyPlanById/' + id);
    }
    public ChangePlanStatus(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/journeyPlan/ChangeJourneyPlanStatus/', model);
    }

    public create(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/journeyPlan/CreateJourneyPlan', model);

    }
    public update(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/journeyPlan/UpdateJourneyPlan', model);
    }
    public delete(id: number) {
        return this.http.delete<any>(this.baseUrl + 'v1/journeyPlan/DeleteJourneyPlan/' + id);
    }
    public getDealerList() {
        return this.http.get<any>(this.baseUrl + 'v1/Common/GetDealList');
    }
}
