import { Injectable, OnDestroy } from '@angular/core';
import { Post } from '../models/post';
import { Observable, of } from 'rxjs';
import { EditPostComponent } from '../components/edit-post/edit-post.component';
import { MatDialog, MatSnackBar } from '@angular/material';
import { PostService } from './post.service';

@Injectable({ providedIn: 'root' })

export class EditPostService implements OnDestroy {

    constructor(
        public dialog: MatDialog,
        public postService: PostService,
        public snackBar: MatSnackBar) {}
        

    create(): Observable<boolean> {
        const post = new Post({});

        const dlg = this.dialog.open(EditPostComponent, {
            data: { post, isNew: true }, width: '80%',
            height: '90%'
        });

        dlg.beforeClose().subscribe(res => {

            if (res) {
                return this.postService.add(res).subscribe(res => {
                    if (!res) {
                        this.snackBar.open(`There was an error at the server. The member has not been created! Please try again.`, 'Dismiss', { duration: 10000 });
                    }
                });
            }
        });
        return of(false);
    }



    delete(post: Post): Observable<boolean> {
        const snackBarRef = this.snackBar.open(`Post '${post.title}' will be deleted`, 'Undo', { duration: 10000 });
        snackBarRef.afterDismissed().subscribe(res => {
            if (!res.dismissedByAction) { return this.postService.delete(post).subscribe(); }

        });
        return of(false);
    }


    edit(post: Post): Observable<boolean> {

        const dlg = this.dialog.open(EditPostComponent, {
            data: { post, isNew: false }, width: '80%',
            height: '90%'
        });

        dlg.beforeClose().subscribe(res => {
            if (res) {
                return this.postService.update(res).subscribe(res => {
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