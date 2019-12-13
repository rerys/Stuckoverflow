import { Injectable, OnDestroy } from '@angular/core';
import { Observable, of } from 'rxjs';
import { MatDialog, MatSnackBar } from '@angular/material';
import { CommentService } from './comment.service';
import { Comment } from '../models/comment';
import { EditCommentComponent } from '../components/edit-comment/edit-comment.component';
import { flatMap } from 'rxjs/operators';


@Injectable({ providedIn: 'root' })

export class EditCommentService implements OnDestroy {

    constructor(
        public dialog: MatDialog,
        public commentService: CommentService,
        public snackBar: MatSnackBar) { }



    create(postId: string): Observable<Comment> {
        const comment = new Comment({});
        comment.postId = postId;
        const dlg = this.dialog.open(EditCommentComponent, {
            data: { comment, isNew: true }, width: '80%',
            height: '90%'
        });

        return dlg.beforeClose().pipe(
            flatMap(res => {
                if (res) {
                    return this.commentService.add(res);
                } else
                    return of(null);
            })
        );


    }

    

    delete(c: Comment): Observable<boolean> {
        const snackBarRef = this.snackBar.open(`Comment will be deleted`, 'Undo', { duration: 10000 });

        return snackBarRef.afterDismissed().pipe(
            flatMap(res => {
                if(!res.dismissedByAction){
                    return this.commentService.delete(c);
                }
                return of(false); 
            }) 
        );
    } 

    edit(comment: Comment): Observable<boolean> {

        const dlg = this.dialog.open(EditCommentComponent, {
            data: { comment, isNew: false }, width: '80%',
            height: '90%'
        });

        return dlg.beforeClose().pipe(
            flatMap(res => {
                if (res) {
                    return this.commentService.update(res);
                } else
                    return of(false);
            })
        );
 
        //TODO add snack bar
 
        //this.snackBar.open(`There was an error at the server. The update has not been done! Please try again.`, 'Dismiss', { duration: 10000 });

    }



    ngOnDestroy(): void {
        this.snackBar.dismiss();
    }
}
