import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity/Response/api-response';
@Injectable({
    providedIn: 'root'
})
export class WorkflowconfigurationService {
    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public getWorkFlowConfigurationList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/workflowconfiguration');
    }

    public getWorkFlowConfigurationById(id: number) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/workflowconfiguration/' + id);
    }

    public postWorkFlowConfiguration(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/workflowconfiguration/save', model);
    }

    public putWorkFlowConfiguration(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/workflowconfiguration/update', model);
    }

    public deleteWorkFlowConfiguration(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/workflowconfiguration/delete/${id}`);
    }

    
}
