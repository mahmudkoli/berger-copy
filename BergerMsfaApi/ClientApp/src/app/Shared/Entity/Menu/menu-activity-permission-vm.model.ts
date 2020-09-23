import { Status } from '../../Enums/status';

export class MenuActivityPermissionVm {
    public id: number;
    public status: number;
    public roleId: number;
    public roleName: string;
    public menuId: number;
    public menuName: string;
    public activityId: number;
    public activityName: string;

    public canView: boolean;
    public canUpdate: boolean;
    public canInsert: boolean;
    public canDelete: boolean;

    constructor() {
        this.id = 0;
        this.status = Status.Active;
        this.canView = false;
        this.canUpdate = false;
        this.canInsert = false;
        this.canDelete = false;
    }
}
