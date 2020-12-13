import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../Common/common.service';
import { APIResponse } from '../../Entity/Response/api-response';
import { SaveELearningDocument } from '../../Entity/ELearning/eLearningDocument';

@Injectable({
    providedIn: 'root'
})

export class ELearningService {
    public baseUrl: string;
    public ELearningsEndpoint: string;

    constructor(
      private http: HttpClient,
      @Inject('BASE_URL') baseUrl: string,
      private commonService: CommonService) {
        this.baseUrl = baseUrl + 'api/';
        this.ELearningsEndpoint = this.baseUrl + 'v1/ELearning';
    }
  
    getELearning(id) {
      return this.http.get<APIResponse>(`${this.ELearningsEndpoint}/${id}`);
    }
  
    getELearnings(filter?) {
      return this.http.get<APIResponse>(`${this.ELearningsEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    create(eLearning: SaveELearningDocument) {
      eLearning.id = 0;
      return this.http.post<APIResponse>(`${this.ELearningsEndpoint}`, this.commonService.toFormData(eLearning));
    }
  
    update(eLearning: SaveELearningDocument) {
      return this.http.put<APIResponse>(`${this.ELearningsEndpoint}/${eLearning.id}`, this.commonService.toFormData(eLearning));
    }
  
    delete(id) {
      return this.http.delete<APIResponse>(`${this.ELearningsEndpoint}/${id}`);
    }
  
    activeInactive(id) {
      return this.http.post<APIResponse>(`${this.ELearningsEndpoint}/activeInactive/${id}`, null);
    }
}