import { UserService } from './../shared/user.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styles: []
})
export class HomeComponent implements OnInit {
  menus;
  userDetails;

  constructor(private router: Router, private service: UserService) { }

  ngOnInit() {
    this.service.getNavigationMenus().subscribe(res => {
      this.service.getUserProfile().subscribe(
      profile => {
        this.menus = res;
        this.userDetails =profile
      })
    })
  }


  onLogout() {
    localStorage.removeItem('token');
    this.router.navigate(['/user/login']);
  }
}