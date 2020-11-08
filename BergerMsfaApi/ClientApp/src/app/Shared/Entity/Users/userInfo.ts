export class UserInfo {
    public id: number;
    public name: string;
    public designation: string;
    public phoneNumber: string;
    public code: string;
    public status: number;
    public statusText: string;
    public employeeId: string;
    public salesPointId: number;
    public nationalNodeId: number;
    public regionNodeId: number;
    public areaNodeId: number;
    public terrNodeId: number;
    public nationalNodeIds: number[];
    public territoryNodeIds: number[];
    public areaNodeIds: number[];
    public regionNodeIds: number[];
    public email: string;
    public adGuid: string;
    public groups: string;
    public roleId: number;
    public roleIds: number[];
    public nodeId: number;
    public city: string;
    public country: string;
    public company: string;
    public department: string;
    public extension: string;
    public fax: string;
    public firstName: string;
    public lastName: string;
    public loginName: string;
    public loginNameWithDomain: string;
    public manager: string;
    public managerName: string;
    public managerId: string;
    public middleName: string;
    public postalCode: string;
    public state: string;
    public streetAddress: string;
    public title: string;

    public hierarchyId: number;

    public plantIds: any[] = [];
    public areaIds: any[] = [];
    public saleOfficeIds: any[] = [];
    public territoryIds: any[] = [];
    public zoneIds: any[] = [];

    constructor() {
        this.status = 1;
        this.nationalNodeIds = [];
        this.regionNodeIds = [];
        this.areaNodeIds = [];
        this.territoryNodeIds = [];
        this.roleIds = [];
        this.plantIds = [];
        this.zoneIds = [];
        this.areaIds = [];
        this.saleOfficeIds = [];
        this.plantIds = [];

    }
}