import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity/Response/api-response';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../Common/common.service';

@Injectable({
    providedIn: 'root'
})
export class DealeropeningService {

    public baseUrl: string;

    constructor(
        private http: HttpClient,
        private commonService: CommonService,
        @Inject('BASE_URL') baseUrl: string
    ) {

        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public GetDealerOpeningList(filter) {
        return this.http.get<APIResponse>(this.baseUrl + `v1/DealerOpening/GetDealerOpeningList?/${this.commonService.toQueryString(filter)}`);
    }

    public GetDealerOpeningDetailById(id) {
        return this.http.get<APIResponse>(this.baseUrl + `v1/DealerOpening/GetDealerOpeningDetailById/${id}`);
    }
    public DealerOpeningStatusChange(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/DealerOpening/ChangeDealerOpeningStatus/', model);
    }

}
