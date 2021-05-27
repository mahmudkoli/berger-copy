import { QueryObject } from '../Common/query-object';

export class ReportBaseQuery extends QueryObject {
    depot: string;
    salesGroups: string[];
    territories: string[];
    zones: string[];
    userId: number;
    fromDate: Date;
    toDate: Date;

    constructor(init?: Partial<ReportBaseQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class LeadSummaryQuery extends ReportBaseQuery {

    constructor(init?: Partial<LeadSummaryQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class LeadGenerationDetailsQuery extends ReportBaseQuery {
    projectName: string;
    paintingStageId: number;

    constructor(init?: Partial<LeadGenerationDetailsQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class LeadFollowUpDetailsQuery extends ReportBaseQuery {
    projectName: string;
    projectCode: string;
    projectStatusId: number;

    constructor(init?: Partial<LeadFollowUpDetailsQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class PainterRegisterQuery extends ReportBaseQuery {
    painterMobileNo: string;
    painterId: number;
    painterType: number;

    constructor(init?: Partial<PainterRegisterQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class DealerOpeningQuery extends ReportBaseQuery {

    constructor(init?: Partial<DealerOpeningQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class CollectionReportQuery extends ReportBaseQuery {

    paymentMethodId: number;
    dealerId: number;

    constructor(init?: Partial<CollectionReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class PaintersCallReportQuery extends ReportBaseQuery {
    painterId: number;
    painterType: number;

    constructor(init?: Partial<PaintersCallReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class DealerVisitReportQuery extends ReportBaseQuery {
    dealerId: number;
    month: number;
    year: number;

    constructor(init?: Partial<DealerVisitReportQuery>) {
        super();
        Object.assign(this, init);
        this.month = new Date().getUTCMonth() + 1,
        this.year = new Date().getUTCFullYear()
    }

    clear() {
    }
}

export class DealerSalesCallReportQuery extends ReportBaseQuery {
    dealerId: number;

    constructor(init?: Partial<DealerSalesCallReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class SubDealerSalesCallReportQuery extends ReportBaseQuery {
    subDealerId: number;

    constructor(init?: Partial<SubDealerSalesCallReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class DealerIssueReportQuery extends ReportBaseQuery {
    dealerId: number;

    constructor(init?: Partial<DealerIssueReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class SubDealerIssueReportQuery extends ReportBaseQuery {
    subDealerId: number;

    constructor(init?: Partial<SubDealerIssueReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class TintingMachineReportQuery extends ReportBaseQuery {
    dealerId: number;
    month: number;
    year: number;

    constructor(init?: Partial<TintingMachineReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class ActiveSummeryReportQuery extends ReportBaseQuery {
    dealerId: number;
    month: number;
    year: number;
    activitySummary:string;
    constructor(init?: Partial<ActiveSummeryReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class OSOver90DaysTrendReportQuery extends ReportBaseQuery {
    creditControlArea: number;
    dealerId: number;
    fromMonth:any;
	fromYear:any;
	toMonth:any;
	toYear:any;

    constructor(init?: Partial<OSOver90DaysTrendReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}



export class MtsValueTargetAchivementReportQuery extends ReportBaseQuery {
    dealerId: number;
    fromMonth:any;
	fromYear:any;
	toMonth:any;
	toYear:any;

    constructor(init?: Partial<MtsValueTargetAchivementReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}


export class BillingDealerQuarterlyGrowthReportQuery extends ReportBaseQuery {
    dealerId: number;
    fromMonth:any;
	fromYear:any;
	toMonth:any;
	toYear:any;

    constructor(init?: Partial<BillingDealerQuarterlyGrowthReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}


export class EnamelPaintsQuarterlyGrowthReportQuery extends ReportBaseQuery {
    dealerId: number;
    fromMonth:any;
	fromYear:any;
	toMonth:any;
	toYear:any;

    constructor(init?: Partial<EnamelPaintsQuarterlyGrowthReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class PremiumBrandsGrowthReportQuery extends ReportBaseQuery {
    dealerId: number;
    fromMonth:any;
	fromYear:any;
	toMonth:any;
	toYear:any;

    constructor(init?: Partial<PremiumBrandsGrowthReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class PremiumBrandsContributionReportQuery extends ReportBaseQuery {
    dealerId: number;
    fromMonth:any;
	fromYear:any;
	toMonth:any;
	toYear:any;

    constructor(init?: Partial<PremiumBrandsContributionReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class SnapShotReportQuery extends ReportBaseQuery {
    dealerId: number;

    constructor(init?: Partial<SnapShotReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class LogInReportQuery extends ReportBaseQuery {
    status: number;

    constructor(init?: Partial<LogInReportQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class TerritoryTargetAchivementQuery extends ReportBaseQuery {

    constructor(init?: Partial<TerritoryTargetAchivementQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class DealerWiseTargetAchivementQuery extends ReportBaseQuery {
    customerNo: string;

    constructor(init?: Partial<DealerWiseTargetAchivementQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class ProductWiseTargetAchivementQuery extends ReportBaseQuery {
    resultType: number;

    constructor(init?: Partial<ProductWiseTargetAchivementQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class BusinessCallAnalysisReportQuery extends ReportBaseQuery {
    month: number;
    year: number;

    constructor(init?: Partial<BusinessCallAnalysisReportQuery>) {
        super();
        Object.assign(this, init);
        this.month = new Date().getUTCMonth() + 1,
        this.year = new Date().getUTCFullYear()
    }

    clear() {
    }
}

export class StrikeRateKpiReportQuery extends ReportBaseQuery {
    month: number;
    year: number;
    reportType: number;

    constructor(init?: Partial<StrikeRateKpiReportQuery>) {
        super();
        Object.assign(this, init);
        this.month = new Date().getUTCMonth() + 1,
        this.year = new Date().getUTCFullYear()
    }

    clear() {
    }
}