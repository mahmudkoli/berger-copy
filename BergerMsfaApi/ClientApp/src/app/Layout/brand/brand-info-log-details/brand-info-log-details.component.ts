import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { BrandService } from 'src/app/Shared/Services/Brand/brand.service';
import { Subscription, of } from 'rxjs';
import { IPTableSetting } from '../../../Shared/Modules/p-table';
import { finalize, take, delay, distinctUntilChanged, debounceTime } from 'rxjs/operators';


@Component({
  selector: 'app-brand-info-log-details',
  templateUrl: './brand-info-log-details.component.html',
	styleUrls: ['./brand-info-log-details.component.css']
})
export class BrandInfoLogDetailsComponent implements OnInit {

	brandInfoLogs: any[];
	
	private subscriptions: Subscription[] = [];
	
	constructor(
		private router: Router,
		private brandService: BrandService,
		private alertService: AlertService,
		private activatedRoute: ActivatedRoute,
		private commonService: CommonService,
		private modalService: NgbModal
		
	) { }
	id: any;
	division: any;
	packSize: any;
	materialDescription: any;
	materialGroupOrBrand: any;
	materialCode: any;

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
		const brandInfoLogsSubscription = this.brandService.getBrandStatusInfoLogDetails(this.id)
			.pipe(
				finalize(() => { this.alertService.fnLoading(false); })
			)
			.subscribe(res => {
				this.brandInfoLogs = res.data;
				console.log("done");
				console.log(this.brandInfoLogs);
				this.brandInfoLogs.forEach(obj => {
					this.materialCode = obj.materialCode;
					this.materialGroupOrBrand = obj.materialGroupOrBrand;
					this.materialDescription = obj.materialDescription;
					this.packSize = obj.packSize;
					this.division = obj.division;
				
				});
				
			},
			(error) => {console.log(error);});
		this.subscriptions.push(brandInfoLogsSubscription);
       
    }

	//getPropertyValue(propertyValue) {
	//	if (propertyValue == "NonCBI") return "Non CBI";
	//	else if (propertyValue == "NonPREMIUM") return "Non PREMIUM";
	//	else if (propertyValue == "NonMTS") return "Non MTS";
	//}

	public backToTheList() {
		this.router.navigate(['/brand']);
	}
	public ptableSettings: IPTableSetting = {
		tableID: "brands-table",
		tableClass: "table table-border ",
		tableName: 'Brand Info Status Log',
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
