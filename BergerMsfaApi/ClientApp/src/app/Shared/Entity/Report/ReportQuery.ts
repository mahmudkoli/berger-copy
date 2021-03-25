import { EnumEmployeeRole } from '../../Enums/employee-role';
import { QueryObject } from '../Common/query-object';

export class ReportBaseQuery extends QueryObject {
    depotId: string;
    employeeRole: EnumEmployeeRole;
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

    constructor(init?: Partial<LeadFollowUpDetailsQuery>) {
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

    constructor(init?: Partial<DealerOpeningQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class PaintersCallReportQuery extends ReportBaseQuery {
    painterId: number;
    painterType: number;

    constructor(init?: Partial<LeadFollowUpDetailsQuery>) {
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

    constructor(init?: Partial<LeadFollowUpDetailsQuery>) {
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

    constructor(init?: Partial<LeadFollowUpDetailsQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class SubDealerSalesCallReportQuery extends ReportBaseQuery {
    subDealerId: number;

    constructor(init?: Partial<LeadFollowUpDetailsQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class DealerIssueReportQuery extends ReportBaseQuery {
    dealerId: number;

    constructor(init?: Partial<LeadFollowUpDetailsQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class SubDealerIssueReportQuery extends ReportBaseQuery {
    subDealerId: number;

    constructor(init?: Partial<LeadFollowUpDetailsQuery>) {
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

    constructor(init?: Partial<LeadFollowUpDetailsQuery>) {
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

    constructor(init?: Partial<LeadFollowUpDetailsQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}

export class OSOver90DaysTrendReportQuery extends ReportBaseQuery {
    creditControllAreaName: number;
    dealerId: number;

    
    constructor(init?: Partial<LeadFollowUpDetailsQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
    }
}