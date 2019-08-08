import { Component, OnInit, Input } from '@angular/core';
import { RequestService } from 'src/app/shared/request.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'ngbd-modal-content',
  template: `
    <div class="modal-header">
      <h4 class="modal-title">Issue Description</h4>
      <button type="button" class="close" aria-label="Close" (click)="activeModal.dismiss('Cross click')">
        <span aria-hidden="true">&times;</span>
      </button>
    </div>
    <div class="modal-body">
      <p>{{issue}}</p>
    </div>
    <div class="modal-footer">
      <button type="button" class="btn btn-outline-dark" (click)="activeModal.close('Close click')">Close</button>
    </div>
  `
})
export class NgbdModalContentUrgent {
  @Input() issue;

  constructor(public activeModal: NgbActiveModal) {}
}
@Component({
  selector: 'app-urgent-request',
  templateUrl: './urgent-request.component.html',
  styleUrls: ['./urgent-request.component.css']
})
export class UrgentRequestComponent implements OnInit {

  constructor(private router: Router, private requestService: RequestService,private modalService: NgbModal,
    private toastr: ToastrService) { }
  requests;
  closeResult: string;
  ngOnInit() {
    this.requestService.getUrgentRequest().subscribe(
      res => {
        console.log(res)
        this.requests = res;
      },
      err => {
        console.log(err);
      },
    );
  }

  open(request) {
    const modalRef = this.modalService.open(NgbdModalContentUrgent);
    modalRef.componentInstance.issue = request.issue;
  }

  changeRequest(requestID ,statusID){

    this.requestService.changeStatus(requestID ,statusID).subscribe(
      (res: any) => {

        if (res) {
          this.toastr.success('Status changed', 'Status changed');

          this.requestService.getUrgentRequest().subscribe(
            res => {
              console.log(res)
              this.requests = res;
            },
            err => {
              console.log(err);
            },
          );
        } else {
          this.toastr.error("Failed",'Failed');
        }
      },
      err => {
        this.toastr.error("Failed",'Failed');
      }
    );


  }
}
