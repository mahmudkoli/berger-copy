import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { DealerStatusExcelImportModel } from '../../Entity/DealerInfo/DealerStatusExcel';
import { APIResponse } from '../../Entity/Response';
import { CommonService } from '../Common/common.service';

@Injectable({
  providedIn: 'root',
})
export class FocusdealerService {
  public baseUrl: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private commonService: CommonService) {
    console.log('baseUrl: ', baseUrl);
    this.baseUrl = baseUrl + 'api/';
  }

  public getDealerList(filerObject: any) {
    return this.http.post<APIResponse>(
      this.baseUrl + `v1/focusdealer/getDealerList`,
      filerObject
    );
  }

  public getFocusDealerList() {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/focusdealer/focusdealer'
    );
  }
  public getFocusdealerListPaging(filterObject: any) {
    return this.http.post<APIResponse>(
      this.baseUrl + `v1/focusdealer/getFocusdealerListPaging`,
      filterObject
    );
  }
  public getFocusDealerById(id) {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/focusdealer/getFocusDealerById/' + id
    );
  }

  public create(model) {
    // debugger;
    return this.http.post<APIResponse>(
      this.baseUrl + 'v1/focusdealer/create',
      model
    );
  }
  public update(model) {
    return this.http.put<APIResponse>(
      this.baseUrl + 'v1/focusdealer/update',
      model
    );
  }
  public delete(id: number) {
    return this.http.delete<any>(this.baseUrl + 'v1/focusdealer/' + id);
  }
  public updateDealerStatus(dealer) {
    return this.http.put<any>(
      this.baseUrl + `v1/focusdealer/UpdateDealerStatus/`,
      dealer
    );
  }

  public getDealerLogByDealerId(id) {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/focusdealer/GetDealerInfoStatusLog/' + id
    );
  }

  public createEmailConfig(model) {
    // debugger;
    return this.http.post<APIResponse>(
      this.baseUrl + 'v1/EmailConfig/CreateEmailConfig',
      model
    );
  }

  public updateEmailConfig(dealer) {
    return this.http.put<any>(
      this.baseUrl + `v1/EmailConfig/UpdateEmailConfig/`,
      dealer
    );
  }

  public getEmailConfig() {
    return this.http.get<APIResponse>(
      this.baseUrl + `v1/EmailConfig/GetEmailConfig`
    );
  }

  public getEmailConfigById(id) {
    return this.http.get<APIResponse>(
      this.baseUrl + 'v1/EmailConfig/GetById/' + id
    );
  }

  public excelDealerStatusUpdate(model: DealerStatusExcelImportModel) {
    return this.http.post<APIResponse>(
      this.baseUrl + 'v1/Excel/DealerStatusUpdate', this.commonService.toFormData(model)
    );
  }
}
