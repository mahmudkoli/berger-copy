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

    public getFocusDealerList() {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/focusdealer/focusdealer');
    }
    public getFocusdealerListPaging(index: number, pageSize: number, searchDate="") {
        return this.http.get<APIResponse>(this.baseUrl + `v1/focusdealer/getFocusdealerListPaging/${index}/${pageSize}?searchDate=${searchDate}`);
    }
    public getFocusDealerById(id) {
        return this.http.get<APIResponse>(this.baseUrl + 'v1/focusdealer/getFocusDealerById/' + id);
    }

    public create(model) {
        return this.http.post<APIResponse>(this.baseUrl + 'v1/focusdealer/create', model);

    }
    public update(model) {
        return this.http.put<APIResponse>(this.baseUrl + 'v1/focusdealer/update', model);
    }
    public delete(id: number) {
        return this.http.delete<any>(this.baseUrl + 'v1/focusdealer/' + id);
    }
}
