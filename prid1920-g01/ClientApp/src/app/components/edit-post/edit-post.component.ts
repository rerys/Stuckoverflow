import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Inject } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import * as _ from 'lodash';
import { Post } from 'src/app/models/post';
import { EditPostBase } from 'src/app/helpers/editPostbase';
import { PostService } from 'src/app/services/post.service';


@Component({
    selector: 'app-edit-user-mat',
    templateUrl: './edit-post.component.html',
    styleUrls: ['./edit-post.component.css']
})

export class EditPostComponent extends EditPostBase {


    public isNew: boolean;
    public add: boolean;

    constructor(public dialogRef: MatDialogRef<EditPostComponent>,

        @Inject(MAT_DIALOG_DATA) public data: { post: Post; isNew: boolean; add: boolean; },
        fb: FormBuilder,
        postService: PostService

    ) {

        super(data, postService, fb);
        this.isNew = data.isNew;
        this.add = data.add;


    }


    onNoClick(): void { this.dialogRef.close(); }

    update() { this.dialogRef.close(this.frm.value); }

    cancel() { this.dialogRef.close(); }


}