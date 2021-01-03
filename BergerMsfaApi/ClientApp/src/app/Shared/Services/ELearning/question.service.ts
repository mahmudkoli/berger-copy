import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../Common/common.service';
import { APIResponse } from '../../Entity/Response/api-response';
import { SaveQuestion } from '../../Entity/ELearning/question';

@Injectable({
    providedIn: 'root'
})

export class QuestionService {
    public baseUrl: string;
    public QuestionsEndpoint: string;

    constructor(
      private http: HttpClient,
      @Inject('BASE_URL') baseUrl: string,
      private commonService: CommonService) {
        this.baseUrl = baseUrl + 'api/';
        this.QuestionsEndpoint = this.baseUrl + 'v1/Question';
    }
  
    getQuestion(id) {
      return this.http.get<APIResponse>(`${this.QuestionsEndpoint}/${id}`);
    }
  
    getQuestionsByELearningDocumentId(id) {
      return this.http.get<APIResponse>(`${this.QuestionsEndpoint}/GetByELearningDocumentId/${id}`);
    }
  
    getQuestions(filter?) {
      return this.http.get<APIResponse>(`${this.QuestionsEndpoint}?${this.commonService.toQueryString(filter)}`);
    }

    create(eLearning: SaveQuestion) {
      eLearning.id = 0;
      return this.http.post<APIResponse>(`${this.QuestionsEndpoint}`, eLearning);
    }
  
    update(eLearning: SaveQuestion) {
      return this.http.put<APIResponse>(`${this.QuestionsEndpoint}/${eLearning.id}`, eLearning);
    }
  
    delete(id) {
      return this.http.delete<APIResponse>(`${this.QuestionsEndpoint}/${id}`);
    }
  
    // activeInactive(id) {
    //   return this.http.post<APIResponse>(`${this.QuestionsEndpoint}/activeInactive/${id}`, null);
    // }
}