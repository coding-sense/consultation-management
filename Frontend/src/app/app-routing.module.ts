import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserComponent } from './user/user.component';
import { RegistrationComponent } from './home/registration/registration.component';
import { LoginComponent } from './user/login/login.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from './auth/auth.guard';
import { ForbiddenComponent } from './forbidden/forbidden.component';
import { CreateRequestComponent } from './home/create-request/create-request.component';
import { MyRequestComponent } from './home/my-request/my-request.component';
import { UrgentRequestComponent } from './home/urgent-request/urgent-request.component';


const routes: Routes = [
{path : '',redirectTo:'/user/login',pathMatch:'full'},
{path : 'user',component: UserComponent,
  children :[
  { path : 'login',component :LoginComponent },
]},

{ path : 'home',component :HomeComponent,
children : [
  { path : 'registration',component :RegistrationComponent  },
  { path : 'create-request',component :CreateRequestComponent , canActivate :[AuthGuard],data :{permittedRoles:['Admin','Student']}  },
  { path : 'my-request',component :MyRequestComponent , canActivate :[AuthGuard],data :{permittedRoles:['Teacher','Student']}  },
  { path : 'urgent-request',component :UrgentRequestComponent , canActivate :[AuthGuard],data :{permittedRoles:['Teacher']}  }
]},
{ path : 'forbidden',component :ForbiddenComponent},
{
  path: '**',
  redirectTo: '/home',
  pathMatch: 'full'
}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
