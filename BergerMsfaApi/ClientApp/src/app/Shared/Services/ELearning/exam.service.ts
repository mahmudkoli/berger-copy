import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonService } from '../Common/common.service';
import { APIResponse } from '../../Entity/Response/api-response';
import { SaveQuestion } from '../../Entity/ELearning/question';

@Injectable({
    providedIn: 'root'
})

export class ExamService {
    public baseUrl: string;
    public ExamEndpoint: string;

    constructor(
      private http: HttpClient,
      @Inject('BASE_URL') baseUrl: string,
      private commonService: CommonService) {
        this.baseUrl = baseUrl + 'api/';
        this.ExamEndpoint = this.baseUrl + 'v1/Exam';
    }

    getAllExamReport(filter?) {
      return this.http.get<APIResponse>(`${this.ExamEndpoint}/GetAllExamReport?${this.commonService.toQueryString(filter)}`);
    }
}