import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';

@Injectable({ providedIn: 'root' })

export class ReportService {
    public baseUrl: string;
    public reportsEndpoint: string;

    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
    private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
        this.reportsEndpoint = this.baseUrl + 'v1/PortalReport';
    }

    getLeadSummary(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetLeadSummary?${this.commonService.toQueryString(filter)}`);
    }

    public downloadLeadSummaryApiUrl(filter?) {
        return `${this.reportsEndpoint}/DownloadLeadSummary?${this.commonService.toQueryString(filter)}`;
    }

    getLeadGenerationDetails(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetLeadGenerationDetails?${this.commonService.toQueryString(filter)}`);
    }

    public downloadLeadGenerationDetailsApiUrl(filter?) {
        return `${this.reportsEndpoint}/DownloadLeadGenerationDetails?${this.commonService.toQueryString(filter)}`;
    }

    getLeadFollowUpDetails(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetLeadFollowUpDetails?${this.commonService.toQueryString(filter)}`);
    }

    public downloadLeadFollowUpDetailsApiUrl(filter?) {
        return `${this.reportsEndpoint}/DownloadLeadFollowUpDetails?${this.commonService.toQueryString(filter)}`;
    }

    getPainterRegistration(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetPainterRegistration?${this.commonService.toQueryString(filter)}`);
    }

    public downloadPainterRegistration(filter?) {
        return `${this.reportsEndpoint}/DownloadPainterRegistration?${this.commonService.toQueryString(filter)}`;
    }

    getDealerOpening(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetDealerOpening?${this.commonService.toQueryString(filter)}`);
    }

    public downloadDealerOpening(filter?) {
        return `${this.reportsEndpoint}/DownloadDealerOpening?${this.commonService.toQueryString(filter)}`;
    }

    //---
    getDealerCollection(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetDealerCollection?${this.commonService.toQueryString(filter)}`);
    }

    public downloadDealerCollection(filter?) {
        return `${this.reportsEndpoint}/DownloadDealerCollection?${this.commonService.toQueryString(filter)}`;
    }

    getSubDealerCollection(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetSubDealerCollection?${this.commonService.toQueryString(filter)}`);
    }

    public downloadSubDealerCollection(filter?) {
        return `${this.reportsEndpoint}/DownloadSubDealerCollection?${this.commonService.toQueryString(filter)}`;
    }

    getCustomerCollection(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetCustomerCollection?${this.commonService.toQueryString(filter)}`);
    }

    public downloadCustomerCollection(filter?) {
        return `${this.reportsEndpoint}/DownloadCustomerCollection?${this.commonService.toQueryString(filter)}`;
    }

    getDirectProjectCollection(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetProjectCollection?${this.commonService.toQueryString(filter)}`);
    }

    public downloadDirectProjectCollection(filter?) {
        return `${this.reportsEndpoint}/DownloadProjectCollection?${this.commonService.toQueryString(filter)}`;
    }

    getPainterCall(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetPaintersCall?${this.commonService.toQueryString(filter)}`);
    }

    public downloadPainterCall(filter?) {
        return `${this.reportsEndpoint}/DownloadPaintersCall?${this.commonService.toQueryString(filter)}`;
    }

}
