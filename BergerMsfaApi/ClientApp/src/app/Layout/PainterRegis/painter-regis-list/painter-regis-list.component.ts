import { Component, OnInit, Inject, ViewChild } from '@angular/core';
import { Painter } from '../../../Shared/Entity/Painter/Painter';

import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { Router } from '@angular/router';
import { PainterRegisService } from '../../../Shared/Services/Painter-Regis/painterRegister.service';
import { APIModel } from 'src/app/Shared/Entity';
import { Paginator } from 'primeng/paginator';
import { finalize } from 'rxjs/operators';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { PainterRegisterQuery } from 'src/app/Shared/Entity/Report/ReportQuery';
import { EnumSearchOption, SearchOptionDef, SearchOptionQuery, SearchOptionSettings } from 'src/app/Shared/Modules/search-option';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { Subscription } from 'rxjs';
import { PainterStatus } from 'src/app/Shared/Entity/Painter/PainterCall';

@Component({
    selector: 'app-painter-regis-list',
    templateUrl: './painter-regis-list.component.html',
    styleUrls: ['./painter-regis-list.component.css']
})
export class PainterRegisListComponent implements OnInit {

    public painterList: Painter[] = [];
    public baseUrl: string;
    query: PainterRegisterQuery;
	searchOptionQuery: SearchOptionQuery;
	PAGE_SIZE: number;
	data: any[];
	totalDataLength: number = 0; // for server side paggination
	totalFilterDataLength: number = 0; // for server side paggination
	private subscriptions: Subscription[] = [];


    first = 1;
    rows = 10;
    pagingConfig: APIModel;
    search: string = "";
 @ViewChild("paging", { static: false }) paging: Paginator;
    constructor(
        @Inject('BASE_URL') baseUrl: string,
        private router: Router,
        private painterRegisSvc: PainterRegisService,
        private alertService: AlertService,
        private modalService: NgbModal,
    ) {
        this.baseUrl = baseUrl;
        this.pagingConfig = new APIModel(1, 10);
    }

    ngOnInit() {
        // this.onLoadPainters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
		this.searchConfiguration();

    }
    
    detail(id) {
        this.router.navigate(['/painter/detail/' + id]);
    }
    updateStatus(id) {
        this.router.navigate(['/painter/update/' + id]);
    }
    
    updatePainterStatus(painter:PainterStatus) {
        console.log('Painter status: ', painter);
		this.alertService.fnLoading(true);
		const painterStatus = this.painterRegisSvc.UpdatePainterStatus(painter)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
                    this.loadReportsPage()
					// this.onLoadPainters(this.pagingConfig.pageNumber, this.pagingConfig.pageSize, this.search);
				},
				(error) => {
					console.log(error);
				});
	}

    


    getData = (query) => this.painterRegisSvc.GetPainterLists(query);
	
	searchConfiguration() {
		this.query = new PainterRegisterQuery({
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
			fromDate: null,
			toDate: null,
			painterId: null,
			painterType: null,
			painterMobileNo: ''
		});
		this.searchOptionQuery = new SearchOptionQuery();
		this.searchOptionQuery.clear();
	}

	searchOptionSettings: SearchOptionSettings = new SearchOptionSettings({
		searchOptionDef:[
			new SearchOptionDef({searchOption:EnumSearchOption.Depot, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.SalesGroup, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Territory, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.Zone, isRequiredBasedOnEmployeeRole:true}),
			new SearchOptionDef({searchOption:EnumSearchOption.FromDate, isRequired:false}),
			new SearchOptionDef({searchOption:EnumSearchOption.ToDate, isRequired:false}),
			new SearchOptionDef({searchOption:EnumSearchOption.UserId, isRequired:false}),
			new SearchOptionDef({searchOption:EnumSearchOption.PainterId, isRequired:false}),
			new SearchOptionDef({searchOption:EnumSearchOption.PainterTypeId, isRequired:false}),
			new SearchOptionDef({searchOption:EnumSearchOption.Text1, isRequired:false, textLabel: 'Painter Mobile No.'}),
		]});

	searchOptionQueryCallbackFn(queryObj:SearchOptionQuery) {
		console.log('Search option query callback: ', queryObj);
		this.query.depot = queryObj.depot;
		this.query.salesGroups = queryObj.salesGroups;
		this.query.territories = queryObj.territories;
		this.query.zones = queryObj.zones;
		this.query.fromDate = queryObj.fromDate;
		this.query.toDate = queryObj.toDate;
		this.query.userId = queryObj.userId;
		this.query.painterId = queryObj.painterId;
		this.query.painterType = queryObj.painterTypeId;
		this.query.painterMobileNo = queryObj.text1;
		this.loadReportsPage();
	}
	//#endregion

	//#region no need to change for another report
	loadReportsPage() {
		this.alertService.fnLoading(true);
		const reportsSubscription = this.getData(this.query)
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					this.painterList = res.data.items;
					this.totalDataLength = res.data.total;
					this.totalFilterDataLength = res.data.totalFilter;
                    this.painterList.forEach(obj => {
                        obj.statusText = (obj.status == 1) ? 'Active' : 'Inactive';
                        obj.statusBtnClass = 'btn-transition btn btn-sm btn-outline-' + ((obj.status == 1) ? 'primary' : 'warning') + ' d-flex align-items-center';
                        obj.statusBtnIcon = 'pr-1 fa fa-' + ((obj.status == 1) ? 'check' : 'ban');
                    });
               
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(reportsSubscription);
	}


    public ptableSettings: IPTableSetting = {
		tableID: "reports-table",
		tableClass: "table table-border ",
		// tableName: this.tableName,
		tableRowIDInternalName: "id",
		tableColDef: [
            { headerName: 'Painter No', width: '7%', internalName: 'painterNo', sort: false, type: "" },
            { headerName: 'Painter Name', width: '7%', internalName: 'painterName', sort: false, type: "" },
            { headerName: 'Painter Code', width: '7%', internalName: 'painterCode', sort: false, type: "" },
			{ headerName: 'painter Image Url', width: '6%', internalName: 'painterImageUrl', sort: false, type: "" },
			{ headerName: 'phone', width: '7%', internalName: 'phone', sort: true, type: "" },
			{ headerName: 'Depot Name', width: '15%', internalName: 'depotName', sort: true, type: "" },
			{ headerName: 'Sale Group Name', width: '10%', internalName: 'saleGroupName', sort: false, type: "" },
			{ headerName: 'Territory Name', width: '15%', internalName: 'territoryName', sort: false, type: "" },
			{ headerName: 'ZoneName', width: '15%', internalName: 'zoneName', sort: false, type: "" },
			{ headerName: 'Status', width: '10%', internalName: 'statusText', sort: true, type: "dynamic-button", onClick: 'true', className: 'statusBtnClass', innerBtnIcon: 'statusBtnIcon' },
			
        ],
		// enabledSearch: true,
		enabledSerialNo: true,
        enabledDetailsBtn: true,
		// pageSize: 10,
		enabledPagination: true,
		enabledDataLength: true,
		// enabledTotal: this.enabledTotal,
		
	};
	
	serverSiteCallbackFn(queryObj: IPTableServerQueryObj) {
		console.log('server site : ', queryObj);
		this.query.page = queryObj.pageNo;
		this.query.pageSize = queryObj.pageSize;
		this.query.sortBy = queryObj.orderBy || this.query.sortBy;
		this.query.isSortAscending = queryObj.isOrderAsc || this.query.isSortAscending;
		this.query.globalSearchValue = queryObj.searchVal;
		this.loadReportsPage();
	}

    public fnCustomTrigger(event) {


            if (event.action == "details-item") {
                this.detail(event.record.id);
            }
			

			// this.updateDealerStatus(dealerStatus);
		

		// if (event.cellName == 'viewDetailsText') {
		// 	let id = event.record.id;
		// 	this.detailsDealerInfoStatusLog(id);
		// }
	}


    public cellClickCallbackFn(event) {
        console.log(event);

        let painter = new PainterStatus();
			 if (event.cellName == "statusText") {
                painter.id=event.record.id;
                painter.status = event.record.status;
                 this.updatePainterStatus(painter)
				// dealerStatus.propertyName = 'IsLastYearAppointed';
				// dealerStatus.dealerId = event.record.id;
			}
    }

}
