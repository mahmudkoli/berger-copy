import { Component, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { APIResponse } from 'src/app/Shared/Entity';
import { DealerStatusExcelImportModel } from 'src/app/Shared/Entity/DealerInfo/DealerStatusExcel';
import { EnumDealerStatusExcelImportType, EnumDealerStatusExcelImportTypeLabel } from 'src/app/Shared/Enums/dealer-info';
import { AlertService } from 'src/app/Shared/Modules/alert/alert.service';
import { FocusDealerService } from 'src/app/Shared/Services/FocusDealer/focus-dealer.service';

@Component({
  selector: 'app-modal-excel-import-dealer-status',
  templateUrl: './modal-excel-import-dealer-status.component.html',
  styleUrls: ['./modal-excel-import-dealer-status.component.css']
})
export class ModalExcelImportDealerStatusComponent implements OnInit {

  isFormValid: boolean = false;
  selectedFile: File = null;
  selectedDealerStatusExcelType: EnumDealerStatusExcelImportType = EnumDealerStatusExcelImportType.ClubSupreme;
	selectedFileName: string;
  tosterMsgError: string = "Something went wrong";
  fileError: string = '';
  exampleImportExcelFileUrl = 'examples/DealerStatusClubSupreme.xlsx';

  enumDealerStatusExcelImportTypeLabels = EnumDealerStatusExcelImportTypeLabel.enumDealerStatusExcelImportTypeLabel;

  constructor(
    public activeModal: NgbActiveModal,
    private dealerService: FocusDealerService, 
    private alertService: AlertService
    ) { 
      
    }

  ngOnInit() {
  }

  onChangeDealerStatusExcelType(event: any) {
    switch (this.selectedDealerStatusExcelType) {

      case EnumDealerStatusExcelImportType.LastYearAppointed:
        this.exampleImportExcelFileUrl = 'examples/DealerStatusLastYearAppointed.xlsx';
        break;
      case EnumDealerStatusExcelImportType.ClubSupreme:
        this.exampleImportExcelFileUrl = 'examples/DealerStatusClubSupreme.xlsx';
        break;
      
      case EnumDealerStatusExcelImportType.BussinessCategory:
        this.exampleImportExcelFileUrl = 'examples/DealerStatusBussinessCategory.xlsx';
        break;
      default:
        break;
    }
  }

	onChangeInputFile(event: any) {
		if (event.target.files && event.target.files[0]) {
      const file = event.target.files[0];
      this.selectedFile = file;
			if (!this.isValidFile(file)) return;
      this.isFormValid = true;
      this.selectedFileName = file.name;
    } else {
      this.clearFile();
		}
	}

	isValidFile(file) {
		const fileExt = file.name.split('.').pop().toLowerCase();
		if (!(['xls','xlsx'].indexOf(fileExt) > -1)) {
			this.fileError = 'Invalid file type';
      this.clearFile();
			return false;
		}
		this.fileError = '';
		return true;
	}
  
  clearFile() { 
    this.selectedFile = null;
    this.isFormValid = false;
    this.selectedFileName = '';
  }

  submit() {
    if(!this.selectedFile) {
      console.log('File is not selected');
      return;
    }

    const model: DealerStatusExcelImportModel =  {file: this.selectedFile, type: this.selectedDealerStatusExcelType};
    console.log(model);
    this.dealerService.excelDealerStatusUpdate(model).subscribe(
      (res: any) => {
        console.log(res.data);
        this.downloadExcelFile(res.data);
        this.alertService.tosterSuccess(`Dealer status has been updated successfully.`);
        this.activeModal.close(`Dealer status has been updated successfully.`);
      },
      (err) => {
        console.log(err);
        this.showError();
      }
    );
  }

  downloadExcelFile(data) {
    // const blob: any = new Blob([data.file], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const blob = this.base64ToBlob(data.file);
    let link = document.createElement("a");

    if (link.download !== undefined) {
      let url = URL.createObjectURL(blob);
      link.setAttribute("href", url);
      link.setAttribute("download", `${data.fileName}.xlsx`);
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
    }
  }

  base64ToBlob(b64Data, sliceSize=512) {
    let byteCharacters = atob(b64Data); //data.file there
    let byteArrays = [];

    for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        let slice = byteCharacters.slice(offset, offset + sliceSize);
        let byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }
        let byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
    }

    return new Blob(byteArrays, {type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'});
  }

  showError(msg: string = null) {
    this.activeModal.close(msg ? msg : this.tosterMsgError);
  }

}
