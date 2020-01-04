import { Component, Input } from "@angular/core";
import { Post } from "src/app/models/post";
import { AuthenticationService } from "src/app/services/authentication.service";
import { EditPostService } from "src/app/services/edit-post.service";
import { Router } from "@angular/router";
import { Role, User } from "src/app/models/user";
import { QuestionComponent } from "../../question/question.component";
import { Vote } from "src/app/models/vote";
import { VoteService } from "src/app/services/vote.service";
import { Tag } from "src/app/models/tag";

@Component({
    selector: 'post',
    templateUrl: './view-post.component.html',
    styleUrls: ['./view-post.component.css']
})


export class PostComponent {

    @Input() data: Post;
    @Input() accepted: boolean = false; 




    constructor(public editPostService: EditPostService,
        private authenticationService: AuthenticationService,
        private router: Router,
        private question: QuestionComponent,
        private voteService: VoteService) {

    }

    activateAction(id: string) {
        return (this.authenticationService.currentUser.id == id || this.authenticationService.currentUser.role == Role.Admin);

    }
    activateDelete() {
        return (this.authenticationService.currentUser.role == Role.Admin || (this.isEmpty(this.data.comments) && this.isEmpty(this.data.responses)));
    }

    get currentUser() { return this.authenticationService.currentUser; }

    delete(post: Post) {
        this.editPostService.delete(post).subscribe(res => {
            if (res) {
                this.question.refrech();
            }
        });
    }

    edit(post: Post) {
        var backUp = new Post(post);
        var backUpTags = new Tag(post.tags);
        this.editPostService.edit(post).subscribe(res => {
            if (!res) {
                //post.body = backUp.body;
                this.question.refrech();
            }
        }
        );
    }

    onReply() {
        this.editPostService.addReply(this.question.id).subscribe(res => {
            if (res) {
                //this.data.responses.push(res);
                this.question.refrech();
            }
        });
    }

    toAccept(post: Post) {

        this.editPostService.accept(post).subscribe(res => {
            if (res) {
                this.question.accepted = post;
            }
        });
    }

    toClear(post: Post) {

        this.editPostService.unAccept(post).subscribe(res => {
            if (res) {
                this.question.accepted = null;
            }
        });
    }



    onVote(upDown: string) {
        var vote = this.data.votes.find(v => v.userId == this.currentUser.id);
        if (vote == null) {
            this.addVote(upDown);
        }
        else if (vote.upDown != upDown) {
            vote.upDown = upDown;
            this.updateVote(vote);
        }
        else if (vote.upDown == upDown) {
            this.deleteVote();
        }
    }

    private addVote(upDown: string) {
        var newVote = new Vote({});
        newVote.postId = this.data.id;
        newVote.upDown = upDown;
        this.voteService.newVote(newVote).subscribe(res => {
            if (res) {
                this.question.refrech();
            }
        });
    }

    private deleteVote() {
        this.voteService.delete(this.data.id).subscribe(res => {
            if (res) {
                this.question.refrech();
            }
        });
    }

    private updateVote(vote: Vote) {
        this.voteService.update(vote).subscribe(res => {
            if (res) {
                this.question.refrech();
            }
        });
    }


    noAccepted() {
        return this.question.accepted == null;
    }
    activateButtonAcceptResponse() {
        return (
            this.data.title == null &&
            this.question.question.user != null &&
            this.currentUser != null &&
            this.currentUser.id == this.question.question.user.id);
    }

    activeColor(value: string) {
        var vote = (this.data.votes != null && this.currentUser != null) ? this.data.votes.find(v => v.userId == this.currentUser.id) : null;
        return (this.currentUser != null && vote != null && (vote.upDown == value));
    }

    activateVoteUp() {
        var vote = (this.data.votes != null && this.currentUser != null) ? this.data.votes.find(v => v.userId == this.currentUser.id) : null;
        return this.currentUser && (+this.currentUser.reputation >= 15 || (vote != null && vote.upDown == "1") );
    }
    activateVoteDown() {
        var vote = (this.data.votes != null && this.currentUser != null) ? this.data.votes.find(v => v.userId == this.currentUser.id) : null;
        return this.currentUser && (+this.currentUser.reputation >= 30 || (vote != null && vote.upDown == "-1"));
    }

    private isEmpty(value: any): boolean {
        if (value == null || value == undefined || value.length == 0) return true;
        return false;
      }
}