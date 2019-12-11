import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from '../components/home/home.component';
import { CounterComponent } from '../components/counter/counter.component';
import { FetchDataComponent } from '../components/fetch-data/fetch-data.component';
import { UserListComponent } from '../components/userlist/userlist.component';
import { LoginComponent } from '../components/login/login.component';
import { RestrictedComponent } from '../components/restricted/restricted.component';
import { UnknownComponent } from '../components/unknown/unknown.component';
import { AuthGuard } from '../services/auth.guard';
import { Role } from '../models/user';
import { QuestionsListComponent } from '../components/questionsList/questionsList.component';
import { QuestionComponent} from '../components/question/question.component';
import { TagsListComponent } from '../components/tagsList/tagsList.component';

const appRoutes: Routes = [

  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },

  {
    path: 'users',
    component: UserListComponent,
    canActivate: [AuthGuard],
    data: { roles: [Role.Admin] }
  },
  { path: 'posts', component: QuestionsListComponent },
  { path: 'question/:id', component: QuestionComponent},
  { path: 'tags', component: TagsListComponent},
  { path: 'login',component: LoginComponent},
  { path: 'restricted', component: RestrictedComponent },
  { path: '**', component: UnknownComponent }

];

export const AppRoutes = RouterModule.forRoot(appRoutes);