import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';

@Injectable({ providedIn: 'root' })

export class NewDealerDevelopmentService {
    public baseUrl: string;
    public NewDealerDevelopmentEndpoint: string;
    public NewDealerDevelopmentSaveEndpoint: string;


    constructor(private http: HttpClient,@Inject('BASE_URL') baseUrl: string,
        private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
        this.NewDealerDevelopmentEndpoint = this.baseUrl + 'v1/NewDealerDevelopment/GetNewDealerDevelopment';
        this.NewDealerDevelopmentSaveEndpoint = this.baseUrl + 'v1/NewDealerDevelopment/SaveNewDealerDevelopment';

    }

    
    getNewDealerDevelopment(filter?) {
        return this.http.get<APIResponse>(`${this.NewDealerDevelopmentEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    SaveOrUpdateNewDealerDevelopment(filter) {
        return this.http.post<APIResponse>(`${this.NewDealerDevelopmentSaveEndpoint}`,filter);
    }

    
}
