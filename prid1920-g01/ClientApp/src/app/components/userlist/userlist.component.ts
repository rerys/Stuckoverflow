import { Component } from '@angular/core';

import { User } from '../../models/user';

import { UserService } from '../../services/user.service';

@Component({

    selector: 'app-userlist',

    templateUrl: './userlist.component.html'

})

export class UserListComponent {

    users: User[] = [];

    constructor(private memberService: UserService) {

        memberService.getAll().subscribe(users => {

            this.users = users;

        })

    }

}