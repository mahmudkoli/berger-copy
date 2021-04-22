import { MapObject } from './mapObject';

export enum EnumEmployeeRole {
    Admin = 0,
    GM = 1,
    DIC = 2,
    RSM = 3,
    BIC = 4,
    AM = 5,
    TM_TO = 6,
    ZO = 7,
}

export class EnumEmployeeRoleLabel{
    public static EmployeeRoles :  MapObject[] = [
        { id : 0, label : "Admin" },
        { id : 1, label : "GM" },
        { id : 2, label : "DIC" },
        { id : 3, label : "RSM" },
        { id : 4, label : "BIC" },
        { id : 5, label : "AM" },
        { id : 6, label : "TM/TO" },
        { id : 7, label : "ZO" }
    ];
}
