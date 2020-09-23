import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';

@Injectable({
    providedIn: 'root'
})
export class DelegationService {

    public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    getDelegationList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/delegation');
    }

    getDelegationNewList() {
        return this.http.get<APIResponse>(`${this.baseUrl}v1/delegation/getnew`);
    }

    getDelegationPastList() {
        return this.http.get<APIResponse>(`${this.baseUrl}v1/delegation/getlogs`);
    }

    public postDelegation(model) {
        console.log("Post Model : ", model);
        return this.http.post<APIResponse>(this.baseUrl + 'v1/delegation/save', model);
    }

    putDelegation(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/delegation/update', model);
    }

    deleteDelegation(id: number) {
        return this.http.delete<any>(`${this.baseUrl}v1/delegation/delete/${id}`);
    }

    getDelegationById(id: number) {
        return this.http.get(`${this.baseUrl}v1/delegation/${id}`);
    }

}
