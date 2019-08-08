import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class TimeslotService {
  readonly BaseURI = 'http://localhost:62469/api';
  constructor(private fb:FormBuilder,private http: HttpClient) {

     
  }
  getUserTimeSlots(userID) {
    return this.http.get(this.BaseURI + '/timeslot/'+userID);
  }

  
  getTeachersAvailibiity(timeslotID) {
    //let params = new HttpParams().set('timeslotID', timeslotID); //{ params: params }
    return this.http.get(this.BaseURI + '/timeslot/availibility/'+timeslotID);
  }
}
