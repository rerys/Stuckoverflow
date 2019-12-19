import { Component, Input } from "@angular/core";
import { Post } from "src/app/models/post";
import { AuthenticationService } from "src/app/services/authentication.service";
import { EditPostService } from "src/app/services/edit-post.service";
import { Router } from "@angular/router";
import { Role } from "src/app/models/user";

@Component({
    selector: 'post',
    templateUrl: './view-post.component.html',
    styleUrls: ['./view-post.component.css']
})


export class PostComponent {

    @Input() data: Post;
    @Input() id: number;
    @Input() accepted: boolean = false;
    @Input() postToAccept: Post = null;


    constructor(public editPostService: EditPostService,
        private authenticationService: AuthenticationService,
        private router: Router) {
    }

    activateAction(id: string) {
        return !(this.authenticationService.currentUser.id == id || this.authenticationService.currentUser.role == Role.Admin);

    }
 
    get currentUser() { return this.authenticationService.currentUser; }

    delete(post: Post) { 
        this.editPostService.delete(post).subscribe(res => {
            if (res) {
                delete this.data;
                if(this.data == null && this.postToAccept == null){
                    this.router.navigate(['/posts'])
                }
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

    toAccept(post: Post){

        this.editPostService.accept(post).subscribe(res => {
            if (res) {
                this.postToAccept.accepted = post;
            }
        });
    }

    toClear(post: Post){

        this.editPostService.unAccept(post).subscribe(res => {
            if (res) {
                this.postToAccept.accepted = null;
            }
        });
    }

    activateButtonAcceptResponse(){
        return (this.postToAccept != null && this.currentUser!= null && this.currentUser.id == this.postToAccept.user.id); 
    }

}