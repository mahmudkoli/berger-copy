import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../Common/common.service';
import { APIResponse } from '../../Entity/Response/api-response';

@Injectable({
    providedIn: 'root'
})

export class LeadService {
    public baseUrl: string;
    public LeadsEndpoint: string;

    constructor(
      private http: HttpClient,
      @Inject('BASE_URL') baseUrl: string,
      private commonService: CommonService) {
        this.baseUrl = baseUrl + 'api/';
        this.LeadsEndpoint = this.baseUrl + 'v1/Lead';
    }
  
    getLead(id) {
      return this.http.get<APIResponse>(`${this.LeadsEndpoint}/${id}`);
    }
    updateLead(lead:any) {
      return this.http.post<any>(
        this.baseUrl + `v1/Lead/UpdateLeadGenerate/`,
        lead
      );
    }
    getLeads(filter?) {
      return this.http.get<APIResponse>(`${this.LeadsEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    deleteLeadFollowUp(id) {
        return this.http.delete<APIResponse>(`${this.LeadsEndpoint}/DeleteLeadFollowUp/${id}`);
    }

    DeleteLeadImage(obj) {
      return this.http.post<any>(
        this.baseUrl + `v1/Lead/DeleteImage/`,
        obj
      );
    }
    // activeInactive(id) {
    //   return this.http.post<APIResponse>(`${this.LeadsEndpoint}/activeInactive/${id}`, null);
    // }
}