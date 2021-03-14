import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { LoanServiceService } from "src/shared/loan-service.service";
import { Banks, landType } from "src/shared/variables";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"],
})
export class AppComponent implements OnInit {
  // title = 'app';
  form;

  step: number = 1;

  selectedLandType = {
    id: 8,
    name: "არ არის არჩეული უძრავი ქონების ტიპი",
  };
  landType = landType;

  banks = Banks;
  selectedBankTab:string = "credo";

  errorMessage: boolean = false;
  loading: boolean = false;

  constructor(
    private loanService: LoanServiceService,
  ) {}

  ngOnInit() {
    console.log(this.banks[0].name);
    this.initializeForm();
  }

  initializeForm() {
    this.form = new FormGroup({
      cadNumber: new FormControl("", Validators.required),
      landTypeId: new FormControl("", Validators.required),
      IsCanalisation: new FormControl(0),
      IsRoad: new FormControl(0),
      IsWater: new FormControl(0),
      IsGas: new FormControl(0),
      Bedrooms: new FormControl(0),
      Floor: new FormControl(0),
      ParkingId: new FormControl(0),
      Electricity: new FormControl(0),
      Rooms: new FormControl(0),
    });
  }

  submitForm() {
    // before service
    this.errorMessage = false;
    this.loading = true;

    const mapData = {
      "CadastralCode": this.form.controls.cadNumber.value,
      "ProductTypeId": this.form.controls.landTypeId.value,
      "IsCanalisation": this.form.controls.IsCanalisation.value?1:null,
      "IsRoad": this.form.controls.IsRoad.value?1:null,
      "IsWater": this.form.controls.IsWater.value?1:null,
      "IsGas": this.form.controls.IsGas.value?1:null,
      "Bedrooms": this.form.controls.Bedrooms.value,
      "Floor": this.form.controls.Floor.value,
      "ParkingId": this.form.controls.ParkingId.value?1:null,
      "Electricity": this.form.controls.Electricity.value?1:null,
      "Rooms": this.form.controls.Rooms.value
    }

    console.log(mapData);
    this.loanService.calculate(mapData).subscribe(
      (res: any) => {
        this.loading = false;
        this.step = 3;
        console.log(res);
      },
      (err: any) => {
        console.log(err)
        this.loading = false; 
        this.errorMessage = true;
        this.step = 3;
      }
    );


    // public string CadastralCode { get; set; }
		// public int? ProductTypeId { get; set; }

		// public int? IsCanalisation { get; set; }
		// public int? IsRoad { get; set; }
		// public int? IsWater { get; set; }
		// public int? IsGas { get; set; }
		// public int? Bedrooms { get; set; }
		// public int? Floor { get; set; }
		// public int? ParkingId { get; set; }
		// public int? Electricity { get; set; }
		// public int? Rooms{ get; set; }
    // in service
    //in response
    // this.step = 3;
    // this.loading = false;

    // if error message
    // this.errorMessage = true;
  }

  landTypeClick(type) {
    if (this.selectedLandType !== type) {
      this.selectedLandType = type;
      this.form.controls.landTypeId.setValue(type.id);
    } else {
      this.selectedLandType = {
        id: 8,
        name: "არ არის არჩეული უძრავი ქონების ტიპი",
      };
      this.form.controls.landTypeId.setValue("");
    }

    // console.log(type.id)
  }

  iconSelect(type) {
    if (!type) {
      return "8";
    } else {
      return this.selectedLandType.id;
    }
  }

  nextStep() {
    if (this.step === 1) {
      this.step = 2;
    }
  }

  backStep() {
    // if(this.step === 2){
    this.step = 1;
    // }
  }

  selectBank(bank){
    console.log(bank)
    this.selectedBankTab = bank.name;
  }
}
