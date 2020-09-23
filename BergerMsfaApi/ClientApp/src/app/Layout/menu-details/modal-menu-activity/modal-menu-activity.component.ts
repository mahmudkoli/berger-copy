import { Component, OnInit, Input } from '@angular/core';
import { MenuActivity } from '../../../Shared/Entity/Menu/menu-activity';
import { Status } from '../../../Shared/Enums/status';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MenuActivityService } from '../../../Shared/Services/Menu-Details/menu-activity.service';

@Component({
  selector: 'app-modal-menu-activity',
  templateUrl: './modal-menu-activity.component.html',
  styleUrls: ['./modal-menu-activity.component.css']
})
export class ModalMenuActivityComponent implements OnInit {

    @Input() menuActivityItem: MenuActivity;
    parentMenuList: MenuActivity[] = [];
    enumStatus = Status;
    statusValues = [];
    tosterMsgError: string = "Something went wrong";
    menuActivity: MenuActivity = new MenuActivity();

    constructor(
        public activeModal: NgbActiveModal,
        public menuActivityService: MenuActivityService
    ) { }

    ngOnInit() {
        
    }





    create() {
        this.menuActivityService.createActivity(this.menuActivityItem).subscribe(
            (res: any) => {
                console.log(res.data);
                this.activeModal.close("new menu activity created");
            },
            (err) => {
                console.log(err);
                this.showError();
            }
        );
    }

    update() {
        this.menuActivityService.updateActivity(this.menuActivityItem).subscribe(
            (res: any) => {
                console.log(res.data);
                this.activeModal.close("new menu activity updated");
            },
            (err) => {
                console.log(err);
                this.showError();
            }
        );
    }

    submit() {
        console.log(this.menuActivityItem);
        if (this.menuActivityItem.id > 0) {
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
