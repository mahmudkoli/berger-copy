import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity/Response/api-response';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class DealeropeningService {

    public baseUrl: string;

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') baseUrl: string
    ) {

        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public GetDealerOpeningList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/DealerOpening/GetDealerOpeningList');
    }
  
   
}