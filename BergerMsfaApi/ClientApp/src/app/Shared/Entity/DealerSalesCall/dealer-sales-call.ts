import { QueryObject } from '../Common/query-object';
import { Dropdown } from '../Setup/dropdown';
import { UserInfo } from '../Users';

export class DealerSalesCall {
    id: number;
    dealerId: number;
    // dealer: any;
    dealerName: string;
    userId: number;
    // user: UserInfo;
    userFullName: string;
    journeyPlanId: number;
    date: Date;
    dateText: string;
    isTargetPromotionCommunicated: boolean;
    isTargetCommunicated: boolean;

    secondarySalesRatingsId: number;
    // secondarySalesRatings: Dropdown;
    secondarySalesRatingsText: string;
    secondarySalesReasonTitle: string;
    secondarySalesReasonRemarks: string;

    hasOS: boolean;
    isOSCommunicated: boolean;
    hasSlippage: boolean;
    isSlippageCommunicated: boolean;

    isPremiumProductCommunicated: boolean;
    isPremiumProductLifting: boolean;
    premiumProductLiftingId: number;
    // premiumProductLifting: Dropdown;
    premiumProductLiftingText: string;
    premiumProductLiftingOthers: string;

    isCBInstalled: boolean;
    isCBProductivityCommunicated: boolean;

    merchendisingId: number;
    // merchendising: Dropdown;
    merchendisingText: string;

    hasSubDealerInfluence: boolean;
    subDealerInfluenceId: number;
    // subDealerInfluence: Dropdown;
    subDealerInfluenceText: string;

    hasPainterInfluence: boolean;
    painterInfluenceId: number;
    // painterInfluence: Dropdown;
    painterInfluenceText: string;

    isShopManProductKnowledgeDiscussed: boolean;
    isShopManSalesTechniquesDiscussed: boolean;
    isShopManMerchendizingImprovementDiscussed: boolean;

    hasCompetitionPresence: boolean;
    isCompetitionServiceBetterThanBPBL: boolean;
    competitionServiceBetterThanBPBLRemarks: string;
    isCompetitionProductDisplayBetterThanBPBL: boolean;
    competitionProductDisplayBetterThanBPBLRemarks: string;
    competitionProductDisplayImageUrl: string;
    competitionSchemeModalityComments: string;
    competitionSchemeModalityImageUrl: string;
    competitionShopBoysComments: string;
    dealerCompetitionSales: DealerCompetitionSales[];

    hasDealerSalesIssue: boolean;
    dealerSalesIssues: DealerSalesIssue[];

    dealerSatisfactionId: number;
    // dealerSatisfaction: Dropdown;
    dealerSatisfactionText: string;
    dealerSatisfactionReason: string;

    // for sub dealer
    isSubDealerCall: boolean;
    hasBPBLSales: boolean;
    bPBLAverageMonthlySales: number;
    bPBLActualMTDSales: number;

    // display text
    detailsBtnText: string;
    // isTargetPromotionCommunicatedText: string;
    // isTargetCommunicatedText: string;
    // hasOSText: string;
    // isOSCommunicatedText: string;
    // hasSlippageText: string;
    // isSlippageCommunicatedText: string;
    // isPremiumProductCommunicatedText: string;
    // isPremiumProductLiftingText: string;
    // isCBInstalledText: string;
    // isCBProductivityCommunicatedText: string;
    // hasSubDealerInfluenceText: string;
    // hasPainterInfluenceText: string;
    // isShopManProductKnowledgeDiscussedText: string;
    // isShopManSalesTechniquesDiscussedText: string;
    // isShopManMerchendizingImprovementDiscussedText: string;
    // hasCompetitionPresenceText: string;
    // isCompetitionServiceBetterThanBPBLText: string;
    // isCompetitionProductDisplayBetterThanBPBLText: string;
    // hasDealerSalesIssueText: string;
    // isSubDealerCallText: string;
    // hasBPBLSalesText: string;

    // [key: string]: any;

    constructor(init?: Partial<DealerSalesCall>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        // this.isTargetPromotionCommunicated = false;
        // this.isTargetCommunicated = false;
        // this.hasOS = false;
        // this.isOSCommunicated = false;
        // this.hasSlippage = false;
        // this.isSlippageCommunicated = false;
        // this.isPremiumProductCommunicated = false;
        // this.isPremiumProductLifting = false;
        // this.isCBInstalled = false;
        // this.isCBProductivityCommunicated = false;
        // this.hasSubDealerInfluence = false;
        // this.hasPainterInfluence = false;
        // this.isShopManProductKnowledgeDiscussed = false;
        // this.isShopManSalesTechniquesDiscussed = false;
        // this.isShopManMerchendizingImprovementDiscussed = false;
        // this.hasCompetitionPresence = false;
        // this.isCompetitionServiceBetterThanBPBL = false;
        // this.isCompetitionProductDisplayBetterThanBPBL = false;
        // this.hasDealerSalesIssue = false;
        // this.isSubDealerCall = false;
        // this.hasBPBLSales = false;
    }

    // booleanToYesNoText() {
    //     let entries = Object.entries(this) || [];
    //     console.log(entries);
    //     entries.forEach(([key, value]) => {
    //         // let value = this[key];
    //         let keyText = key+'Text';
    //         if (typeof value === "boolean") {
    //             this[keyText] = value ? 'YES' : 'NO'; 
    //         }
    //     });
    // }
}

export class DealerCompetitionSales {
    dealerSalesCallId: number;
    //dealerSalesCall: DealerSalesCall;
    companyId: number;
    // company: Dropdown;
    companyName: string;
    averageMonthlySales: number;
    actualMTDSales: number;
    
    constructor(init?: Partial<DealerCompetitionSales>) {
        Object.assign(this, init);
    }

    clear() {
    }
}

export class DealerSalesIssue {
    dealerSalesCallId: number;
    //dealerSalesCall: DealerSalesCall;
    dealerSalesIssueCategoryId: number;
    // dealerSalesIssueCategory: Dropdown;
    dealerSalesIssueCategoryText: string;
    materialName: string;
    materialGroup: string;
    quantity: number;
    batchNumber: string;
    comments: string;
    priorityId: number;
    // priority: Dropdown;
    priorityText: string;

    hasCBMachineMantainance: number;
    cBMachineMantainanceId: number;
    // cBMachineMantainance: Dropdown;
    cBMachineMantainanceText: string;
    cBMachineMantainanceRegularReason: string;

    constructor(init?: Partial<DealerSalesIssue>) {
        Object.assign(this, init);
    }

    clear() {
    }
}

export class DealerSalesCallQuery extends QueryObject {
    title: string;
    
    constructor(init?: Partial<DealerSalesCallQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.title = '';
    }
}