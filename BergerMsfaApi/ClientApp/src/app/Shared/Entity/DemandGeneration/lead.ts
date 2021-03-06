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
    otherClientName: string;
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
    requirementOfColorSchemeText: string;
    productSamplingRequired: boolean;
    productSamplingRequiredText: string;
    nextFollowUpDate: Date;
    nextFollowUpDateText: string;
    remarks: string;
    photoCaptureUrl: string;
    photoCaptureUrlBase64: string;


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
    otherClientName: string;
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
    projectStatusHandOverRemarks: string;
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
    // actualVolumeSoldInteriorGallon: number;
    // actualVolumeSoldInteriorKg: number;
    // actualVolumeSoldExteriorGallon: number;
    // actualVolumeSoldExteriorKg: number;
    // actualVolumeSoldUnderCoatGallon: number;
    // actualVolumeSoldTopCoatGallon: number;
    actualVolumeSoldInteriors: LeadActualVolumeSoldModel[];
    actualVolumeSoldExteriors: LeadActualVolumeSoldModel[];
    actualVolumeSoldUnderCoats: LeadActualVolumeSoldModel[];
    actualVolumeSoldTopCoats: LeadActualVolumeSoldModel[];
    businessAchievementId: number;
    businessAchievement: LeadBusinessAchievement;

    detailsBtnText: string;
    deleteBtnText: string;
    deleteBtnClass: string;
    deleteBtnIcon: string;
    
    constructor(init?: Partial<LeadFollowUp>) {
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
        this.actualVolumeSoldInteriors = [];
        this.actualVolumeSoldExteriors = [];
        this.actualVolumeSoldUnderCoats = [];
        this.actualVolumeSoldTopCoats = [];
    }
}

export class LeadBusinessAchievement {
    id: number;
    bergerValueSales: number;
    bergerPremiumBrandSalesValue: number;
    competitionValueSales: number;
    // productSourcing: string;
    productSourcingId: number;
    productSourcingText: string;
    productSourcingRemarks: string;
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

export interface LeadActualVolumeSoldModel {
    id: number;
    leadFollowUpId: number;
    brandInfoId: number;
    brandInfoText: string;
    quantity: number;
    totalAmount: number;
    actualVolumeSoldType: EnumLeadActualVolumeSoldType;
}

export enum EnumLeadActualVolumeSoldType {
    Interior = 1,
    Exterior = 2,
    UnderCoat = 3,
    TopCoat = 4
}

export class LeadQuery extends QueryObject {
    userId: number;;
    depot: string;
    territories: string[];
    zones: string[];

    constructor(init?: Partial<LeadQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.userId = 0;
        this.depot = '';
        this.territories = [];
        this.zones = [];
    }
}