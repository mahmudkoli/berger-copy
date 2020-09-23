import { Status } from "../../Enums/status";
import { UserOrganizationRoleMapping } from './userrorgolemapping';
import { UserInfo } from '../Users/userInfo';

export class OrganizationRole {
    public id: number;
    public name: string;
    public status: number;
    public checked: boolean;
    public designationId: number;
    public userList: number[];
  //  public userInfoId: number;
  //  public user: UserInfo;
    public configList: UserOrganizationRoleMapping[];
    constructor() {
        this.id = 0;
        this.name = '';
        this.status = Status.Active;
        this.configList = [];
    }
}