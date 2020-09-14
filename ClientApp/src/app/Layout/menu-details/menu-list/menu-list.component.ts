import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ModalMenuComponent } from '../modal-menu/modal-menu.component';
import { Menu } from 'src/app/Shared/Entity/Menu/menu.model';
import { MenuService } from 'src/app/Shared/Services/Menu-Details/menu.service';
import { SidebarComponent } from '../../LayoutComponent/Components/sidebar/sidebar.component';
import { Role } from 'src/app/Shared/Entity/Users/role';
import { RoleService } from 'src/app/Shared/Services/Users/role.service';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
@Component({
  selector: 'app-menu-list',
  templateUrl: './menu-list.component.html',
  styleUrls: ['./menu-list.component.css']
})
export class MenuListComponent implements OnInit {
  closeResult: string;
  menuItem: Menu;
  menuList: Menu[] = [];
  tosterMsgDltSuccess: string = "Successfully Deleted";

  constructor(
      private modalService: NgbModal,
      private alertService: AlertService,
    private menuService: MenuService  ) { }

  ngOnInit() {
    this.getMenus();
  }

  getMenus() { 
    this.menuService.getAll().subscribe(
      (res: any) => {
        //console.log(res.data);
        this.menuList = res.data;        
      },
      (err: any) => {
        console.log(err);
      }
    );
  }

  onCreateNewMenu(parentMenu: Menu = null) {
    let newMenuItem = new Menu();
    if (parentMenu) {
      newMenuItem.parentId = parentMenu.id;
      newMenuItem.hasParentOnCreateNew = true;
    }
    this.openMenuModal(newMenuItem);
  }

  openMenuModal(menu: Menu = null) {
    let ngbModalOptions: NgbModalOptions = {
      backdrop: 'static',
      keyboard: false
    };
    const modalRef = this.modalService.open(ModalMenuComponent, ngbModalOptions);
    modalRef.componentInstance.menuItem = menu ? menu : new Menu();

    modalRef.result.then((result) => {
      console.log(result);
      this.closeResult = `Closed with: ${result}`;
      this.getMenus();
    },
      (reason) => {
        console.log(reason);
      });
  }

  delete(id: number) {
    this.alertService.confirm("Are you sure?",
      () => {
        this.alertService.fnLoading(true);
        this.menuService.delete(id).subscribe(
          (succ: any) => {
            console.log(succ.data);
            this.alertService.tosterSuccess(this.tosterMsgDltSuccess);
            this.getMenus();
          },
          (error) => {
            console.log(error);
            this.showError(error.message)
          },
          () => this.alertService.fnLoading(false));
      }, () => { });
  }

  showError(msg: string) {
		this.alertService.fnLoading(false);
		this.alertService.tosterDanger(msg);
	}
}
