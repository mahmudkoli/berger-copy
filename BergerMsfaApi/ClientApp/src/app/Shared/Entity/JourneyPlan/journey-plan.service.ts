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

    public getJourneyPlanList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/journeyPlan/getJourneyPlanList');
    }
    public getJourneyPlanById(id) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/journeyPlan/getJourneyPlanById/' + id);
    }

    public create(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/journeyPlan/create', model);

    }
    public update(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/journeyPlan/update', model);
    }
    public delete(id: number) {
        return this.http.delete<any>(this.baseUrl + 'v1/journeyPlan/' + id);
    }
}
