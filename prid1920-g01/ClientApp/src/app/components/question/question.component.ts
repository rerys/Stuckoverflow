import { Component, OnInit } from '@angular/core';
import * as _ from 'lodash';
import { Post } from 'src/app/models/post';
import { PostService } from 'src/app/services/post.service';
import { RouterLinkActive, ActivatedRoute, Router } from '@angular/router';

@Component({
    selector: 'app-question',
    templateUrl: './question.component.html',
    styleUrls: ['./question.component.css']
})

export class QuestionComponent{

    public question: Post;

    constructor(private postService: PostService, private route: ActivatedRoute) {
        this.question = new Post([]);
        const id = this.route.snapshot.params['id'];
        this.postService.getQuestionById(id).subscribe(q => {
            this.question = q;
            console.log(this.question);
        });
    }



}