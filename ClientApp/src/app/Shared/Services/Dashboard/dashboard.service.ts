import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  public baseUrl: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    getDashboardData() {
      return this.http.get<APIResponse>(this.baseUrl + 'v1/dashboard');
  }
}
