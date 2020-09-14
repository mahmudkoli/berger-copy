import { Role } from "./role";
import { UserInfo } from "./userInfo";

export class UserRoleMapping {
    public roleId: number;
    public role: Role;
    public userInfoId: number;
    public user: UserInfo;
    public userIds: number[];
    constructor() {

    }
}