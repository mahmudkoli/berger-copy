import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { DealerInfoStatus } from '../../Entity/DealerInfo/DealerInfo';
import { DealerStatusExcelImportModel } from '../../Entity/DealerInfo/DealerStatusExcel';
import { SaveFocusDealer } from '../../Entity/FocusDealer/FocusDealer';
import { APIResponse } from '../../Entity/Response';
import { CommonService } from '../Common/common.service';

@Injectable({
  providedIn: 'root',
})
export class FocusDealerService {
  public baseUrl: string;
  public focusDealersEndpoint: string;
  public emailConfigEndpoint: string;
  public excelEndpoint: string;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
    private commonService: CommonService) {
      console.log('baseUrl: ', baseUrl);
      this.baseUrl = baseUrl + 'api/';
      this.focusDealersEndpoint = this.baseUrl + 'v1/FocusDealer';
      this.emailConfigEndpoint = this.baseUrl + 'v1/EmailConfig';
      this.excelEndpoint = this.baseUrl + 'v1/Excel';
  }

  //#region Focus Dealer
  getFocusDealerList(filter?) {
      return this.http.get<APIResponse>(`${this.focusDealersEndpoint}/GetFocusDealerList?${this.commonService.toQueryString(filter)}`);
  }

  public getFocusDealerById(id) {
    return this.http.get<APIResponse>(`${this.focusDealersEndpoint}/GetFocusDealerById/${id}`);
  }

  public createFocusDealer(model: SaveFocusDealer) {
    model.id = 0;
    return this.http.post<APIResponse>(`${this.focusDealersEndpoint}/CreateFocusDealer`, model);
  }

  public updateFocusDealer(model: SaveFocusDealer) {
    return this.http.put<APIResponse>(`${this.focusDealersEndpoint}/UpdateFocusDealer`, model);
  }

  public deleteFocusDealer(id) {
    return this.http.delete<any>(`${this.focusDealersEndpoint}/DeleteFocusDealer/${id}`);
  }
  //#endregion

  //#region Dealer
  public getDealerList(filter?) {
    return this.http.get<APIResponse>(`${this.focusDealersEndpoint}/GetDealerList?${this.commonService.toQueryString(filter)}`);
  }

  public updateDealerStatus(dealer: DealerInfoStatus) {
    return this.http.post<any>(`${this.focusDealersEndpoint}/UpdateDealerStatus`, dealer);
  }

  public getDealerLogByDealerId(id) {
    return this.http.get<APIResponse>(`${this.focusDealersEndpoint}/GetDealerInfoStatusLog/${id}`);
  }
//#endregion

  //#region Email Config
  public createEmailConfig(model) {
    return this.http.post<APIResponse>(`${this.emailConfigEndpoint}/CreateEmailConfig`, model);
  }

  public updateEmailConfig(dealer) {
    return this.http.put<any>(`${this.emailConfigEndpoint}/UpdateEmailConfig/`, dealer);
  }

  public getEmailConfig() {
    return this.http.get<APIResponse>(`${this.emailConfigEndpoint}/GetEmailConfig`);
  }

  public getEmailConfigById(id) {
    return this.http.get<APIResponse>(`${this.emailConfigEndpoint}/GetById/${id}`);
  }
  //#endregion

  public excelDealerStatusUpdate(model: DealerStatusExcelImportModel) {
    return this.http.post<APIResponse>(`${this.excelEndpoint}/DealerStatusUpdate`, this.commonService.toFormData(model));
  }

  public DeleteDealerOppeningEmailById(id){
    return this.http.delete<APIResponse>(`${this.emailConfigEndpoint}/DeleteById/`+ id);

  }








}
