import { Component, Input } from "@angular/core";
import { AuthenticationService } from "src/app/services/authentication.service";


@Component({
    selector: 'comments',
    templateUrl: './view-comments.component.html',
    styleUrls: ['./view-comments.component.css']
})

export class CommentsComponents {

    @Input() data = new Comment();

    constructor(private authenticationService: AuthenticationService) {

    }


    activateAction(id: string) {
        return this.authenticationService.currentUser.id != id;

    }

    get currentUser() { return this.authenticationService.currentUser; }


}