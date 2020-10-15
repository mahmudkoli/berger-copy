import { Component, OnInit } from '@angular/core';
import { PermissionGroup, ActivityPermissionService } from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { DealeropeningService } from '../../../Shared/Services/FocusDealer/dealeropening.service';

@Component({
  selector: 'app-dealer-opening-list',
  templateUrl: './dealer-opening-list.component.html',
  styleUrls: ['./dealer-opening-list.component.css']
})
export class DealerOpeningListComponent implements OnInit {

    permissionGroup: PermissionGroup = new PermissionGroup();
    public dealerOpeningList: any[] = [];

    constructor(
        private activityPermissionService: ActivityPermissionService,
        private activatedRoute: ActivatedRoute,
        private router: Router,
        private alertService: AlertService,
        private dealeropeningService: DealeropeningService
    ) {
      //  this._initPermissionGroup();
    }

    ngOnInit() {
        this.fnDealerOpeningList();
    }
    private fnDealerOpeningList() {
        this.alertService.fnLoading(true);

        this.dealeropeningService.GetDealerOpeningList().subscribe(
            (res) => {
                this.dealerOpeningList = res.data || [];;
            },
            (error) => {
                console.log(error);
            },
            () => this.alertService.fnLoading(false)
        );
    }
    detail(id) {
        this.router.navigate(['/dealer/openingList/' + id]);
      
    }

}
