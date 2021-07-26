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

@Component({
    selector: 'app-focus-dealer-add',
    templateUrl: './focus-dealer-add.component.html',
    styleUrls: ['./focus-dealer-add.component.css']
})
export class FocusDealerAddComponent implements OnInit {

	focusDealer: FocusDealer;
	focusDealerForm: FormGroup;
    dealers: any[] = [];
    users: any[] = [];
	actInStatusTypes: MapObject[] = StatusTypes.actInStatusType;
	// @ViewChild('fileInput', {static:false}) fileInput: FileUpload;

	private subscriptions: Subscription[] = [];

	constructor(private activatedRoute: ActivatedRoute,
		private router: Router,
		private formBuilder: FormBuilder,
		private alertService: AlertService,
		private commonService: CommonService,
		private focusDealerService: FocusDealerService,
		private formatter:NgbDateParserFormatter
		) { }

	ngOnInit() {
		this.populateDropdownDataList();
		// this.alertService.fnLoading(true);
		const routeSubscription = this.activatedRoute.params.subscribe(params => {
			const id = params['id'];
			if (id) {
				this.alertService.fnLoading(true);
				this.focusDealerService.getFocusDealerById(id)
					.pipe(finalize(() => this.alertService.fnLoading(false)))
					.subscribe(res => {
						if (res) {
							this.focusDealer = res.data as FocusDealer;
							this.initFocusDealers();
						}
					});
			} else {
				this.focusDealer = new FocusDealer();
				this.focusDealer.clear();
				this.initFocusDealers();
			}
		});
		this.subscriptions.push(routeSubscription);
	}

	ngOnDestroy() {
		this.subscriptions.forEach(sb => sb.unsubscribe());
	}

	populateDropdownDataList() {
        const forkJoinSubscription1 = forkJoin([
            this.commonService.getUserInfoListByCurrentUserWithoutZoUser(),
            this.commonService.getDealerList('',[]),
        ]).subscribe(([users, dealers]) => {
            this.users = users.data;
            this.dealers = dealers.data;
        }, (err) => { }, () => { });

		this.subscriptions.push(forkJoinSubscription1);
    }

	initFocusDealers() {
		this.createForm();
	}

	createForm() {
		console.log('hi there')
		console.log(this.focusDealer.validFrom)
		console.log(this.focusDealer.validTo)
		this.focusDealerForm = this.formBuilder.group({
			dealerId: [this.focusDealer.dealerId, [Validators.required]],
			employeeId: [this.focusDealer.employeeId, [Validators.required]],
			validFrom: [],
			validTo: [],
			// validFrom: [this.formatter.parse(this.focusDealer.validFrom.toString()),[Validators.required]],
			// validTo: [this.formatter.parse(this.focusDealer.validTo? this.focusDealer.validTo.toString():null)],
		});

		if (this.focusDealer.validFrom) {
			const fromDate = new Date(this.focusDealer.validFrom);
			this.focusDealerForm.controls.validFrom.setValue({
				year: fromDate.getFullYear(),
				month: fromDate.getMonth()+1,
				day: fromDate.getDate()
			});
		}

		if (this.focusDealer.validTo) {
			const toDate = new Date(this.focusDealer.validTo);
			this.focusDealerForm.controls.validTo.setValue({
				year: toDate.getFullYear(),
				month: toDate.getMonth()+1,
				day: toDate.getDate()
			});
		}

		this.focusDealerForm.controls.validFrom.setValidators([Validators.required]);
		this.focusDealerForm.controls.validFrom.updateValueAndValidity();
		this.focusDealerForm.controls.validTo.setValidators([Validators.required]);
		this.focusDealerForm.controls.validTo.updateValueAndValidity();
	}

	get formControls() { return this.focusDealerForm.controls; }

	onSubmit() {
		const controls = this.focusDealerForm.controls;

		if (this.focusDealerForm.invalid) {
			Object.keys(controls).forEach(controlName =>
				controls[controlName].markAsTouched()
			);
			return;
		}

		const editedFocusDealers = this.prepareFocusDealers();
		if (editedFocusDealers.id) {
			this.updateFocusDealers(editedFocusDealers);
		}
		else {
			this.createFocusDealers(editedFocusDealers);
		}
	}

	prepareFocusDealers(): SaveFocusDealer {
		const controls = this.focusDealerForm.controls;

		const _focusDealer = new SaveFocusDealer();
		_focusDealer.clear();
		_focusDealer.id = this.focusDealer.id;
		_focusDealer.dealerId = controls['dealerId'].value;
		_focusDealer.employeeId = controls['employeeId'].value;
		_focusDealer.validFrom = controls['validFrom'].value;
		_focusDealer.validTo = controls['validTo'].value;

		// if (controls['validFrom'].value) {
		// 	const value = controls['validFrom'].value;
		// 	_focusDealer.validFrom =  this.formatter.format(value)
		//   }
		// if (controls['validTo'].value) {
		// 	const value = controls['validTo'].value;
		// 	_focusDealer.validTo =  this.formatter.format(value)
		//   }

		const fromDate = controls['validFrom'].value;
		if (fromDate && fromDate.year && fromDate.month && fromDate.day) {
			_focusDealer.validFrom = new Date(fromDate.year,fromDate.month-1,fromDate.day);
		} else {
			_focusDealer.validFrom = null;
		}

		const toDate = controls['validTo'].value;
		if (toDate && toDate.year && toDate.month && toDate.day) {
			_focusDealer.validTo = new Date(toDate.year,toDate.month-1,toDate.day);
		} else {
			_focusDealer.validTo = null;
		}

		return _focusDealer;
	}

	createFocusDealers(_focusDealer: SaveFocusDealer) {
		this.alertService.fnLoading(true);
		const createSubscription = this.focusDealerService.createFocusDealer(_focusDealer)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`New Focus Dealer has been added successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(createSubscription);
	}

	updateFocusDealers(_focusDealer: SaveFocusDealer) {
		this.alertService.fnLoading(true);
		const updateSubscription = this.focusDealerService.updateFocusDealer(_focusDealer)
			.pipe(finalize(() => this.alertService.fnLoading(false)))
			.subscribe(res => {
				this.alertService.tosterSuccess(`Focus Dealer has been saved successfully.`);
				this.goBack();
			},
				error => {
					this.throwError(error);
				});
		this.subscriptions.push(updateSubscription);
	}

	getComponentTitle() {
		let result = 'Create Focus Dealer';
		if (!this.focusDealer || !this.focusDealer.id) {
			return result;
		}

		result = `Edit Focus Dealer - ${this.focusDealer.dealerName}`;
		return result;
	}

	goBack() {
		this.router.navigate([`/dealer/focus-dealer-list`], { relativeTo: this.activatedRoute });
	}

	stringToInt(value): number {
		return Number.parseInt(value);
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
