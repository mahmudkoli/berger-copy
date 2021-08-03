export class PainterCall {
    id: number;
    hasSchemeComnunaction: boolean;
    hasSchemeComnunactionText: string;

    hasPremiumProtBriefing: boolean;
    hasPremiumProtBriefingText: string;

    hasNewProBriefing: boolean;
    hasNewProBriefingText: string;

    hasUsageEftTools: boolean;
    hasUsageEftToolsText: string;

    hasAppUsage: boolean;
    hasAppUsageText: string;

    hasDbblIssue: boolean;
    hasDbblIssueText: string;

    workInHandNumber: number;
   
    comment: string;
    painterId: number;
    painterCompanyMTDValue: any[];
    viewDetailsText: string;
    viewDetailsBtnclass: string;

    
}


export class PainterStatus {
    id: number;
    status: number;
    resoan: string;

    constructor(init?: Partial<PainterStatus>) {
        Object.assign(this, init);
    }

    clear() {
    }
}