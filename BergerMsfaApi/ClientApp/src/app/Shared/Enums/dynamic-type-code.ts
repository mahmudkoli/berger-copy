import { MapObject } from "./mapObject";

export enum EnumDynamicTypeCode {
    Dealer = "D01",
    Employee = "E01",
    Company = "C01",
    Payment = "P01",
    Ratings = "R01",
    ProductLifting = "PL01",
    Merchendising = "M01",
    SubDealerInfluence = "SDI01",
    PainterInfluence = "PI01",
    Priority = "P02",
    Satisfaction = "S01",
    TypeOfClient = "TOC01",
    PaintingStage = "PS01",
    ProjectStatus = "PS02",
    ProjectStatusLeadCompleted = "PSLC01",
    SwappingCompetition = "SC01",
    Painter = "Painter01",
    ISSUES_01 = "ISSUES01",
    ISSUES_02 = "ISSUES01",
    ISSUES_03 = "ISSUES01",
    ISSUES_04 = "ISSUES01",
    CBMachineMantainance = "CBMM01",
    Customer = "Customer01",
    ELearningCategory = "ELC01",
    ProductSourcing = "PS03",
}


export class EnumDealerSalesCallIssueCategory{
    public static IssueCategory :  MapObject[] = [
        { id : 1, label : "ISSUES01" },
        { id : 2, label : "ISSUES02" },
        { id : 3, label : "ISSUES03" },
        { id : 4, label : "ISSUES04" },
        { id : 5, label : "ISSUES05" }
    ];
}

