import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/shared/user.service';
import { RequestService } from 'src/app/shared/request.service';
import { TimeslotService } from 'src/app/shared/timeslot.service.';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-create-request',
  templateUrl: './create-request.component.html',
  styleUrls: ['./create-request.component.css']
})
export class CreateRequestComponent implements OnInit {

  constructor(private router: Router, private userService: UserService, private requestService: RequestService,
    private timeSlotService: TimeslotService,private toastr: ToastrService) { }
  teachers;
  timeslots;
  issueTypes;
  errors;
  selectedTeacher =[];

  ngOnInit() {
    this.errors =[];
    this.requestService.formModel.reset();
    this.userService.getTeachers().subscribe( teachersRes =>
      this.requestService.getIssueTypes().subscribe(issueTypes =>
        {
          
        this.teachers = teachersRes;
        this.issueTypes = issueTypes;

        })
    );
  }

  onSubmit() {
    this.requestService.createRequest().subscribe(
      (res: any) => {
        if (res) {
          this.requestService.formModel.reset();
          this.toastr.success('New request created', 'New request created');
        } else {
          this.toastr.error("Unbale to create new request",'Unbale to create nee request');
        }
      },
      err => {
        this.toastr.error("Unbale to create new request",'Unbale to create nee request');
      }
    );
}
onChangeTimeSlot(timeSlotID){

  this.timeSlotService.getTeachersAvailibiity(this.requestService.formModel.value.TimeSlot).subscribe(
  res => {
    this.errors = res;
    if (this.errors.length > 0){

      alert('Error');
    }else{

      this.errors=[];
    }
  },
  err => {
    console.log(err);
  },
);
}
  onChange(value){
    this.timeSlotService.getUserTimeSlots(value).subscribe(
      res => {
        this.timeslots = res;
        if (this.timeslots.length ==0){

          alert('Teacher doesnot have any schedule assigned');
        }
      },
      err => {
        console.log(err);
      },
    );
  }
}
