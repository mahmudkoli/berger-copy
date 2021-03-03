import { QueryObject } from '../Common/query-object';
import { Dropdown } from '../Setup/dropdown';
import { UserInfo } from '../Users/userInfo';

export class LeadGeneration {
    id: number;
    userId: number;
    // user: UserInfo;
    userFullName: string;
    code: string;
    depot: string;
    territory: string;
    zone: string;
    typeOfClientId: number;
    // typeOfClient: Dropdown;
    typeOfClientText: string;
    projectName: string;
    projectAddress: string;
    keyContactPersonName: string;
    keyContactPersonMobile: string;
    paintContractorName: string;
    paintContractorMobile: string;
    paintingStageId: number;
    // paintingStage: Dropdown;
    paintingStageText: string;
    visitDate: Date;
    visitDateText: string;
    expectedDateOfPainting: Date;
    expectedDateOfPaintingText: string;
    numberOfStoriedBuilding: number;
    totalPaintingAreaSqftInterior: number;
    totalPaintingAreaSqftInteriorChangeCount: number;
    totalPaintingAreaSqftExterior: number;
    totalPaintingAreaSqftExteriorChangeCount: number;
    expectedValue: number;
    expectedValueChangeCount: number;
    expectedMonthlyBusinessValue: number;
    expectedMonthlyBusinessValueChangeCount: number;
    requirementOfColorScheme: boolean;
    productSamplingRequired: boolean;
    nextFollowUpDate: Date;
    nextFollowUpDateText: string;
    remarks: string;
    photoCaptureUrl: string;

    leadFollowUps: LeadFollowUp[];

    detailsBtnText: string;

    constructor(init?: Partial<LeadGeneration>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class LeadFollowUp {
    id: number;
    leadGenerationId: number;
    //leadGeneration: LeadGeneration;
    // depot: string;
    // territory: string;
    // zone: string;
    lastVisitedDate: Date;
    lastVisitedDateText: string;
    nextVisitDatePlan: Date;
    nextVisitDatePlanText: string;
    actualVisitDate: Date;
    actualVisitDateText: string;
    typeOfClientId: number;
    //typeOfClient: Dropdown;
    typeOfClientText: string;
    keyContactPersonName: string;
    keyContactPersonNameChangeReason: string;
    keyContactPersonMobile: string;
    keyContactPersonMobileChangeReason: string;
    paintContractorName: string;
    paintContractorNameChangeReason: string;
    paintContractorMobile: string;
    paintContractorMobileChangeReason: string;
    numberOfStoriedBuilding: number;
    numberOfStoriedBuildingChangeReason: string;
    expectedValue: number;
    //expectedValueChangeCount: number;
    expectedValueChangeReason: string;
    expectedMonthlyBusinessValue: number;
    //expectedMonthlyBusinessValueChangeCount: number;
    expectedMonthlyBusinessValueChangeReason: string;
    projectStatusId: number;
    //projectStatus: Dropdown;
    projectStatusText: string;
    projectStatusLeadCompletedId: number;
    //projectStatusLeadCompleted: Dropdown;
    projectStatusLeadCompletedText: string;
    // projectStatusTotalLossId: number;
    //projectStatusTotalLoss: Dropdown;
    // projectStatusTotalLossText: string;
    projectStatusTotalLossRemarks: string;
    // projectStatusPartialBusinessId: number;
    //projectStatusPartialBusiness: Dropdown;
    // projectStatusPartialBusinessText: string;
    projectStatusPartialBusinessPercentage: number;
    swappingCompetitionId: number;
    hasSwappingCompetition: boolean;
    //swappingCompetition: Dropdown;
    swappingCompetitionText: string;
    swappingCompetitionAnotherCompetitorName: string;
    totalPaintingAreaSqftInterior: number;
    totalPaintingAreaSqftInteriorChangeCount: number;
    totalPaintingAreaSqftInteriorChangeReason: string;
    totalPaintingAreaSqftExterior: number;
    totalPaintingAreaSqftExteriorChangeCount: number;
    totalPaintingAreaSqftExteriorChangeReason: string;
    upTradingFromBrandName: string;
    upTradingToBrandName: string;
    brandUsedInteriorBrandName: string;
    brandUsedExteriorBrandName: string;
    brandUsedUnderCoatBrandName: string;
    brandUsedTopCoatBrandName: string;
    actualPaintJobCompletedInteriorPercentage: number;
    actualPaintJobCompletedExteriorPercentage: number;
    actualVolumeSoldInteriorGallon: number;
    actualVolumeSoldInteriorKg: number;
    actualVolumeSoldExteriorGallon: number;
    actualVolumeSoldExteriorKg: number;
    actualVolumeSoldUnderCoatGallon: number;
    actualVolumeSoldTopCoatGallon: number;
    businessAchievementId: number;
    businessAchievement: LeadBusinessAchievement;

    detailsBtnText: string;
    
    constructor(init?: Partial<LeadFollowUp>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class LeadBusinessAchievement {
    id: number;
    bergerValueSales: number;
    bergerPremiumBrandSalesValue: number;
    competitionValueSales: number;
    productSourcing: string;
    isColorSchemeGiven: boolean;
    isProductSampling: boolean;
    productSamplingBrandName: string;
    nextVisitDate: Date;
    nextVisitDateText: string;
    remarksOrOutcome: string;
    photoCaptureUrl: string;
    
    constructor(init?: Partial<LeadBusinessAchievement>) {
        Object.assign(this, init);
    }

    clear() {
        this.id =null;
    }
}

export class LeadQuery extends QueryObject {
    title: string;
    
    constructor(init?: Partial<LeadQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.title = '';
    }
}