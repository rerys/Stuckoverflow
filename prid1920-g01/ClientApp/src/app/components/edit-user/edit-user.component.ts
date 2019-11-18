import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Inject } from '@angular/core';
import { UserService } from '../../services/user.service';
import { FormBuilder } from '@angular/forms';
import * as _ from 'lodash';
import { User, Role } from 'src/app/models/user';
import { EditUSerBase } from 'src/app/helpers/editUserBase';


@Component({
    selector: 'app-edit-user-mat',
    templateUrl: './edit-user.component.html',
    styleUrls: ['./edit-user.component.css']
})

export class EditUserComponent extends EditUSerBase {


    public isNew: boolean;
    public confirmPassword: boolean;
    public signup: boolean;

    constructor(public dialogRef: MatDialogRef<EditUserComponent>,

        @Inject(MAT_DIALOG_DATA) public data: { user: User; isNew: boolean; signup: boolean; },
        fb: FormBuilder,
        userService: UserService

    ) {

        super(data, userService, fb);
        this.isNew = data.isNew;
        this.confirmPassword = data.isNew;
        this.signup = data.signup;


    }


    onNoClick(): void { this.dialogRef.close(); }

    update() { this.dialogRef.close(this.frm.value); }

    cancel() { this.dialogRef.close(); }

    passwordChanged() {

        if (!this.isNew && this.ctlPassword != null) {
            this.confirmPassword = true;

        } else if (!this.isNew) {
            this.confirmPassword = false;
        }
    }

}