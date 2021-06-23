import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Paginator } from 'primeng/paginator';
import {
  EnumSearchOption,
  SearchOptionDef,
  SearchOptionQuery,
  SearchOptionSettings
} from 'src/app/Shared/Modules/search-option';
import { APIModel } from '../../../Shared/Entity';
import { FocusDealer } from '../../../Shared/Entity/FocusDealer/JourneyPlan';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import {
  ActivityPermissionService,
  PermissionGroup
} from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { FocusdealerService } from '../../../Shared/Services/FocusDealer/focusdealer.service';

@Component({
  selector: 'app-focusdealer-list',
  templateUrl: './focusdealer-list.component.html',
  styleUrls: ['./focusdealer-list.component.css'],
})
export class FocusdealerListComponent implements OnInit {
  searchOptionQuery: SearchOptionQuery;

  permissionGroup: PermissionGroup = new PermissionGroup();
  public focusDealerList: FocusDealer[] = [];

  constructor(
    private activityPermissionService: ActivityPermissionService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private alertService: AlertService,
    private focusDealerService: FocusdealerService
  ) {
    this._initPermissionGroup();
  }
  first = 1;
  rows = 10;
  pagingConfig: APIModel;
  pageSize: number;
  search: string;
  filterObject: any;
  @ViewChild('paginator', { static: false }) paginator: Paginator;

  searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
    searchOptionDef: [
      new SearchOptionDef({
        searchOption: EnumSearchOption.Depot,
        isRequiredBasedOnEmployeeRole: true,
      }),
      new SearchOptionDef({
        searchOption: EnumSearchOption.Territory,
        isRequiredBasedOnEmployeeRole: false,
      }),
      new SearchOptionDef({
        searchOption: EnumSearchOption.Zone,
        isRequiredBasedOnEmployeeRole: false,
      }),
    ],
  });

  ngOnInit() {
    this.searchOptionQuery = new SearchOptionQuery();
    this.searchOptionQuery.clear();

    this.pagingConfig = new APIModel(1, 10);

    this.filterObject = {
      depoId: '',
      index: this.pagingConfig.pageNumber,
      pageSize: this.pagingConfig.pageSize,
      search: '',
      territories: [],
      custZones: [],
    };
  }

  private OnLoadFocusDealer(filterObject: any) {
    this.alertService.fnLoading(true);

    this.focusDealerService
      .getFocusdealerListPaging(filterObject)
      .subscribe(
        (res) => {
          // debugger;
          this.pagingConfig = res.data;
          // this.pageSize = Math.ceil((this.pagingConfig.totalItemCount) / this.rows);
          this.focusDealerList = (this.pagingConfig.model as []) || [];
        },
        (error) => console.log(error)
      )
      .add(() => this.alertService.fnLoading(false));
  }
  next() {
    this.pagingConfig.pageNumber =
      this.pagingConfig.pageNumber + this.pagingConfig.pageSize;

    this.OnLoadFocusDealer(this.getFilterObject());
  }

  prev() {
    this.pagingConfig.pageNumber =
      this.pagingConfig.pageNumber - this.pagingConfig.pageSize;
    this.OnLoadFocusDealer(this.getFilterObject());
  }
  onSearch() {
    this.reset();
    this.OnLoadFocusDealer(this.getFilterObject());
  }
  reset() {
    this.paginator.first = 1;
    this.pagingConfig = new APIModel(1, 10);
    this.OnLoadFocusDealer(this.getFilterObject());
  }

  isLastPage(): boolean {
    return this.focusDealerList
      ? this.first === this.focusDealerList.length - this.rows
      : true;
  }

  isFirstPage(): boolean {
    return this.focusDealerList ? this.first === 1 : true;
  }

  getFilterObject() {
    this.filterObject['index'] = this.pagingConfig.pageNumber;
    this.filterObject['pageSize'] = this.pagingConfig.pageSize;
    return this.filterObject;
  }

  paginate(event) {
    this.pagingConfig.pageNumber = Number(event.page) + 1;
    this.pagingConfig.pageSize = Number(event.rows);
    // event.first == 0 ?  1 : event.first;
    //  let first = Number(event.page) + 1;

    this.OnLoadFocusDealer(this.getFilterObject());
    //event.first = Index of the first record
    //event.rows = Number of rows to display in new page
    //event.page = Index of the new page
    //event.pageCount = Total number of pages
  }
  fnFocusDealerList() {
    this.alertService.fnLoading(true);

    this.focusDealerService.getFocusDealerList().subscribe(
      (res) => {
        this.focusDealerList = res.data || [];
      },
      (error) => {
        console.log(error);
      },
      () => this.alertService.fnLoading(false)
    );
  }
  public fnCustomTrigger(event) {
    console.log('custom  click: ', event);

    if (event.action == 'new-record') {
      this.add();
    } else if (event.action == 'edit-item') {
      this.edit(event.record.id);
    } else if (event.action == 'delete-item') {
      this.delete(event.record.id);
    }
  }
  private add() {
    this.router.navigate(['/dealer/add-focusdealer']);
  }

  private edit(id: number) {
    console.log('edit plan', id);
    this.router.navigate(['/dealer/add-focusdealer/' + id]);
  }

  private delete(id: number) {
    console.log('Id:', id);
    this.alertService.confirm(
      'Are you sure you want to delete this item?',
      () => {
        this.focusDealerService.delete(id).subscribe(
          (res: any) => {
            console.log('res from del func', res);
            this.alertService.tosterSuccess(
              'Focus dealer has been deleted successfully.'
            );
            //this.fnFocusDealerList();
            this.OnLoadFocusDealer(this.getFilterObject());
          },
          (error) => {
            console.log(error);
          }
        );
      },
      () => {}
    );
  }
  private _initPermissionGroup() {
    this.permissionGroup = this.activityPermissionService.getPermission(
      this.activatedRoute.snapshot.data.permissionGroup
    );
    console.log(this.permissionGroup);
    //this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
    //this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
    //this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;

    //this.ptableSettings.enabledRecordCreateBtn = true;
    //this.ptableSettings.enabledEditBtn = true;
    //this.ptableSettings.enabledDeleteBtn = true;
  }

  searchOptionQueryCallbackFn(queryObj: SearchOptionQuery) {
    console.log(queryObj);
    this.filterObject = {
      depoId: queryObj.depot,
      index: this.pagingConfig.pageNumber,
      pageSize: this.pagingConfig.pageSize,
      search: '',
      territories: queryObj.territories,
      custZones: queryObj.zones,
    };
    this.OnLoadFocusDealer(this.getFilterObject());

    // this.filterObj = {
    //   index: this.pagingConfig.pageNumber,
    //   pageSize: this.pagingConfig.pageSize,
    //   search: this.search,
    //   depoId: queryObj.depot,
    //   customerNo: queryObj.dealerId,
    //   territories: queryObj.territories,
    //   custZones: queryObj.zones,
    // };
    //this.OnLoadDealer(this.filterObj);
  }
}
