import { Injectable } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Injectable({
  providedIn: 'root'
})
export class RequestService {
  readonly BaseURI = 'http://localhost:62469/api';
  constructor(private fb:FormBuilder,private http: HttpClient) {

     
  }
  formModel = this.fb.group({
    IssueType: ['', Validators.required],
    Issue: ['', Validators.required],
    Teacher: ['', Validators.required],
    TimeSlot : ['', Validators.required],
  });


  createRequest() {
    var body = {
      IssueTypeID: this.formModel.value.IssueType,
      Issue: this.formModel.value.Issue,
      ToUserID: this.formModel.value.Teacher,
      TimeSlotID : this.formModel.value.TimeSlot,
    };

    return this.http.post(this.BaseURI + '/request/create-request', body);
  }

  changeStatus(requestID,statusID) {
    var param ={
      status : statusID
    }

    return this.http.put(this.BaseURI + '/request/change-status/'+requestID , param);
  }

  getMyRequest(){
    return this.http.get(this.BaseURI + '/request/my-requests')
  }

  getUrgentRequest(){
    return this.http.get(this.BaseURI + '/request/urgent-requests')
  }

  getIssueTypes() {
    return this.http.get(this.BaseURI + '/request/issue-types');
  }
}
