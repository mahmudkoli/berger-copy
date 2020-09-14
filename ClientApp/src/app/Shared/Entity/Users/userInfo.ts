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
    // public nationalNodeId: number;
    // public regionNodeId: number;
    // public areaNodeId: number;
    // public terrNodeId: number;
    public nationalNodeIds: number[];
    public territoryNodeIds: number[];
    public areaNodeIds : number[];
    public regionNodeIds : number[];
    public email: string;
    public adGuid: string;
    public groups: string;
    public roleId: number;
    public roleIds: number[];
    public nodeId: number;
    public hierarchyId: number;


    constructor() {
        this.status = 1;
        this.nationalNodeIds = [];
        this.regionNodeIds = [];
        this.areaNodeIds = [];
        this.territoryNodeIds = [];

    }
}