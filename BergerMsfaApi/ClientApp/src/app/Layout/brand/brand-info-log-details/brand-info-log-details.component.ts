import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { BrandService } from 'src/app/Shared/Services/Brand/brand.service';
import { Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { IPTableSetting } from '../../../Shared/Modules/p-table';


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
	brandInfoId: any;
	
	ngOnInit() {
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			this.brandInfoId = id;
			
			if (id) {
				this.alertService.fnLoading(true);
				this.brandService.getBrandStatusInfoLogDetails(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.brandInfoLogs = res.data;
							console.log(this.brandInfoLogs);
							this.brandInfoLogs.forEach(obj => {
								obj.propertyValue = (obj.propertyValue == "CBI" || obj.propertyValue == "PREMIUM" || obj.propertyValue == "MTS") ? obj.propertyValue : this.getPropertyValue(obj.propertyValue);
								
							});
						}
					},
					(error) => {
						console.log(error);
					});
			}
		});
		this.subscriptions.push(routeSubscription);
	}
	getPropertyValue(propertyValue) {
		if (propertyValue == "NonCBI") return "Non CBI";
		else if (propertyValue == "NonPREMIUM") return "Non PREMIUM";
		else if (propertyValue == "NonMTS") return "Non MTS";
	}


	public ptableSettings: IPTableSetting = {
		tableID: "brands-table",
		tableClass: "table table-border ",
		tableName: 'Brand Info Status Log',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Created By', width: '20%', internalName: 'userfullName', sort: false, type: "" },
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
