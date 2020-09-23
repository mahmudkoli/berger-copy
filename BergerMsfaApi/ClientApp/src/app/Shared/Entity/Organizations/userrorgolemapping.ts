import { OrganizationRole } from "./orgrole";
import { UserInfo } from "../Users/userInfo";

export class UserOrganizationRoleMapping {
    public roleId: number;
    public role: OrganizationRole;
    public userInfoId: number;
    public user: UserInfo;
    public userName: string;
    public userId: number;
    constructor() {

    }
}