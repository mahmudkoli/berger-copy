import { Component, OnInit } from '@angular/core';
import { ExampleService } from 'src/app/Shared/Services/example.service';

@Component({
  selector: 'app-example-form-validation',
  templateUrl: './example-form-validation.component.html',
  styleUrls: ['./example-form-validation.component.sass']
})
export class ExampleFormValidationComponent implements OnInit {
  selectedCompanies;
  companies: any[] = [];
  loading = false;
  companiesNames = ['Uber', 'Microsoft', 'Flexigen'];
  constructor(private exampleService: ExampleService) { }
  ngOnInit() {
    this.companiesNames.forEach((c, i) => {
      this.companies.push({ id: i, name: c });
    });    
  }

  addTagPromise(name) {
    return new Promise((resolve) => {
      this.loading = true;
      // Simulate backend call.
      setTimeout(() => {
        resolve({ id: 5, name: name, valid: true });
        this.loading = false;
      }, 1000);
    })
  }



  public userModel: User = new User();


  public submitUserFomr(model: User) {
    console.log("user model: ", model);
    this.fnGetDailyCMData();


  }

  private fnGetExampleData() {
    this.exampleService.getExampleData().subscribe(res => {
      console.log("example response: ", res);
    });
  }
  private fnGetDailyCMData() {
    this.exampleService.getDailyCMData().subscribe(res => {
      console.log("example response: ", res);
    });
  }

}

class User {
  public name: string;
  public emailAddress: string;
  public password: string;
  public address:string;
  public city:string;
  public state:string;
  public zip:number;
  constructor(
  ) { }
}
