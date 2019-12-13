import { Component, Input } from "@angular/core";
import { Post } from "src/app/models/post";
import { AuthenticationService } from "src/app/services/authentication.service";
import { EditPostService } from "src/app/services/edit-post.service";

@Component({
    selector: 'post',
    templateUrl: './view-post.component.html',
    styleUrls: ['./view-post.component.css']
})


export class PostComponent {

    @Input() data: Post;
    @Input() id: number;


    constructor(public editPostService: EditPostService,
        private authenticationService: AuthenticationService) {
    }

    activateAction(id: string) {
        return this.authenticationService.currentUser.id != id;

    }

    get currentUser() { return this.authenticationService.currentUser; }

    delete(post: Post) { 
        this.editPostService.delete(post).subscribe(res => {
            if (res) {
                delete this.data;
            }
        });
    }

    edit(post: Post) {
        var backUp = new Post(post);
        this.editPostService.edit(post).subscribe(res => {
            if (!res) {
                post.body = backUp.body;
            }
        }
        );
    }

    onReply() {
        this.editPostService.addReply(this.id).subscribe(res => {
            if (res) {
                this.data.responses.push(res);
            }
        });
    }


}