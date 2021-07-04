import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { ActivityPermissionService, PermissionGroup } from 'src/app/Shared/Services/Activity-Permission/activity-permission.service';
import { FocusDealerService } from 'src/app/Shared/Services/FocusDealer/focus-dealer.service';
import { EmailConfigForDealerOpening } from '../../../Shared/Entity/DealerOpening/EmailConfig';

@Component({
  selector: 'app-email-config-list',
  templateUrl: './email-config-list.component.html',
  styleUrls: ['./email-config-list.component.css']
})
export class EmailConfigListComponent implements OnInit {

  permissionGroup: PermissionGroup = new PermissionGroup();
  public emailconfigList: EmailConfigForDealerOpening[] = [];

  constructor(
      private activityPermissionService: ActivityPermissionService,
      private activatedRoute: ActivatedRoute,
      private router: Router,
      private alertService: AlertService,
      private focusDealerService: FocusDealerService
  ) {
      this._initPermissionGroup();
  }

  ngOnInit() {
     
      this.OnLoadEmailConfig();

  }

  private OnLoadEmailConfig() {

      this.alertService.fnLoading(true);

      this.focusDealerService.getEmailConfig()
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

      this.focusDealerService.getEmailConfig().subscribe(
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
      this.router.navigate(['/dealer/addEmail']);
  }

  private edit(id: number) {
      console.log('edit email config', id);
      this.router.navigate(['/dealer/addEmail/' + id]);
  }

  private delete(id: number) {
      console.log("Id:", id);
      this.alertService.confirm("Are you sure you want to delete this item?", () => {
          this.focusDealerService.deleteFocusDealer(id).subscribe(
              (res: any) => {
                  console.log('res from del func', res);
                  this.alertService.tosterSuccess("Dropdown has been deleted successfully.");
                  //this.fnFocusDealerList();
                  this.OnLoadEmailConfig();
              },
              (error) => {
                  console.log(error);
              }
          );
      }, () => {

      });
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
