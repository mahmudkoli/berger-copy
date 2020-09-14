
import { User } from '..';


export class Dashboard {
    // public posmProductList : POSMProduct[];
    //#region POSM Summary data
    public  posmInstallationProductCount : number;
    public  posmRepairProductCount : number;
    public  posmRemovalProductCount : number;

    public  totalPOSMInstallationProductCount : number;
    public  totalPOSMRepairProductCount : number;
    public  totalPOSMRemovalProductCount : number;
    //#endregion

    //#region CM Status data

    //#endregion

    //#region CM Activity graph data
    // public userList : User[];
    // public dailyCMActivityCountList : number[];

    public  cmUserNameList : string[];
    public  posmInstallationProductCountListForCM : number[];
    public  posmRemovalProductCountListForCM : number[];
    public  posmRepairProductCountListForCM : number[];
    public  dailyCMActivityCount : number;
    //#endregion

    //#region Tiles data
    public totalLastMonthPOSMInstallationProductCount : number;
    public totalLastMonthPOSMRepairProductCount : number;
    public totalLastMonthPOSMRemovalProductCount : number;
    public totalLastMonthAuditReportProductCount : number;
    public totalLastMonthSurveyReportProductCount : number;
    public totalLastMonthConsumerSurveyReportProductCount : number;
    //#endregion

    constructor(){
        this.totalPOSMInstallationProductCount = 0;
        this.totalPOSMRemovalProductCount = 0;
        this.totalPOSMRepairProductCount = 0;
        this.posmInstallationProductCount = 0;
        this.posmRemovalProductCount = 0;
        this.posmRepairProductCount = 0;
        // this.dailyCMActivityCountList = [];
    
        this.posmInstallationProductCountListForCM = [];
        this.posmRemovalProductCountListForCM = [];
        this.posmRepairProductCountListForCM = [];
        this.dailyCMActivityCount = 0;
        this.totalLastMonthPOSMInstallationProductCount = 0;
        this.totalLastMonthPOSMRepairProductCount = 0;
        this.totalLastMonthPOSMRemovalProductCount = 0;
        this.totalLastMonthAuditReportProductCount = 0;
        this.totalLastMonthSurveyReportProductCount = 0;
        this.totalLastMonthConsumerSurveyReportProductCount = 0;
    }
}
