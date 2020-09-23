export class Menu {
    public id: number = 0;
    public status: number = 1;
    public name: string = '';
    public controller: string = '';
    public action: string = '';
    public url: string = '';
    public iconClass: string = '';
    public parentId: number = 0;
    public isParent: boolean = false;
    public sequence: number = 0;
    public menuPermissions: [] = [];
    
    public isMapped: boolean = false;
    public hasParentOnCreateNew: boolean = false;
    public children: [] = [];
}
