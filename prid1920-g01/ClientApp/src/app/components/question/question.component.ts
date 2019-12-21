import { Component, OnDestroy, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { Post } from 'src/app/models/post';
import { PostService } from 'src/app/services/post.service';
import { ActivatedRoute, Router } from '@angular/router';
import { EditPostService } from 'src/app/services/edit-post.service';
import { Subscription } from 'rxjs';
import { emit } from 'cluster';
import { User } from 'src/app/models/user';

@Component({
    selector: 'app-question',
    templateUrl: './question.component.html',
    styleUrls: ['./question.component.css']
})

export class QuestionComponent implements OnInit, OnDestroy {

    question: Post;
    questionSubsription: Subscription;

    accepted: Post;
    acceptedSubcription: Subscription;

    responses: Post[];
    responsesSubsription: Subscription;


    public id: number;
    public userPost: User;

    constructor(private postService: PostService,
        private route: ActivatedRoute,
        public editPostService: EditPostService, ) {
        this.question = new Post([]);

    }

    ngOnInit() {
        this.id = this.route.snapshot.params['id'];
        this.refrech();

    }

    public refrech() {

        this.questionSubsription = this.postService.questionSubject.subscribe(post => {
            this.question = post;
            this.userPost = post.user;
        });

        this.responsesSubsription = this.postService.responsesSubject.subscribe(responses => {
            this.responses = responses;
        });

        this.acceptedSubcription = this.postService.acceptedSubject.subscribe(accepted => {
            this.accepted = accepted;
        });



        this.postService.getRefrechPost(this.id);
        this.postService.emitAllResponses();
        this.postService.emitAccepted();
        this.postService.emitPost();


    }

    ngOnDestroy() {
        this.questionSubsription.unsubscribe();
        this.acceptedSubcription.unsubscribe();
        this.responsesSubsription.unsubscribe();
    }


}