import { MapObject } from './mapObject';

export enum EnumEmployeeRole {
    Admin = 0,
    DIC = 1,
    BIC = 2,
    AM = 3,
    TM_TO = 4,
    ZO = 5,
}

export class EnumEmployeeRoleLabel{
    public static EmployeeRoles :  MapObject[] = [
        { id : 0, label : "Admin" },
        { id : 1, label : "DIC" },
        { id : 2, label : "BIC" },
        { id : 3, label : "AM" },
        { id : 4, label : "TM/TO" },
        { id : 5, label : "ZO" }
    ];
}
