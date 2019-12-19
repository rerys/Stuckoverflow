import { Component, Input } from "@angular/core";
import { AuthenticationService } from "src/app/services/authentication.service";
import { EditCommentService } from "src/app/services/edit-comment.service";
import { Comment } from '../../../models/comment';
import { Role } from "src/app/models/user";


@Component({
    selector: 'comments',
    templateUrl: './view-comments.component.html',
    styleUrls: ['./view-comments.component.css']
})

export class CommentsComponents {

    @Input() data: Comment[];
    @Input() postId: string;

    constructor(private authenticationService: AuthenticationService,
        public editCommentService: EditCommentService) {

    }


    activateAction(id: string) {
        return !(this.authenticationService.currentUser.id == id || this.authenticationService.currentUser.role == Role.Admin);
    }

    get currentUser() { return this.authenticationService.currentUser; }

    edit(comment: Comment) {
        var backUp = new Comment(comment);
        this.editCommentService.edit(comment).subscribe(res => {
            if (!res) {
                comment.body = backUp.body;
            }
 
        });
    }

    delete(comment: Comment) {
        this.editCommentService.delete(comment).subscribe(res => {
            if (res) {
                this.data.splice(this.data.indexOf(comment), 1);
            }
        }
        );
    }
    create() {
        this.editCommentService.create(this.postId).subscribe(res => {
            this.data.push(res);
        });
    }


}