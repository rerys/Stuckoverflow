import { Component, Inject, Input } from "@angular/core";
import { MatDialogRef,MAT_DIALOG_DATA } from "@angular/material";
import { Comment } from 'src/app/models/comment';

@Component({
  selector: 'edit-comment',
  templateUrl: './edit-comment.component.html',
  styleUrls: ['./edit-comment.component.css']
})

export class EditCommentComponent {

  @Input() comment = this.data.comment;


  options: any = {
    autoScrollEditorIntoView: true,
    maxLines: 28,
    showLineNumbers: true
  };



  constructor(public dialogRef: MatDialogRef<EditCommentComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { comment: Comment; isNew: boolean; },
  ) {
    console.log(data.comment);
  }


  onNoClick(): void { this.dialogRef.close(); }

  save() { this.dialogRef.close(this.data.comment); }

  cancel() { this.dialogRef.close(); }

  canActivate() {
    return !this.isEmpty(this.comment.body);
  }

  private isEmpty(value: string): boolean {
    if (value == null || value == undefined || value.length == 0) return true;
    return false;
  }

}