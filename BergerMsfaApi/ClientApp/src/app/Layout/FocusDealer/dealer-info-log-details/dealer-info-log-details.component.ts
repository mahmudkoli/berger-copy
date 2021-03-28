import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { BrandService } from 'src/app/Shared/Services/Brand/brand.service';
import { Subscription, of } from 'rxjs';
import { IPTableSetting } from '../../../Shared/Modules/p-table';
import { finalize, take, delay, distinctUntilChanged, debounceTime } from 'rxjs/operators';
import { FocusdealerService } from '../../../Shared/Services/FocusDealer/focusdealer.service';


@Component({
	selector: 'app-dealer-info-log-details',
	templateUrl: './dealer-info-log-details.component.html',
	styleUrls: ['./dealer-info-log-details.component.css']
})
export class DealerInfoLogDetailsComponent implements OnInit {

	dealerInfoLogs: any[];
	
	private subscriptions: Subscription[] = [];
	
	constructor(
		private router: Router,
		private dealerService: FocusdealerService,
		private alertService: AlertService,
		private activatedRoute: ActivatedRoute,
		private commonService: CommonService,
		private modalService: NgbModal
		
	) { }
	id: any;
	division: any;
	salesOffice: any;
	salesGroup: any;

	ngOnInit() {
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			this.id = params['id'];
			of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
				this.loadPage();
			});

		});
		this.subscriptions.push(routeSubscription);
		
		
	}
	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}
	loadPage() {
		this.alertService.fnLoading(true);
		const dealerInfoLogsSubscription = this.dealerService.getDealerLogByDealerId(this.id)
			.pipe(
				finalize(() => { this.alertService.fnLoading(false); })
			)
			.subscribe(res => {
				this.dealerInfoLogs = res.data;
				//console.log("done");
				//console.log(this.dealerInfoLogs);
				this.dealerInfoLogs.forEach(obj => {
					//this.materialCode = obj.materialCode;
					//this.materialGroupOrBrand = obj.materialGroupOrBrand;
					this.division = obj.division;
					this.salesOffice = obj.salesOffice;
					this.salesGroup = obj.salesGroup;


					//this.packSize = obj.packSize;
					
				
				});
				
			},
			(error) => {console.log(error);});
		this.subscriptions.push(dealerInfoLogsSubscription);
       
    }

	//getPropertyValue(propertyValue) {
	//	if (propertyValue == "NonCBI") return "Non CBI";
	//	else if (propertyValue == "NonPREMIUM") return "Non PREMIUM";
	//	else if (propertyValue == "NonMTS") return "Non MTS";
	//}

	public backToTheList() {
		this.router.navigate(['/dealer/dealerList']);
	}
	public ptableSettings: IPTableSetting = {
		tableID: "dealers-table",
		tableClass: "table table-border ",
		tableName: 'Dealer Info Status Log',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Created By', width: '20%', internalName: 'createdBy', sort: false, type: "" },
			{ headerName: 'Property Name', width: '20%', internalName: 'propertyName', sort: false, type: "" },
			{ headerName: 'Property Value', width: '20%', internalName: 'propertyValue', sort: false, type: "" },
			{ headerName: 'Created Time', width: '20%', internalName: 'createdTime', sort: false, type: "" },
			
		],
		//enabledSearch: true,
		enabledSerialNo: true,
		// pageSize: 10,
		enabledPagination: true,
		// enabledDeleteBtn: true,
		// enabledEditBtn: true,
		//enabledCellClick: true,
		//enabledColumnFilter: false,
		// enabledRecordCreateBtn: true,
		enabledDataLength: true,
		// newRecordButtonText: 'New ELearning'
	};

}