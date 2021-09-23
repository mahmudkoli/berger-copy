import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { APIResponse } from '../../Entity/Response/api-response';
import { CommonService } from '../Common/common.service';

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


    updateDealerSalesCall(dealerSalesCall) {
      return this.http.put<any>(this.baseUrl + `v1/DealerSalesCall/UpdateDealerSalesCallList/`,dealerSalesCall);
    }

    getDealerSalesCalls(filter?) {
      return this.http.get<APIResponse>(`${this.DealerSalesCallsEndpoint}?${this.commonService.toQueryString(filter)}`);
    }




    public createEmailConfig(model) {
      // debugger;
      return this.http.post<APIResponse>(this.baseUrl + 'v1/EmailConfigDealerSales/CreateEmailConfig', model);

  }

  public updateEmailConfig(dealer) {
      return this.http.put<any>(this.baseUrl + `v1/EmailConfigDealerSales/UpdateEmailConfig/`,dealer);
  }

  public getEmailConfig() {
      return this.http.get<APIResponse>(this.baseUrl + `v1/EmailConfigDealerSales/GetEmailConfig`);
  }

  public getEmailConfigById(id) {
      return this.http.get<APIResponse>(this.baseUrl + 'v1/EmailConfigDealerSales/GetById/' + id);
  }

public DeleteDealerOppeningEmailById(id){
  return this.http.delete<APIResponse>(this.baseUrl + 'v1/EmailConfigDealerSales/DeleteById/' + id);

}

    // activeInactive(id) {
    //   return this.http.post<APIResponse>(`${this.DealerSalesCallsEndpoint}/activeInactive/${id}`, null);
    // }
}