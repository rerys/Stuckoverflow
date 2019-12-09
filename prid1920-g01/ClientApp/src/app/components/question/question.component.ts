import { Component, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { Post } from 'src/app/models/post';
import { PostService } from 'src/app/services/post.service';
import { RouterLinkActive, ActivatedRoute, Router } from '@angular/router';
import { EditPostService } from 'src/app/services/edit-post.service';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
    selector: 'app-question',
    templateUrl: './question.component.html',
    styleUrls: ['./question.component.css']
})

export class QuestionComponent{

    public question: Post;
    public id: number;

    constructor(private postService: PostService, 
        private route: ActivatedRoute,
        public editPostService: EditPostService,
        private authenticationService: AuthenticationService) {
        this.question = new Post([]);

        this.id = this.route.snapshot.params['id'];
        this.postService.getQuestionById(this.id).subscribe(q => {
            this.question = q;
        });
    }

    onReply(){
        this.editPostService.addReply(this.id).subscribe();
    }

    get currentUser() { return this.authenticationService.currentUser; }



}