import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { Router } from '@angular/router';
import { of, Subscription } from 'rxjs';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { CommonService } from 'src/app/Shared/Services/Common/common.service';
import { delay, finalize, take } from 'rxjs/operators';
import { IPTableServerQueryObj, IPTableSetting } from 'src/app/Shared/Modules/p-table';
import { CollectionConfig } from 'src/app/Shared/Entity/KPI/CollectionPlan';
import { CollectionPlanService } from 'src/app/Shared/Services/KPI/CollectionPlanService';

@Component({
    selector: 'app-collection-config-list',
    templateUrl: './collection-config-list.component.html',
    styleUrls: ['./collection-config-list.component.css']
})
export class CollectionConfigListComponent implements OnInit, OnDestroy {

	collectionConfigs: CollectionConfig[];

	// Subscriptions
	private subscriptions: Subscription[] = [];

	constructor(
		private router: Router,
		private alertService: AlertService,
		private collectionPlanService: CollectionPlanService,
		private modalService: NgbModal,
		private commonService: CommonService) {
	}

	ngOnInit() {
		of(undefined).pipe(take(1), delay(1000)).subscribe(() => {
			this.loadCollectionConfigsPage();
		});
	}

	ngOnDestroy() {
		this.subscriptions.forEach(el => el.unsubscribe());
	}

	loadCollectionConfigsPage() {
		this.alertService.fnLoading(true);
		const schemeMastersSubscription = this.collectionPlanService.getCollectionConfigs()
			.pipe(finalize(() => { this.alertService.fnLoading(false); }))
			.subscribe(
				(res) => {
					console.log("res.data", res.data);
					this.collectionConfigs = res.data;
				},
				(error) => {
					console.log(error);
				});
		this.subscriptions.push(schemeMastersSubscription);
	}

	editCollectionConfig(id) {
		this.router.navigate(['/collection-plan/collection-config-edit', id]);
	}

	public ptableSettings: IPTableSetting = {
		tableID: "CollectionConfigs-table",
		tableClass: "table table-border ",
		tableName: 'Collection Config',
		tableRowIDInternalName: "id",
		tableColDef: [
			{ headerName: 'Changeable Max Date', width: '100%', internalName: 'changeableMaxDateText', sort: false, type: "" },
		],
		enabledSerialNo: true,
		enabledEditBtn: true,
		enabledPagination: false,
		tableFooterVisibility: false
	};

	public fnCustomTrigger(event) {
		console.log("custom  click: ", event);

		if (event.action == "edit-item") {
			this.editCollectionConfig(event.record.id);
		}
	}
}

