import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity/Response/api-response';

@Injectable({
    providedIn: 'root'
})
export class WorkFlowService {

    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    getWorkFLowList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/workflow');
    }
        getWorflowType() {
            return this.http.get<APIResponse>(this.baseUrl + 'v1/workflow/workflowtype');
        }

    getWorkflowListWithConfig() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/workflow/workflowtree');// v1/controller/controller function
    }


    public postWorkFlow(model) {

        return this.http.post<APIResponse>(this.baseUrl + 'v1/workflow/save', model);
    }

    deleteWorkFlow(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/workflow/delete/${id}`);
    }

    getWorkflowById(id: number) {
        return this.http.get(`${this.baseUrl}v1/workflow/${id}`);
    }


}