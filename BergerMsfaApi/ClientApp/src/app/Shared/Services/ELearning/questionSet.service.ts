import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../Common/common.service';
import { APIResponse } from '../../Entity/Response/api-response';
import { SaveQuestionSet } from '../../Entity/ELearning/questionSet';

@Injectable({
    providedIn: 'root'
})

export class QuestionSetService {
    public baseUrl: string;
    public QuestionSetsEndpoint: string;

    constructor(
      private http: HttpClient,
      @Inject('BASE_URL') baseUrl: string,
      private commonService: CommonService) {
        this.baseUrl = baseUrl + 'api/';
        this.QuestionSetsEndpoint = this.baseUrl + 'v1/QuestionSet';
    }
  
    getQuestionSet(id) {
      return this.http.get<APIResponse>(`${this.QuestionSetsEndpoint}/${id}`);
    }
  
    getQuestionSets(filter?) {
      return this.http.get<APIResponse>(`${this.QuestionSetsEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    create(eLearning: SaveQuestionSet) {
      eLearning.id = 0;
      return this.http.post<APIResponse>(`${this.QuestionSetsEndpoint}`, eLearning);
    }
  
    update(eLearning: SaveQuestionSet) {
      return this.http.put<APIResponse>(`${this.QuestionSetsEndpoint}/${eLearning.id}`, eLearning);
    }
  
    delete(id) {
      return this.http.delete<APIResponse>(`${this.QuestionSetsEndpoint}/${id}`);
    }
  
    // activeInactive(id) {
    //   return this.http.post<APIResponse>(`${this.QuestionSetsEndpoint}/activeInactive/${id}`, null);
    // }
}