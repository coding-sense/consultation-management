import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule,FormsModule } from '@angular/forms'
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import { ToastrModule } from 'ngx-toastr';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './home/registration/registration.component';
import { UserService } from './shared/user.service';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthInterceptor } from './auth/auth.interceptor';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { CreateRequestComponent } from './home/create-request/create-request.component';
import { RequestService } from './shared/request.service';
import { TimeslotService } from './shared/timeslot.service.';
import { MyRequestComponent, NgbdModalContent } from './home/my-request/my-request.component';
import { UrgentRequestComponent, NgbdModalContentUrgent } from './home/urgent-request/urgent-request.component';

@NgModule({
  declarations: [
    AppComponent,
    UserComponent,
    RegistrationComponent,
    LoginComponent,
    HomeComponent,
    ForbiddenComponent,
    CreateRequestComponent,
    MyRequestComponent,
    NgbdModalContent,
    NgbdModalContentUrgent,
    UrgentRequestComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NgbModule,
    ToastrModule.forRoot({
      progressBar: true
    })
  ],
  entryComponents: [NgbdModalContent,NgbdModalContentUrgent],
  providers: [UserService,RequestService,TimeslotService, {
    provide: HTTP_INTERCEPTORS,
    useClass: AuthInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
