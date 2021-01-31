// export interface IAuthUser {
//     id: string;
//     fullName: string;
//     userName: string;
//     email: string;
//     phoneNumber: string;
//     imageUrl: string;
//     token?: string;
// }

export interface IAuthUser {
    userId: number;
    fullName: string;
    planIds: string[];
    planId: string;
    salesOfficeIds: string[];
    salesOfficeId: string;
    areaIds: string[];
    areaId: string;
    territoryIds: string[];
    territoryId: string;
    zoneIds: string[];
    zoneId: string;
    roleId: number;
    roleName: string;
    employeeId: string;
    userCategory: string;
    userCategoryIds: string[];
    token: string;
    expiration: Date;
}

export class Login {
    userName: string;
    password: string;
    rememberMe: boolean;
    
    constructor(init?: Partial<Login>) {
        Object.assign(this, init);
    }

    clear() {
        this.userName = '';
        this.password = '';
        this.rememberMe = false;
    }
}

export class ResetPassword {
    userId: string;
    newPassword: string;
    token: string;
    
    constructor(init?: Partial<ResetPassword>) {
        Object.assign(this, init);
    }

    clear() {
        this.userId = '';
        this.newPassword = '';
        this.token = '';
    }
}