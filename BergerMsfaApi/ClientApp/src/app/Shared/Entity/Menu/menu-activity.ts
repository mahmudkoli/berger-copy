import { Menu } from './menu.model';
import { MenuActivityPermission } from './menu-activity-permission.model';

export class MenuActivity {
    public id: number;
    public status: number;
    public statusText: string;
    public name: string;
    public activityCode: string;
    public menuId: number;
    public menu: Menu;
    public menuName: string;
    public menuCode: string;
    public menuActivityPermission : MenuActivityPermission;

    // constructor() {
    //     this.id = 0;
    //     this.status = 1;
    //     this.statusText = "";
    //     this.name = "";
    //     this.activityCode = "";
    //     this.menuId = 0;
    //     this.menu = new Menu();
    //     this.menuName = "";
    // }
    
}
