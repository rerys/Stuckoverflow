import { Component, Input } from "@angular/core";
import { AuthenticationService } from "src/app/services/authentication.service";
import { EditCommentService } from "src/app/services/edit-comment.service";
import { Comment } from '../../../models/comment';


@Component({
    selector: 'comments',
    templateUrl: './view-comments.component.html',
    styleUrls: ['./view-comments.component.css']
})

export class CommentsComponents {

    @Input() data = new Comment({});
    @Input() postId: string;

    constructor(private authenticationService: AuthenticationService,
        public editCommentService: EditCommentService) {

    }
 

    activateAction(id: string) {
        return this.authenticationService.currentUser.id != id;

    }

    get currentUser() { return this.authenticationService.currentUser; }

    edit(comment: Comment) {
        this.editCommentService.edit(comment).subscribe();
    }

    delete(comment: Comment) {
        this.editCommentService.delete(comment).subscribe();
    }
    create() {
        this.editCommentService.create(this.postId).subscribe();
    }


}