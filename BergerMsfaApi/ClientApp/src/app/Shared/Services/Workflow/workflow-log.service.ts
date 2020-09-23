import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';

@Injectable({
  providedIn: 'root'
})
export class WorkflowLogService {

  public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    getWorkflowLogList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/workflowlog');
    }


    getWorkflowLogForCurrentUser(status, pageNumber, pageSize) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/workflowlog/get-workflow-log-for-current-user', {
                params: {
                    status: status,
                    pageNumber: pageNumber,
                    pageSize: pageSize
                }
            });
    }


    public postWorkflowLog(model) {

        console.log("Post Model : ", model);

        return this.http.post<APIResponse>(this.baseUrl + 'v1/workflowlog/save', model);
    }


    updateWorkflowLog(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/workflowlog/save', model);
    }

    deleteWorkflowLog(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/workflowlog/delete/${id}`);
    }

    getWorkflowLogById(id: number) {
        return this.http.get(`${this.baseUrl}v1/workflowlog/${id}`);
    }

}
