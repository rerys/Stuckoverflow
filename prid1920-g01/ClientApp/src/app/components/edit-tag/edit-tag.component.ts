import { Component, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Inject } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { FormControl } from '@angular/forms';
import { FormBuilder } from '@angular/forms';
import { Validators } from '@angular/forms';
import * as _ from 'lodash';
import { Tag } from 'src/app/models/tag';
import { TagService } from 'src/app/services/tag.service';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { Role } from 'src/app/models/user';


@Component({
    selector: 'app-edit-tag-mat',
    templateUrl: './edit-tag.component.html',
    styleUrls: ['./edit-tag.component.css']
})

export class EditTagComponent {

    public frm: FormGroup;
    public ctlName: FormControl; 

    public isNew: boolean;

    constructor(public dialogRef: MatDialogRef<EditTagComponent>,

        @Inject(MAT_DIALOG_DATA) public data: { tag: Tag; isNew: boolean; },
        private fb: FormBuilder,
        private tagService: TagService

    ) {

        this.ctlName = this.fb.control('', [
            Validators.required
        ], [this.nameUsed()]);
 


        this.frm = this.fb.group({
            name: this.ctlName
        });

        this.isNew = data.isNew;
        this.frm.patchValue(data.tag);

    }

    nameUsed(): any {

        let timeout: NodeJS.Timer;

        return (ctl: FormControl) => {

            clearTimeout(timeout);

            const name = ctl.value;

            return new Promise(resolve => {
                timeout = setTimeout(() => {
                    if (ctl.pristine) {
                        resolve(null);
                    } else {
                        this.tagService.getByName(name).subscribe(tag => {
                            resolve(tag ? { nameUsed: true } : null);
                        });
                    }
                }, 300);
            });
        };
    }

    onNoClick(): void { this.dialogRef.close(); }
    update() { this.dialogRef.close(this.frm.value); }
    cancel() { this.dialogRef.close(); }

}