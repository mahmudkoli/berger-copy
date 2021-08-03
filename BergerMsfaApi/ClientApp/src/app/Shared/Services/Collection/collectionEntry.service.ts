import { Injectable, Inject } from '@angular/core';
import { APIResponse } from '../../Entity/Response';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../Common/common.service';
import { Payments } from '../../Entity/CollectionEntry/payments';

@Injectable({
    providedIn: 'root'
})
export class CollectionEntryService {

    public baseUrl: string;
    public baseUrls: string;


    constructor(private commonService: CommonService,private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/v1/collectionEntry';
        this.baseUrls = baseUrl + 'api/';

    }

    // public getCollectionByType( paymentFrom) {
    //     return this.http.get<APIResponse>(this.baseUrls + 'v1/collectionEntry/getCollectionByType/' + paymentFrom);
    // }

    public  getCollectionByType(filter?) {
        return this.http.get<APIResponse>(
          `${
            this.baseUrl
          }/getCollectionByType?${this.commonService.toQueryString(filter)}`
        );
      }

    deleteCollection(id) {
        return this.http.delete<APIResponse>(`${this.baseUrl}/DeleteCollection/${id}`);
    }

    getCollectionById(id) {
      return this.http.get<APIResponse>(`${this.baseUrl}/GetCollectionById/${id}`);
  }

  public updateCollection(model: Payments) {
    return this.http.post<APIResponse>(`${this.baseUrl}/UpdateCollection`, model);
  }
}
