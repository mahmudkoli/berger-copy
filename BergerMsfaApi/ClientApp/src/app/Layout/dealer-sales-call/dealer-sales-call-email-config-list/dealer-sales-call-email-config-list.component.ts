import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EmailConfigForDealerSalesCall } from 'src/app/Shared/Entity/DealerSalesCall/EmailConfigForDealerSalesCall';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { DealerSalesCallService } from 'src/app/Shared/Services/DealerSalesCall/dealer-sales-call.service';
import { FocusDealerService } from 'src/app/Shared/Services/FocusDealer/focus-dealer.service';

@Component({
  selector: 'app-email-config-list',
  templateUrl: './dealer-sales-call-email-config-list.component.html',
  styleUrls: ['./dealer-sales-call-email-config-list.component.css']
})
export class DealerSalesCallEmailConfigListComponent implements OnInit {

  permissionGroup: PermissionGroup = new PermissionGroup();
  public emailconfigList: EmailConfigForDealerSalesCall[] = [];

  constructor(
      private activityPermissionService: ActivityPermissionService,
      private activatedRoute: ActivatedRoute,
      private router: Router,
      private alertService: AlertService,
      private dealerSalesCallService: DealerSalesCallService
  ) {
      this._initPermissionGroup();
  }

  ngOnInit() {
     
      this.OnLoadEmailConfig();

  }

  private OnLoadEmailConfig() {

      this.alertService.fnLoading(true);

      this.dealerSalesCallService.getEmailConfig()
          .subscribe(
              (res) => {
                  // debugger;
                  this.emailconfigList = res.data;
                 // this.pageSize = Math.ceil((this.pagingConfig.totalItemCount) / this.rows);
              },
              (error) => console.log(error)

          ).add(() => this.alertService.fnLoading(false));
  }

  fnFocusDealerList() {
      this.alertService.fnLoading(true);

      this.dealerSalesCallService.getEmailConfig().subscribe(
          (res) => {
              this.emailconfigList = res.data || [];;
          },
          (error) => {
              console.log(error);
          },
          () => this.alertService.fnLoading(false)
      );
  }
  public fnCustomTrigger(event) {
      console.log("custom  click: ", event);

      if (event.action == "new-record") {
          this.add();
      }
      else if (event.action == "edit-item") {
          this.edit(event.record.id);
      }
      else if (event.action == "delete-item") {
          this.delete(event.record.id);
      }
  } private add() {
      this.router.navigate(['/dealer-sales-call/addemailconfig']);
  }

  private edit(id: number) {
      console.log('edit email config', id);
      this.router.navigate(['/dealer-sales-call/addemailconfig/' + id]);
  }

  private delete(id: number) {
    //   console.log("Id:", id);
    //   this.alertService.confirm("Are you sure you want to delete this item?", () => {
    //       this.focusDealerService.delete(id).subscribe(
    //           (res: any) => {
    //               console.log('res from del func', res);
    //               this.alertService.tosterSuccess("dropdown has been deleted successfully.");
    //               //this.fnFocusDealerList();
    //               this.OnLoadEmailConfig();
    //           },
    //           (error) => {
    //               console.log(error);
    //           }
    //       );
    //   }, () => {

    //   });
  }
  private _initPermissionGroup() {

      this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
      console.log(this.permissionGroup);
      //this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
      //this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
      //this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;

      //this.ptableSettings.enabledRecordCreateBtn = true;
      //this.ptableSettings.enabledEditBtn = true;
      //this.ptableSettings.enabledDeleteBtn = true;


  }

}
