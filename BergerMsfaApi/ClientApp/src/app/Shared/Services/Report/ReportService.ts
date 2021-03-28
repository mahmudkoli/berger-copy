import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { APIResponse } from '../../Entity';
import { CommonService } from '../Common/common.service';

@Injectable({ providedIn: 'root' })

export class ReportService {
    public baseUrl: string;
    public reportsEndpoint: string;
    public quaterlyReportEndpoint: string;


    constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string,
    private commonService: CommonService) {
        console.log("baseUrl: ", baseUrl);
        this.baseUrl = baseUrl + 'api/';
        this.reportsEndpoint = this.baseUrl + 'v1/PortalReport';
        this.quaterlyReportEndpoint = this.baseUrl + 'v1/PortalQuartPerformReport';

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

    getDealerVisit(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetDealerVisit?${this.commonService.toQueryString(filter)}`);
    }

    public downloadDealerVisit(filter?) {
        return `${this.reportsEndpoint}/DownloadDealerVisit?${this.commonService.toQueryString(filter)}`;
    }

    getDealerSalesCall(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetDealerSalesCall?${this.commonService.toQueryString(filter)}`);
    }

    public downloadDealerSalesCall(filter?) {
        return `${this.reportsEndpoint}/DownloadDealerSalesCall?${this.commonService.toQueryString(filter)}`;
    }

    getSubDealerSalesCall(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetSubDealerSalesCall?${this.commonService.toQueryString(filter)}`);
    }

    public downloadSubDealerSalesCall(filter?) {
        return `${this.reportsEndpoint}/DownloadSubDealerSalesCall?${this.commonService.toQueryString(filter)}`;
    }

    getDealerIssue(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetDealerIssue?${this.commonService.toQueryString(filter)}`);
    }

    public downloadDealerIssue(filter?) {
        return `${this.reportsEndpoint}/DownloadDealerIssue?${this.commonService.toQueryString(filter)}`;
    }

    getSubDealerIssue(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetSubDealerIssue?${this.commonService.toQueryString(filter)}`);
    }

    public downloadSubDealerIssue(filter?) {
        return `${this.reportsEndpoint}/DownloadSubDealerIssue?${this.commonService.toQueryString(filter)}`;
    }


    public getTintingMachine(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetTintingMachine?${this.commonService.toQueryString(filter)}`);
    }

    public downloadTintingMachine(filter?) {
        return `${this.reportsEndpoint}/DownloadTintingMachine?${this.commonService.toQueryString(filter)}`;
    }

    public getActiveSummery(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetActiveSummery?${this.commonService.toQueryString(filter)}`);
    }

    public downloadActiveSummery(filter?) {
        return `${this.reportsEndpoint}/DownloadActiveSummery?${this.commonService.toQueryString(filter)}`;
    }

    public getOsOver90DaysTrend(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/OsOver90daysTrendReport?${this.commonService.toQueryString(filter)}`);
    }

    public downloadOsOver90DaysTrend(filter?) {
        return `${this.reportsEndpoint}/DownloadOsOver90daysTrendReport?${this.commonService.toQueryString(filter)}`;
    }


    public getMtsValueTargetAchivement(filter?) {
        return this.http.get<APIResponse>(`${this.quaterlyReportEndpoint}/GetMTSValueTargetAchivement?${this.commonService.toQueryString(filter)}`);
    }

    public downloadMtsValueTargetAchivement(filter?) {
        return `${this.quaterlyReportEndpoint}/DownloadMTSValueTargetAchivement?${this.commonService.toQueryString(filter)}`;
    }


    public getBillingDealerQuarterlyGrowth(filter?) {
        return this.http.get<APIResponse>(`${this.quaterlyReportEndpoint}/GetBillingDealerQuarterlyGrowth?${this.commonService.toQueryString(filter)}`);
    }

    public downloadBillingDealerQuarterlyGrowth(filter?) {
        return `${this.quaterlyReportEndpoint}/DownloadBillingDealerQuarterlyGrowth?${this.commonService.toQueryString(filter)}`;
    }

    public getEnamelPaintsQuarterlyGrowth(filter?) {
        return this.http.get<APIResponse>(`${this.quaterlyReportEndpoint}/GetEnamelPaintsQuarterlyGrowth?${this.commonService.toQueryString(filter)}`);
    }

    public downloadEnamelPaintsQuarterlyGrowth(filter?) {
        return `${this.quaterlyReportEndpoint}/DownloadEnamelPaintsQuarterlyGrowth?${this.commonService.toQueryString(filter)}`;
    }


    public getPremiumBrandsGrowth(filter?) {
        return this.http.get<APIResponse>(`${this.quaterlyReportEndpoint}/GetPremiumBrandsGrowth?${this.commonService.toQueryString(filter)}`);
    }

    public downloadPremiumBrandsGrowth(filter?) {
        return `${this.quaterlyReportEndpoint}/DownloadPremiumBrandsGrowth?${this.commonService.toQueryString(filter)}`;
    }



    public getPremiumBrandsContribution(filter?) {
        return this.http.get<APIResponse>(`${this.reportsEndpoint}/GetPremiumBrandsContribution?${this.commonService.toQueryString(filter)}`);
    }

    public downloadPremiumBrandsContribution(filter?) {
        return `${this.reportsEndpoint}/DownloadPremiumBrandsContribution?${this.commonService.toQueryString(filter)}`;
    }

}
