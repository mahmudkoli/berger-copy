import { MapObject } from './mapObject';

export enum EnumEmployeeRole {
    Admin = 0,
    GM = 1,
    DSM = 2,
    RSM = 3,
    BM_BSI = 4,
    AM = 5,
    TM_TO = 6,
    ZO = 7,
}

export class EnumEmployeeRoleLabel{
    public static EmployeeRoles :  MapObject[] = [
        { id : 0, label : "Admin" },
        { id : 1, label : "GM" },
        { id : 2, label : "DSM" },
        { id : 3, label : "RSM" },
        { id : 4, label : "BM/BSI" },
        { id : 5, label : "AM" },
        { id : 6, label : "TM/TO" },
        { id : 7, label : "ZO" }
    ];
}

export class EnumTypeLabel{
    public static EnumTypes :  MapObject[] = [
        { id : 0, label : "Web Portal Menu Permission" },
        { id : 1, label : "Mobile App Menu Permission" },
        { id : 2, label : "Mobile App Alert Permission" },
    ];
}

export enum EnumType {
    WebPortal = 0,
    MobileApp = 1,
    Alert = 2
}
