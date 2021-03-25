import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity/Response';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class FocusdealerService {

    public baseUrl: string;

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') baseUrl: string
    ) {

        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }
 
    public getDealerList(index: number, pageSize: number, search: string) {
        return this.http.get<APIResponse>(this.baseUrl + `v1/focusdealer/getDealerList?index=${index}&pageSize=${pageSize}&search=${search}`);
    }

    public getFocusDealerList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/focusdealer/focusdealer');
    }
    public getFocusdealerListPaging(index: number, pageSize: number, search="") {
        return this.http.get<APIResponse>(this.baseUrl + `v1/focusdealer/getFocusdealerListPaging/${index}/${pageSize}?search=${search}`);
    }
    public getFocusDealerById(id) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/focusdealer/getFocusDealerById/' + id);
    }

    public create(model) {
        // debugger;
        return this.http.post<APIResponse>(this.baseUrl + 'v1/focusdealer/create', model);

    }
    public update(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/focusdealer/update', model);
    }
    public delete(id: number) {
        return this.http.delete<any>(this.baseUrl + 'v1/focusdealer/' + id);
    }
    public updateDealerStatus(dealer) {
        return this.http.put<any>(this.baseUrl + `v1/focusdealer/UpdateDealerStatus/`,dealer);
    }
    public getDealerLogByDealerId(id) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/focusdealer/GetDealerInfoStatusLog/' + id);
    }

  
}
