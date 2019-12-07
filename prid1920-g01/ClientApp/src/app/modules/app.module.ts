import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutes } from '../routing/app.routing';
import { AppComponent } from '../components/app/app.component';
import { JwtInterceptor } from '../interceptors/jwt.interceptor';
import { NavMenuComponent } from '../components/nav-menu/nav-menu.component';
import { HomeComponent } from '../components/home/home.component';
import { LoginComponent } from '../components/login/login.component';
import { CounterComponent } from '../components/counter/counter.component';
import { FetchDataComponent } from '../components/fetch-data/fetch-data.component';
import { UserListComponent } from '../components/userlist/userlist.component';
import { UnknownComponent } from '../components/unknown/unknown.component';
import { RestrictedComponent } from '../components/restricted/restricted.component';
import { SharedModule } from './shared.module';
import { EditUserComponent } from '../components/edit-user/edit-user.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SetFocusDirective } from '../directives/setfocus.directive';
import { QuestionsListComponent } from '../components/questionsList/questionsList.component';
import { EditPostComponent } from '../components/edit-post/edit-post.component';
import { QuestionComponent } from '../components/question/question.component';
import { MarkdownModule } from 'ngx-markdown';
import { AceEditorModule } from 'ng2-ace-editor';

@NgModule({

  declarations: [

    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginComponent,
    UserListComponent,
    UnknownComponent,
    RestrictedComponent,
    SetFocusDirective,
    EditUserComponent,
    EditPostComponent,
    QuestionsListComponent,
    QuestionComponent

  ],

  entryComponents: [EditUserComponent,EditPostComponent],

  imports: [

    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    AppRoutes,
    BrowserAnimationsModule,
    SharedModule,
    AceEditorModule,
    MarkdownModule.forRoot(),

  ],

  providers: [

    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }

  ],

  bootstrap: [AppComponent]

})

export class AppModule { }