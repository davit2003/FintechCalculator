import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoanServiceService {

  constructor(
    private httpClient: HttpClient
  ) { }

  public calculate(mapData: any){
    return this.httpClient.post(
      'https://localhost:44355/api/FintechCalculator/GetById',
      mapData
    );
    // let userName
    // const body = {
    //     userName,
    //     password,
    //   };
  }
   // public login(userName: string, password: string) {
      
  
    //   return this.httpClient.post(
    //     `${environment.apiUrl}/api/Account/Login`,
    //     body,
    //     { headers: { skip: "true" } }
    //   );
    // }


}
