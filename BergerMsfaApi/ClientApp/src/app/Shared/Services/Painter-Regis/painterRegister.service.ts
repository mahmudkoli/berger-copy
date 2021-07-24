import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity/Response';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class PainterRegisService {

    public baseUrl: string;

    constructor(
        private http: HttpClient,
        @Inject('BASE_URL') baseUrl: string
    ) {

        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
    }

    public GetPainterList(index,pageSize,search) {
        return this.http.get<APIResponse>(this.baseUrl + `v1/PainterRegis/GetPainterList/${index}/${pageSize}?search=${search}`);
    }
    public GetRegisterPainterById(id) {

        return this.http.get<APIResponse>(this.baseUrl + 'v1/PainterRegis/GetPainterById/' + id);
    }
    public UpdatePainterStatus(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/PainterRegis/UpdatePainterStatus', model);
    }
   
}
