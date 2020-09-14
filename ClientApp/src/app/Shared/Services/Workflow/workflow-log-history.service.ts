import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';

@Injectable({
  providedIn: 'root'
})
export class WorkflowLogHistoryService {

  public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    getWorkflowLogHistoryList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/workflowloghistory');
    }


    public postWorkflowLogHistory(model) {

        return this.http.post<APIResponse>(this.baseUrl + 'v1/workflowloghistory/create', model);
    }


    getWorkflowLogHistoryForCurrentUser(pageNumber, pageSize) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/workflowloghistory/get-workflow-log-history-for-current-user', {
            params: {
                status: status,
                pageNumber: pageNumber,
                pageSize: pageSize
            }
        });
    }


    updateWorkflowLogHistory(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/workflowloghistory/save', model);
    }

    deleteWorkflowLogHistory(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/workflowloghistory/delete/${id}`);
    }

    getWorkflowLogHistoryById(id: number) {
        return this.http.get(`${this.baseUrl}v1/workflowloghistory/${id}`);
    }
}
