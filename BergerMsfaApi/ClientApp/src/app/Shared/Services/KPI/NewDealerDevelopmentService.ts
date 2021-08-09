import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';

@Injectable({ providedIn: 'root' })

export class NewDealerDevelopmentService {
    public baseUrl: string;
    public NewDealerDevelopmentEndpoint: string;
    public NewDealerDevelopmentSaveEndpoint: string;
    public DealerOpeningStatusEndpoint: string;
    public DealerConversionEndpoint: string;




    constructor(private http: HttpClient,@Inject('BASE_URL') baseUrl: string,
        private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
        this.NewDealerDevelopmentEndpoint = this.baseUrl + 'v1/NewDealerDevelopment/GetNewDealerDevelopment';
        this.NewDealerDevelopmentSaveEndpoint = this.baseUrl + 'v1/NewDealerDevelopment/SaveNewDealerDevelopment';
        this.DealerOpeningStatusEndpoint = this.baseUrl + 'v1/NewDealerDevelopment/GetDealerOpeningStatus';
        this.DealerConversionEndpoint = this.baseUrl + 'v1/NewDealerDevelopment/GetDealerConversion';



    }

    
    getNewDealerDevelopment(filter?) {
        return this.http.get<APIResponse>(`${this.NewDealerDevelopmentEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    GetDealerOpeningStatus(filter) {
        return this.http.get<APIResponse>(`${this.DealerOpeningStatusEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    GetDealerConversion(filter) {
        return this.http.get<APIResponse>(`${this.DealerConversionEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    SaveOrUpdateNewDealerDevelopment(filter) {
        return this.http.post<APIResponse>(`${this.NewDealerDevelopmentSaveEndpoint}`,filter);
    }

    
}
