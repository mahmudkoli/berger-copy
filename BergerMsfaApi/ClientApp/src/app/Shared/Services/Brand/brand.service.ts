import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../Common/common.service';
import { APIResponse } from '../../Entity/Response/api-response';
import { BrandStatus } from '../../Entity/Brand/brand';

@Injectable({
    providedIn: 'root'
})

export class BrandService {
    public baseUrl: string;
    public BrandsEndpoint: string;

    constructor(
      private http: HttpClient,
      @Inject('BASE_URL') baseUrl: string,
      private commonService: CommonService) {
        this.baseUrl = baseUrl + 'api/';
        this.BrandsEndpoint = this.baseUrl + 'v1/Brand';
    }
  
    getBrand(id) {
      return this.http.get<APIResponse>(`${this.BrandsEndpoint}/${id}`);
    }
  
    getBrands(filter?) {
      return this.http.get<APIResponse>(`${this.BrandsEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    updateBrandStatus(brand: BrandStatus) {
      return this.http.post<APIResponse>(`${this.BrandsEndpoint}`, brand);
    }
  
    // activeInactive(id) {
    //   return this.http.post<APIResponse>(`${this.BrandsEndpoint}/activeInactive/${id}`, null);
    // }
}