import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../Common/common.service';
import { APIResponse } from '../../Entity/Response/api-response';

@Injectable({
    providedIn: 'root'
})

export class DealerSalesCallService {
    public baseUrl: string;
    public DealerSalesCallsEndpoint: string;

    constructor(
      private http: HttpClient,
      @Inject('BASE_URL') baseUrl: string,
      private commonService: CommonService) {
        this.baseUrl = baseUrl + 'api/';
        this.DealerSalesCallsEndpoint = this.baseUrl + 'v1/DealerSalesCall';
    }
  
    getDealerSalesCall(id) {
      return this.http.get<APIResponse>(`${this.DealerSalesCallsEndpoint}/${id}`);
    }
  
    getDealerSalesCalls(filter?) {
      return this.http.get<APIResponse>(`${this.DealerSalesCallsEndpoint}?${this.commonService.toQueryString(filter)}`);
    }
  
    // activeInactive(id) {
    //   return this.http.post<APIResponse>(`${this.DealerSalesCallsEndpoint}/activeInactive/${id}`, null);
    // }
}