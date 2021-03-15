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