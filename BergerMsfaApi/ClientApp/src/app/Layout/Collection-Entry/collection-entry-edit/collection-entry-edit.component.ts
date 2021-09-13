import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from '../../../Shared/Modules/alert/alert.service';
import { NgbDateParserFormatter, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { FocusDealerService } from '../../../Shared/Services/FocusDealer/focus-dealer.service';
import { CommonService } from '../../../Shared/Services/Common/common.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MapObject } from 'src/app/Shared/Enums/mapObject';
import { forkJoin, Subscription } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { FocusDealer, SaveFocusDealer } from 'src/app/Shared/Entity/FocusDealer/FocusDealer';
import { StatusTypes } from 'src/app/Shared/Enums/statusTypes';
import { Payments } from 'src/app/Shared/Entity/CollectionEntry/payments';
import { CollectionEntryService } from 'src/app/Shared/Services/Collection/collectionentry.service';
import { EnumDynamicTypeCode } from 'src/app/Shared/Enums/dynamic-type-code';
import { DynamicDropdownService } from 'src/app/Shared/Services/Setup/dynamic-dropdown.service';

@Component({
    selector: 'app-collection-entry-edit',
    templateUrl: './collection-entry-edit.component.html',
    styleUrls: ['./collection-entry-edit.component.css']
})
export class CollectionEntryEditComponent implements OnInit {

	payment: Payments;
	paymentForm: FormGroup;
    dealers: any[] = [];
    users: any[] = [];
	lstpaymentmethod:any[] = [];
	lstCustomerType:any[] = [];
	lstCreditControl:any[] = [];
	actInStatusTypes: MapObject[] = StatusTypes.actInStatusType;
    depots: any[] = [];
    territories: any[]=[]
    zones: any[] = [];
	// @ViewChild('fileInput', {static:false}) fileInput: FileUpload;

	private subscriptions: Subscription[] = [];

	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private formBuilder: FormBuilder,
		private alertService: AlertService,
		private commonService: CommonService,
		private collectionEntryServiceService: CollectionEntryService,
		private formatter:NgbDateParserFormatter,
		private dynamicDropdownService: DynamicDropdownService
		) { }

	ngOnInit() {
		this.populateDropdownDataList();
		// this.alertService.fnLoading(true);
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			if (id) {
				this.alertService.fnLoading(true);
				this.collectionEntryServiceService.getCollectionById(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.payment = res.data as Payments;
							this.initCollectionForm();
						}
					});
			} else {
				this.payment = new Payments();
				
			}
		});
		this.subscriptions.push(routeSubscription);
	}

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	populateDropdownDataList() {
		// this.loadDynamicDropdown();

        const forkJoinSubscription1 = forkJoin([
            this.commonService.getUserInfoListByCurrentUserWithoutZoUser(),
            this.commonService.getDealerList('',[]),
			this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.Payment),
			this.dynamicDropdownService.GetDropdownByTypeCd(EnumDynamicTypeCode.Customer),
            this.commonService.getCreditControlAreaList(),


        ]).subscribe(([users, dealers,paymentMethod,customerType, creditControlAreas]) => {
            this.users = users.data;
            this.dealers = dealers.data;
			this.lstpaymentmethod = paymentMethod.data;
			this.lstCustomerType = customerType.data;
			this.lstCreditControl = creditControlAreas.data;
			

        }, (err) => { }, () => { });
		
		const forkJoinSubscription2 = forkJoin([
			this.commonService.getDepotList(),
			this.commonService.getTerritoryList(),
			this.commonService.getZoneList()
		  ]).subscribe(
			([plants, territories, zones]) => {
			  this.depots = plants.data;
			  this.territories = territories.data;
			  this.zones = zones.data;
			},
			(err) => {},
			() => {}
		  );

		this.subscriptions.push(forkJoinSubscription1);
		this.subscriptions.push(forkJoinSubscription2);
    }

	initCollectionForm() {
		this.createForm();
	}

	createForm() {
		// console.log('hi there')
		// console.log(this.focusDealer.validFrom)
		// console.log(this.focusDealer.validTo)
		this.paymentForm = this.formBuilder.group({
			// dealerId: [this.payment.code, [Validators.required]],
			employeeId: [this.payment.employeeId],
			name: [this.payment.name],
			address: [this.payment.address],
			mobileNumber: [this.payment.mobileNumber],
			bankName: [this.payment.bankName],
			number: [this.payment.number],
			amount: [this.payment.amount],
			manualNumber: [this.payment.manualNumber],
			remarks: [this.payment.remarks],
			dealerId: [this.payment.dealerId?parseInt(this.payment.dealerId):null],
			paymenyMethodId: [this.payment.paymentMethodId],
			creditControlArea: [this.payment.creditControlAreaId],
			customerTypeId: [this.payment.customerTypeId],
			depot: [this.payment.depot],
			territory: [this.payment.territory],
			zone: [this.payment.zone],
		
		});

console.log(this.paymentForm);
	}

	get formControls() { return this.paymentForm.controls; }

	onSubmit() {
		const controls = this.paymentForm.controls;

		if (this.paymentForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			return;
		}

		const editedCollectionEntry = this.prepareCollectionEntry();
		this.updateCollectionEntry(editedCollectionEntry);

	}

	prepareCollectionEntry(): Payments {
		const controls = this.paymentForm.controls;

		const _payments = new Payments();
		// _focusDealer.clear();
		_payments.id = this.payment.id;
		_payments.dealerId = controls['dealerId'].value;
		_payments.employeeId = controls['employeeId'].value;
		_payments.name = controls['name'].value;
		_payments.address = controls['address'].value;
		_payments.mobileNumber = controls['mobileNumber'].value;
		_payments.bankName = controls['bankName'].value;
		_payments.number = controls['number'].value;
		_payments.amount = controls['amount'].value;
		_payments.manualNumber = controls['manualNumber'].value;
		_payments.remarks = controls['remarks'].value;
		_payments.paymentMethodId = controls['paymenyMethodId'].value;
		_payments.creditControlAreaId = controls['creditControlArea'].value;
		_payments.customerTypeId = controls['customerTypeId'].value;
		_payments.createdBy=this.payment.createdBy;
		_payments.createdTime=this.payment.createdTime;
		_payments.modifiedBy=this.payment.modifiedBy;
		_payments.modifiedTime=this.payment.modifiedTime;
		_payments.status=this.payment.status;
		_payments.collectionDate=this.payment.collectionDate;
		_payments.sapId=this.payment.sapId;
		_payments.depot=controls['depot'].value;
		_payments.territory=controls['territory'].value;
		_payments.zone=controls['zone'].value;
		// _payments.name=this.payment.name;





		return _payments;
	}


	updateCollectionEntry(_payment: Payments) {
		this.alertService.fnLoading(true);
		const updateSubscription = this.collectionEntryServiceService.updateCollection(_payment)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`Collection Entry has been updated successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(updateSubscription);
	}

	getComponentTitle() {
		let result = `Edit Collection Form - ${this.payment.name}`;
		return result;
	}

	goBack() {
		this.router.navigate([`/collection/payment-list`], { relativeTo: this.activatedRoute });
	}

	stringToInt(value): number {
		return Number.parseInt(value);
	}

	changeDealer() {
		var dealerId = this.paymentForm.controls['dealerId'].value;
		if (dealerId) {
			var dealer = this.dealers.find(x => x.id == dealerId);
			if (dealer) {
				this.paymentForm.controls.name.setValue(dealer.customerName);
			}
		}
	}

	private throwError(errorDetails: any) {
		// this.alertService.fnLoading(false);
		console.log("error", errorDetails);
		let errList = errorDetails.error.errors;
		if (errList.length) {
			// console.log("error", errList, errList[0].errorList[0]);
			// this.alertService.tosterDanger(errList[0].errorList[0]);
		} else {
			// this.alertService.tosterDanger(errorDetails.error.message);
		}
	}

	


	
}
