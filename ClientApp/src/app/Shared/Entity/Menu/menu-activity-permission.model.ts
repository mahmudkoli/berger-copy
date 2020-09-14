export class MenuActivityPermission {
    public id: number;
    public status: number;
    public roleId: number;
    public activityId: number;
    public canView: boolean=false;
    public canUpdate: boolean=false;
    public canInsert: boolean=false;
    public canDelete: boolean=false;
}