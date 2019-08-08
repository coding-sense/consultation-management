import { Component, OnInit, Input } from '@angular/core';
import { RequestService } from 'src/app/shared/request.service';
import { Router } from '@angular/router';
import {NgbModal, ModalDismissReasons,NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';

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
export class NgbdModalContent {
  @Input() issue;

  constructor(public activeModal: NgbActiveModal) {}
}
@Component({
  selector: 'app-my-request',
  templateUrl: './my-request.component.html',
  styleUrls: ['./my-request.component.css']
})
export class MyRequestComponent implements OnInit {

  constructor(private router: Router, private requestService: RequestService,private modalService: NgbModal,
    private toastr: ToastrService) { }
  requests;
  closeResult: string;
  ngOnInit() {
    this.requestService.getMyRequest().subscribe(
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
    const modalRef = this.modalService.open(NgbdModalContent);
    modalRef.componentInstance.issue = request.issue;
  }

  changeRequest(requestID ,statusID){
    debugger;
    this.requestService.changeStatus(requestID ,statusID).subscribe(
      (res: any) => {
        debugger;
        if (res) {
          this.toastr.success('Status changed', 'Status changed');

          this.requestService.getMyRequest().subscribe(
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
  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return  `with: ${reason}`;
    }
  }
}
