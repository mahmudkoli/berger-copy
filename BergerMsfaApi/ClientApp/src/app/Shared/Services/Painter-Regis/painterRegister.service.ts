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

    public GetPainterList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/AppPainterRegis/GetPainterList');
    }
    public GetRegisterPainterById(id) {

        return this.http.get<APIResponse>(this.baseUrl + 'v1/AppPainterRegis/GetPainterById/' + id);
    }
   
}
