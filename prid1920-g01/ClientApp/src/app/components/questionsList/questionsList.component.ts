import { Component } from '@angular/core';
import { Post } from 'src/app/models/post';
import { PostService } from 'src/app/services/post.service';
import { EditPostService } from 'src/app/services/edit-post.service';
import { AuthenticationService } from 'src/app/services/authentication.service';


@Component({
    selector: 'app-questionsList',
    templateUrl: './questionsList.component.html',
    styleUrls: ['./questionsList.component.css']
})


export class QuestionsListComponent {

    allActive: boolean;
    newestActive: boolean;
    votesActive: boolean;
    unansweredActive: boolean;
    tagActive: boolean;
    posts: Post[] = [];
    filter = "";


    constructor(private postService: PostService,
        public editPostService: EditPostService,
        private authenticationService: AuthenticationService) {
        this.allQuest();
    }

    filterChanged(filterValue: string) {
        this.filter = filterValue;
        this.getQuestions();

    }

    allQuest() {
        this.unActive();
        this.allActive = true;
        this.getQuestions();
    }

    newest() {
        this.unActive();
        this.newestActive = true;
        this.getQuestions();
    }

    votes() {
        this.unActive();
        this.votesActive = true;
        this.getQuestions();
    }

    unanswered() {
        this.unActive();
        this.unansweredActive = true;
        this.getQuestions();
    }

    tag() {
        this.unActive();
        this.tagActive = true;
        this.getQuestions();
    }

    private getQuestions() {
        if (this.allActive) {
            this.postService.getQuestions(this.filter).subscribe(q => {
                this.posts = q;
            });

        } else if (this.newestActive) {
            this.postService.getQuestionsByNewest(this.filter).subscribe(q => {
                this.posts = q;
            });

        } else if (this.votesActive) {
            this.postService.getQuestionsByVotes(this.filter).subscribe(q => {
                this.posts = q;
            });

        } else if (this.unansweredActive) {
            this.postService.getQuestionsByUnanswered(this.filter).subscribe(q => {
                this.posts = q;
            });

        } else if (this.tagActive) {
            this.postService.getQuestionsByTags(this.filter).subscribe(q => {
                this.posts = q;
            });

        }
    }
    onCreate() {
        this.editPostService.create().subscribe(res => {
            if(res){
                this.posts.push(res);
            }
        });
    }


    private unActive() {
        this.allActive = false;
        this.newestActive = false;
        this.votesActive = false;
        this.unansweredActive = false;
        this.tagActive = false;

    }

    get currentUser() { return this.authenticationService.currentUser; }




}
