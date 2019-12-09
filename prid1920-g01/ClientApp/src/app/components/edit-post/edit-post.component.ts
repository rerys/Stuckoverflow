import { Component, OnInit, Input, Inject } from '@angular/core';
import { PostService } from 'src/app/services/post.service';
import { Post } from 'src/app/models/post';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { EditPostService } from 'src/app/services/edit-post.service';


@Component({
  selector: 'edit-post',
  templateUrl: './edit-post.component.html',
  styleUrls: ['./edit-post.component.css']
})
export class EditPostComponent {

  @Input() post = this.data.post;
  new: boolean = true;
  options: any = {
    autoScrollEditorIntoView: true,
    maxLines: 28,
    showLineNumbers: true
  };

  requiredTitle: boolean;

  constructor(public dialogRef: MatDialogRef<EditPostComponent>,

    @Inject(MAT_DIALOG_DATA) public data: { post: Post; isNew: boolean; },
    private postService: PostService,
    private route: ActivatedRoute,
    //public editPostService: EditPostService

  ) {

    this.requiredTitle = this.titleIsRequired();
  }

  private titleIsRequired() {
    return this.data.isNew || this.data.post.title != null
  }


  canActivate() {
    let act = true;
    if (this.requiredTitle) { if (this.isEmpty(this.data.post.title)) act = false; }
    if (this.isEmpty(this.data.post.body)) act = false;
    return act;
  }

  onNoClick(): void { this.dialogRef.close(); }

  save() { this.dialogRef.close(this.data.post); }

  cancel() { this.dialogRef.close(); }

  private isEmpty(value: string): boolean {
    if (value == null || value == undefined || value.length == 0) return true;
    return false;
  }


}
