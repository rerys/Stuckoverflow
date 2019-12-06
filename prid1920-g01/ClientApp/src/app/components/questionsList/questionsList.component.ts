import { Component } from '@angular/core';
import * as _ from 'lodash';
import { Post } from 'src/app/models/post';
import { PostService } from 'src/app/services/post.service';

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


    constructor(private postService: PostService) {
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

        } else if (this.unanswered) {
            this.postService.getQuestionsByUnanswered(this.filter).subscribe(q => {
                this.posts = q;
            });

        } else if (this.tagActive) {
            this.postService.getQuestionsByTags(this.filter).subscribe(q => {
                this.posts = q;
            });

        }
        console.log(this.posts);
    }




    create() {
        // const post = new Post({});
        // const dlg = this.dialog.open(EditPostComponent, { data: { post, isNew: true, add: false } });
        // dlg.beforeClose().subscribe(res => {
        //     if (res) {
        //         this.dataSource.data = [...this.dataSource.data, new Post(res)];
        //         this.postService.add(res).subscribe(res => {
        //             if (!res) {
        //                 this.snackBar.open(`There was an error at the server. The post has not been created! Please try again.`, 'Dismiss', { duration: 10000 });
        //                 this.refresh();
        //             }
        //         });
        //     }
        // });
    }

    private unActive() {
        this.allActive = false;
        this.newestActive = false;
        this.votesActive = false;
        this.unansweredActive = false;
        this.tagActive = false;

    }
}
