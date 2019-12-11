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
import { CommentsComponents } from '../components/views/view-comments/view-comments.component';
import { PostComponent } from '../components/views/view-post/view-post.component';
import { TagsComponent } from '../components/views/view-tags/view-tags.component';
import { EditCommentComponent } from '../components/edit-comment/edit-comment.component'
import { TagsListComponent } from '../components/tagsList/tagsList.component';
import { EditTagComponent } from '../components/edit-tag/edit-tag.component';


@NgModule({

  declarations: [

    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    UserListComponent,
    UnknownComponent,
    RestrictedComponent,
    SetFocusDirective,
    EditUserComponent,
    EditPostComponent,
    QuestionsListComponent,
    QuestionComponent,
    CommentsComponents,
    PostComponent,
    TagsComponent,
    EditCommentComponent,
    TagsListComponent,
    EditTagComponent

  ],

  entryComponents: [
    EditUserComponent, 
    EditPostComponent, 
    EditCommentComponent,
    EditTagComponent
  ],

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