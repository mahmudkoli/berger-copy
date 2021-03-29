import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import {
  ActivityPermissionService,
  PermissionGroup,
} from '../../../Shared/Services/Activity-Permission/activity-permission.service';
import { CollectionEntryService } from '../../../Shared/Services/Collection/collectionentry.service';

export enum CustomerTypeEnum {
  Dealer = 1,
  SubDealer = 2,
  Project = 3,
  Customer = 4,
}
@Component({
  selector: 'app-collection-entry-list',
  templateUrl: './collection-entry-list.component.html',
  styleUrls: ['./collection-entry-list.component.css'],
})
export class CollectionEntryListComponent implements OnInit {
  permissionGroup: PermissionGroup = new PermissionGroup();
  //   customerTypeList: { key: number; type: string }[] = [
  //     { key: 1, type: 'Dealer' },
  //     { key: 2, type: 'Sub Dealer' },
  //     { key: 3, type: 'Project' },
  //     { key: 4, type: 'Customer' },
  //   ];
  customerTypeList: { key: number; type: string }[] = [];

  ptableSettings: any = null;
  gridDataSource: any[] = [];
  selectedType: number;

  constructor(
    private collectionEntryService: CollectionEntryService,
    private alertService: AlertService,
    private activityPermissionService: ActivityPermissionService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private dynamicDropdownService: DynamicDropdownService
  ) {
    this._initPermissionGroup();
  }

  ngOnInit() {
    this.loadDynamicDropdown();
    // this.selectedType = CustomerTypeEnum.Dealer;
  }
  private loadDynamicDropdown() {
    this.dynamicDropdownService
      .GetDropdownByTypeCd(EnumDynamicTypeCode.Customer)
      .subscribe((res) => {
        let data = res.data as any[];
        this.customerTypeList = data.map((x) => ({
          key: x.id,
          type: x.dropdownName,
        }));

        this.selectedType = this.customerTypeList[0].key;
        this.onChange(this.customerTypeList[0].key);
      });
  }

  private _initPermissionGroup() {
    //this.permissionGroup = this.activityPermissionService.getPermission(this.activatedRoute.snapshot.data.permissionGroup);
    //console.log(this.permissionGroup);
    //this.ptableSettings.enabledRecordCreateBtn = this.permissionGroup.canCreate;
    //this.ptableSettings.enabledEditBtn = this.permissionGroup.canUpdate;
    //this.ptableSettings.enabledDeleteBtn = this.permissionGroup.canDelete;
  }

  onChange(selected: number) {
    this.customerWiseTableConfig(selected);
  }

  private getCustomerDetails(type: any) {
    this.collectionEntryService.getCollectionByType(type).subscribe((res) => {
      this.gridDataSource = res.data;
    });
  }

  private customerWiseTableConfig(selected) {
    let tableColDef: any;
    this.gridDataSource = [];
    let tableName: string = '';
    let selectedValue = this.customerTypeList.find((x) => x.key == selected);
    console.log(selectedValue);

    if (selectedValue.type == 'Dealer') {
      tableName = 'Dealer Collection';
      tableColDef = [
        {
          headerName: 'Code  ',
          width: '10%',
          internalName: 'code',
          sort: true,
          type: '',
        },
        {
          headerName: 'Dealer ',
          width: '10%',
          internalName: 'name',
          sort: true,
          type: '',
        },
        {
          headerName: 'Mobile Number ',
          width: '10%',
          internalName: 'mobileNumber',
          sort: true,
          type: '',
        },
        {
          headerName: 'Payment Method',
          width: '5%',
          internalName: 'paymentMethodName',
          sort: true,
          type: '',
        },
        {
          headerName: 'Area',
          width: '15%',
          internalName: 'creditControllAreaName',
          sort: true,
          type: '',
        },
        {
          headerName: 'Bank Name',
          width: '15%',
          internalName: 'bankName',
          sort: true,
          type: '',
        },
        {
          headerName: 'Number(Account)',
          width: '15%',
          internalName: 'number',
          sort: true,
          type: '',
        },
        {
          headerName: 'Amount',
          width: '10%',
          internalName: 'number',
          sort: true,
          type: '',
        },
        {
          headerName: 'Number(Manual)',
          width: '10%',
          internalName: 'manualNumber',
          sort: true,
          type: '',
        },
      ];
    } else if (selectedValue.type == 'Sub-Dealer') {
      tableName = 'Sub Dealer Collection';
      tableColDef = [
        {
          headerName: 'Code  ',
          width: '10%',
          internalName: 'code',
          sort: true,
          type: '',
        },
        {
          headerName: 'Sub Dealer ',
          width: '10%',
          internalName: 'name',
          sort: true,
          type: '',
        },
        {
          headerName: 'Mobile Number ',
          width: '10%',
          internalName: 'mobileNumber',
          sort: true,
          type: '',
        },
        {
          headerName: 'Payment Method',
          width: '5%',
          internalName: 'paymentMethodName',
          sort: true,
          type: '',
        },
        {
          headerName: 'Area',
          width: '15%',
          internalName: 'creditControllAreaName',
          sort: true,
          type: '',
        },
        {
          headerName: 'Bank Name',
          width: '15%',
          internalName: 'bankName',
          sort: true,
          type: '',
        },
        {
          headerName: 'Number(Account)',
          width: '15%',
          internalName: 'number',
          sort: true,
          type: '',
        },
        {
          headerName: 'Amount',
          width: '10%',
          internalName: 'number',
          sort: true,
          type: '',
        },
        {
          headerName: 'Number(Manual)',
          width: '10%',
          internalName: 'manualNumber',
          sort: true,
          type: '',
        },
      ];
    } else if (selectedValue.type == 'Direct Project') {
      tableName = 'Project Collection';
      tableColDef = [
        {
          headerName: 'Code  ',
          width: '10%',
          internalName: 'code',
          sort: true,
          type: '',
        },
        {
          headerName: 'Project ',
          width: '10%',
          internalName: 'name',
          sort: true,
          type: '',
        },
        {
          headerName: 'Mobile Number ',
          width: '10%',
          internalName: 'mobileNumber',
          sort: true,
          type: '',
        },
        {
          headerName: 'Payment Method',
          width: '5%',
          internalName: 'paymentMethodName',
          sort: true,
          type: '',
        },
        {
          headerName: 'Area',
          width: '15%',
          internalName: 'creditControllAreaName',
          sort: true,
          type: '',
        },
        {
          headerName: 'Bank Name',
          width: '15%',
          internalName: 'bankName',
          sort: true,
          type: '',
        },
        {
          headerName: 'Number(Account)',
          width: '15%',
          internalName: 'number',
          sort: true,
          type: '',
        },
        {
          headerName: 'Amount',
          width: '10%',
          internalName: 'number',
          sort: true,
          type: '',
        },
        {
          headerName: 'Number(Manual)',
          width: '10%',
          internalName: 'manualNumber',
          sort: true,
          type: '',
        },
      ];
    } else if (selectedValue.type == 'Customer') {
      tableName = 'Cutomer Collection';
      tableColDef = [
        {
          headerName: 'Code  ',
          width: '10%',
          internalName: 'code',
          sort: true,
          type: '',
        },
        {
          headerName: 'Customer ',
          width: '10%',
          internalName: 'name',
          sort: true,
          type: '',
        },
        {
          headerName: 'Mobile Number ',
          width: '10%',
          internalName: 'mobileNumber',
          sort: true,
          type: '',
        },
        {
          headerName: 'Payment Method',
          width: '5%',
          internalName: 'paymentMethodName',
          sort: true,
          type: '',
        },
        {
          headerName: 'Area',
          width: '15%',
          internalName: 'creditControllAreaName',
          sort: true,
          type: '',
        },
        {
          headerName: 'Bank Name',
          width: '15%',
          internalName: 'bankName',
          sort: true,
          type: '',
        },
        {
          headerName: 'Number(Account)',
          width: '15%',
          internalName: 'number',
          sort: true,
          type: '',
        },
        {
          headerName: 'Amount',
          width: '10%',
          internalName: 'number',
          sort: true,
          type: '',
        },
        {
          headerName: 'Number(Manual)',
          width: '10%',
          internalName: 'manualNumber',
          sort: true,
          type: '',
        },
      ];
    }

    this.getCustomerDetails(selected);
    this.configureTable(tableName, tableColDef);
  }

  private configureTable(tableName: string, tableCol: any) {
    this.ptableSettings = {
      tableID: 'Setup-table',
      tableClass: 'table table-border ',
      tableName: tableName,
      tableRowIDInternalName: 'Id',

      tableColDef: tableCol,
      enabledSearch: true,
      enabledSerialNo: true,
      pageSize: 10,
      enabledPagination: true,
      //enabledAutoScrolled:true,
      enabledDeleteBtn: true,
      enabledEditBtn: true,
      // enabledCellClick: true,
      enabledColumnFilter: true,
      // enabledDataLength:true,
      // enabledColumnResize:true,
      // enabledReflow:true,
      // enabledPdfDownload:true,
      // enabledExcelDownload:true,
      // enabledPrint:true,
      // enabledColumnSetting:true,
      enabledRecordCreateBtn: false,
      // enabledTotal:true,
    };
  }
}
