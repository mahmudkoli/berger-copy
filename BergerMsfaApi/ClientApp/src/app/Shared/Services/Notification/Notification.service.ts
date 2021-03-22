import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

    public baseUrl: string;

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') baseUrl: string
    ) {

        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }


    public getJourneyPlanList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/Notification/GetAllNotification');
    }

}
