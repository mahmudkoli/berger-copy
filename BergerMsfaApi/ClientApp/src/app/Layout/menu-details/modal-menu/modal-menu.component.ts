import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Menu } from 'src/app/Shared/Entity/Menu/menu.model';
import { Status } from 'src/app/Shared/Enums/status';
import { MenuService } from 'src/app/Shared/Services/Menu-Details/menu.service';
import { EnumType, EnumTypeLabel } from 'src/app/Shared/Enums/employee-role';


@Component({
  selector: 'app-modal-menu',
  templateUrl: './modal-menu.component.html',
  styleUrls: ['./modal-menu.component.css']
})
export class ModalMenuComponent implements OnInit {

  @Input() menuItem: Menu;
  @Input() type: number;
  @Input() isEditMode: boolean;


  parentMenuList: Menu[] = [];
  enumStatus = Status;
  statusValues = [];
  tosterMsgError: string = "Something went wrong";

  constructor(
    public activeModal: NgbActiveModal,
    public menuService: MenuService
  ) { }

  ngOnInit() {
    console.log(this.menuItem,"menu");
    this.statusValues = Object.keys(this.enumStatus).filter(e => !isNaN(Number(e)));
    this.getActiveMenus();
  }

  getActiveMenus() {
    this.menuService.getAllActive().subscribe(
      (res: any) => {
        this.parentMenuList = res.data;
        console.log(res.data);
      },
      (error) => {
        console.log(error);
        this.showError();
      });

  }

  create() {
    this.menuService.create(this.menuItem).subscribe(
      (res: any) => {
        console.log(res.data);
        this.activeModal.close("new menu created");
      },
      (err) => {
        console.log(err);
        this.showError();
      }
    );
  }

  update() {
    this.menuService.update(this.menuItem).subscribe(
      (res: any) => {
        console.log(res.data);
        this.activeModal.close("new menu updated");
      },
      (err) => {
        console.log(err);
        this.showError();
      }
    );
  }

  submit() {
    this.menuItem.type=this.type;
    if (this.menuItem.id > 0) {
      this.update();
    }
    else {
      this.create();
    }
  }

  showError(msg: string = null) {
    this.activeModal.close(msg ? msg : this.tosterMsgError);
  }
}
