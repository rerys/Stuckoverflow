import { Injectable, OnDestroy } from '@angular/core';
import { Observable, of } from 'rxjs';
import { MatDialog, MatSnackBar } from '@angular/material';
import { CommentService } from './comment.service';
import { Comment } from '../models/comment';
import { EditCommentComponent } from '../components/edit-comment/edit-comment.component';

@Injectable({ providedIn: 'root' })

export class EditCommentService implements OnDestroy {
    postService: any;

    constructor(
        public dialog: MatDialog,
        public commentService: CommentService,
        public snackBar: MatSnackBar) { }



    create(postId: string): Observable<boolean> {
        var comment = new Comment({});
        comment.postId = postId;
        const dlg = this.dialog.open(EditCommentComponent, {
            data: { comment, isNew: true }, width: '80%',
            height: '90%'
        });

        dlg.beforeClose().subscribe(res => {

            if (res) {
                return this.commentService.add(res).subscribe(res => {
                    if (!res) {
                        this.snackBar.open(`There was an error at the server. The post has not been created! Please try again.`, 'Dismiss', { duration: 10000 });
                    }
                    return of(true);
                });
            }
        });
        return of(false);
    }

    

    delete(c: Comment): Observable<boolean> {
        const snackBarRef = this.snackBar.open(`Comment will be deleted`, 'Undo', { duration: 10000 });
        snackBarRef.afterDismissed().subscribe(res => {
            if (!res.dismissedByAction) { return this.commentService.delete(c).subscribe(); }

        });
        return of(false);
    } 

    edit(comment: Comment): Observable<boolean> {

        const dlg = this.dialog.open(EditCommentComponent, {
            data: { comment, isNew: false }, width: '80%',
            height: '90%'
        });
 
        dlg.beforeClose().subscribe(res => {
            if (res) {
                return this.commentService.update(res).subscribe(res => {
                    if (!res) {
                        this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`, 'Dismiss', { duration: 10000 });
                    }
                });
            }
        });
        return of(false);
    }



    ngOnDestroy(): void {
        this.snackBar.dismiss();
    }
}
