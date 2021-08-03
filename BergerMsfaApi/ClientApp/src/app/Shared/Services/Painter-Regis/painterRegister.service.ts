import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity/Response';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../Common/common.service';

@Injectable({
  providedIn: 'root'
})
export class PainterRegisService {

    public baseUrl: string;

    constructor(
        private http: HttpClient,
        private commonService: CommonService,
        @Inject('BASE_URL') baseUrl: string
    ) {

        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public GetPainterList(index,pageSize,search) {
        return this.http.get<APIResponse>(this.baseUrl + `v1/PainterRegis/GetPainterList/${index}/${pageSize}?search=${search}`);
    }

    public GetPainterLists(query) {
        return this.http.get<APIResponse>(this.baseUrl + `v1/PainterRegis/GetPainterList?${this.commonService.toQueryString(query)}`);
    }
    public GetRegisterPainterById(id) {

        return this.http.get<APIResponse>(this.baseUrl + 'v1/PainterRegis/GetPainterById/' + id);
    }
    public UpdatePainterStatus(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/PainterRegis/UpdatePainterStatus', model);
    }
   
}
