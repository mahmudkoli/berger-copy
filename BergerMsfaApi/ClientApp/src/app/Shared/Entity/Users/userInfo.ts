import { QueryObject } from "../Common/query-object";

export class UserInfo {
    public id: number;
    public fullName: string;
    public userName: string;
    public email: string;
    public phoneNumber: string;
    public code: string;
    public employeeId: string;
    public designation: string;
    public department: string;
    public managerName: string;
    public managerId: string;

    public gender: string;
    public address: string;
    public dateOfBirth?: Date;
    // public imageUrl: string;

    public status: number;
    public roleId: number;
    public roleName: string;

    public roleIds: number[];
    public plantIds: any[] = [];
    public areaIds: string[] = [];
    public saleOfficeIds: string[] = [];
    public territoryIds: string[] = [];
    public zoneIds: string[] = [];

    public statusText: string;

    constructor(init?: Partial<UserInfo>) {
        this.roleIds = [];
        this.plantIds = [];
        this.zoneIds = [];
        this.areaIds = [];
        this.saleOfficeIds = [];
        this.plantIds = [];
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class SaveUserInfo {
    public id: number;
    public fullName: string;
    public userName: string;
    public email: string;
    public phoneNumber: string;
    public code: string;
    public employeeId: string;
    public designation: string;
    public department: string;
    public managerName: string;
    public managerId: string;

    public gender: string;
    public address: string;
    public dateOfBirth?: Date;
    // public imageUrl: string;

    public status: number;
    public roleId: number;
    public roleName: string;

    public roleIds: number[];
    public plantIds: any[] = [];
    public areaIds: string[] = [];
    public saleOfficeIds: string[] = [];
    public territoryIds: string[] = [];
    public zoneIds: string[] = [];

    public statusText: string;

    constructor(init?: Partial<UserInfo>) {
        this.roleIds = [];
        this.plantIds = [];
        this.zoneIds = [];
        this.areaIds = [];
        this.saleOfficeIds = [];
        this.plantIds = [];
        Object.assign(this, init);
    }

    clear() {
        this.id = null;
    }
}

export class UserInfoQuery extends QueryObject {
    title: string;
    
    constructor(init?: Partial<UserInfoQuery>) {
        super();
        Object.assign(this, init);
    }

    clear() {
        this.title = '';
    }
}