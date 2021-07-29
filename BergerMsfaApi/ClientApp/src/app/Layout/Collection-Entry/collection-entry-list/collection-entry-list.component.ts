import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { finalize } from 'rxjs/operators';
import { CollectionReportQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';
import { IPTableServerQueryObj } from 'src/app/Shared/Modules/p-table';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';
import { AuthService } from 'src/app/Shared/Services/Users';
import { forkJoin, of, Subscription } from 'rxjs';
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
  query: CollectionReportQuery;
	searchOptionQuery: SearchOptionQuery;
  PAGE_SIZE: number;
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; 
  ptableSettings: any = null;
  gridDataSource: any[] = [];
  selectedType: number;
	private subscriptions: Subscription[] = [];


  constructor(
    private collectionEntryService: CollectionEntryService,
    private alertService: AlertService,
    private activityPermissionService: ActivityPermissionService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private dynamicDropdownService: DynamicDropdownService,
	  private authService: AuthService
  ) {
    this._initPermissionGroup();
  }

  ngOnInit() {
    this.loadDynamicDropdown();
		this.searchConfiguration();

    // this.selectedType = CustomerTypeEnum.Dealer;
  }
  ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}
	getData = (query) => this.collectionEntryService.getCollectionByType(query);

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
    // this.gridDataSource = [];
    let tableName: string = '';
    let selectedValue = this.customerTypeList.find((x) => x.key == selected);
    console.log(selectedValue);

    if (selectedValue.type == 'Dealer') {
      tableName = 'Dealer Collection';
      tableColDef = [
        // {
        //   headerName: 'Code  ',
        //   width: '10%',
        //   internalName: 'code',
        //   sort: true,
        //   type: '',
        // },
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
          internalName: 'amount',
          sort: true,
          type: 'text',
          displayType: 'number-format-color'
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
        // {
        //   headerName: 'Code  ',
        //   width: '10%',
        //   internalName: 'code',
        //   sort: true,
        //   type: '',
        // },
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
          internalName: 'amount',
          sort: true,
          type: 'text',
          displayType: 'number-format-color'
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
        // {
        //   headerName: 'Code  ',
        //   width: '10%',
        //   internalName: 'code',
        //   sort: true,
        //   type: '',
        // },
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
          internalName: 'amount',
          sort: true,
          type: 'text',
          displayType: 'number-format-color'
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
        // {
        //   headerName: 'Code  ',
        //   width: '10%',
        //   internalName: 'code',
        //   sort: true,
        //   type: '',
        // },
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
          internalName: 'amount',
          sort: true,
          type: 'text',
          displayType: 'number-format-color'
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

    // this.getCustomerDetails(selected);
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
      // enabledDeleteBtn: false,
      // enabledEditBtn: false,
      // enabledCellClick: true,
      // enabledColumnFilter: true,
      // enabledDataLength:true,
      // enabledColumnResize:true,
      // enabledReflow:true,
      // enabledPdfDownload:true,
      // enabledExcelDownload:true,
      // enabledPrint:true,
      // enabledColumnSetting:true,
      // enabledRecordCreateBtn: false,
      // enabledTotal:true,
    };

		this.ptableSettings.enabledDeleteBtn = this.authService.isAdmin;
		this.ptableSettings.enabledEditBtn = this.authService.isAdmin;

  }

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		
		if (event.action == "delete-item") {
			this.deleteCollection(event.record.id);
			
		}

    if (event.action == "edit-item") {
			//Edit code..
      this.router.navigate([`/collection/payment-details/${event.record.id}`]);
			
		}
	}

	deleteCollection(id) {
		this.alertService.confirm("Are you sure to delete this?",
			() => {
				this.alertService.fnLoading(true);
				this.collectionEntryService.deleteCollection(id)
					.pipe(finalize(() => { this.alertService.fnLoading(false); }))
					.subscribe((res: any) => {
						console.log('res from del func', res);
						this.alertService.tosterSuccess("Collection has been deleted successfully.");
							this.gridDataSource.forEach((value, index) => {
								if (value.id == id) this.gridDataSource.splice(index, 1);
							});
						},
						(error) => {
							console.log(error);
						});
			},
			() => { });
	}
  searchConfiguration() {
		this.query = new CollectionReportQuery({
			page: 1,
			pageSize: this.PAGE_SIZE,
			sortBy: 'createdTime',
			isSortAscending: false,
			globalSearchValue: '',
			depot: '',
			salesGroups: [],
			territories: [],
			zones: [],
			userId: null,
			date: null,
			dealerId: null,
			paymentMethodId: null,
			paymentFromId: null,

		});
		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();
	}

	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		searchOptionDef:[
			new SearchOptionDef({searchOption:EnumSearchOption.Depot, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Territory, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Zone, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Date, isRequired:false}),
			new SearchOptionDef({searchOption:EnumSearchOption.UserId, isRequired:false}),
			new SearchOptionDef({searchOption:EnumSearchOption.DealerId, isRequired:false}),
			new SearchOptionDef({searchOption:EnumSearchOption.PaymentMethodId, isRequired:false}),
			new SearchOptionDef({searchOption:EnumSearchOption.PaymentFromId, isRequired:true}),

		]});

	searchOptionQueryCallbackFn(queryObj:SearchOptionQuery) {
		console.log('Search option query callback: ', queryObj);
		this.query.depot = queryObj.depot;
		this.query.territories = queryObj.territories;
		this.query.zones = queryObj.zones;
		this.query.date = queryObj.date;
		this.query.userId = queryObj.userId;
		this.query.dealerId = queryObj.dealerId;
		this.query.paymentMethodId = queryObj.paymentMethodId;
		this.query.paymentFromId = queryObj.paymentFromId;

		// this.ptableSettings.downloadDataApiUrl = this.getDownloadDataApiUrl(this.query);
		this.loadCollectionData();
	}

  serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query.page = queryObj.pageNo;
		this.query.pageSize = queryObj.pageSize;
		this.query.sortBy = queryObj.orderBy || this.query.sortBy;
		this.query.isSortAscending = queryObj.isOrderAsc || this.query.isSortAscending;
		this.query.globalSearchValue = queryObj.searchVal;
		this.loadCollectionData();
	}

  loadCollectionData() {
		this.alertService.fnLoading(true);
		const reportsSubscription = this.getData(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					this.gridDataSource = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
					console.log("res.data", res);

          this.onChange(this.searchOptionQuery.paymentFromId);
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(reportsSubscription);
	}
}
