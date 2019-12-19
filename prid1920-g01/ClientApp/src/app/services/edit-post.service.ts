import { Injectable, OnDestroy } from '@angular/core';
import { Post } from '../models/post';
import { Observable, of } from 'rxjs';
import { EditPostComponent } from '../components/edit-post/edit-post.component';
import { MatDialog, MatSnackBar } from '@angular/material';
import { PostService } from './post.service';
import { flatMap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })

export class EditPostService implements OnDestroy {

    constructor(
        public dialog: MatDialog,
        public postService: PostService,
        public snackBar: MatSnackBar) { }


    create(): Observable<Post> {
        const post = new Post({});

        const dlg = this.dialog.open(EditPostComponent, {
            data: { post, isNew: true }, width: '80%',
            height: '90%'
        });


        return dlg.beforeClose().pipe(
            flatMap(res => {
                if (res) {
                    return this.postService.add(res);
                } else
                    return of(null);
            })
        );
        //TODO add snack bar
        // this.snackBar.open(`There was an error at the server. The post has not been created! Please try again.`, 'Dismiss', { duration: 10000 });

    }


    addReply(idParent: number): Observable<Post> {
        const post = new Post({});

        const dlg = this.dialog.open(EditPostComponent, {
            data: { post, isNew: false }, width: '80%',
            height: '90%'
        });

        return dlg.beforeClose().pipe(
            flatMap(res => {
                if (res) {
                    return this.postService.addReply(idParent, res);
                } else
                    return of(null);
            })
        );
        //TODO add snack bar
    }



    delete(post: Post): Observable<boolean> {
        const snackBarRef = this.snackBar.open(`Post '${post.title}' will be deleted`, 'Undo', { duration: 10000 });

        return snackBarRef.afterDismissed().pipe(
            flatMap(res => {
                if(!res.dismissedByAction){
                    return this.postService.delete(post);
                }
                return of(false);
            }) 
        );
    }


    edit(post: Post): Observable<boolean> {

        const dlg = this.dialog.open(EditPostComponent, {
            data: { post, isNew: false }, width: '80%',
            height: '90%'
        });

        return dlg.beforeClose().pipe(
            flatMap(res => {
                if (res) {
                    return this.postService.update(res);
                } else
                    return of(false);
            })
        );
        //TODO add snack bar
    }

    accept(post: Post): Observable<Post> {
        const snackBarRef = this.snackBar.open(`This post will be accepted`, 'Undo', { duration: 10000 });

        return snackBarRef.afterDismissed().pipe(
            flatMap(res => {
                if(!res.dismissedByAction){
                    return this.postService.acceptPost(post);
                }
                return of(null);
            }) 
        );
    }

    unAccept(post: Post): Observable<Post> {
        const snackBarRef = this.snackBar.open(`This post will be deleted`, 'Undo', { duration: 10000 });

        return snackBarRef.afterDismissed().pipe(
            flatMap(res => {
                if(!res.dismissedByAction){
                    return this.postService.unAcceptPost(post);
                }
                return of(null);
            }) 
        );
    }

    

    ngOnDestroy(): void {
        this.snackBar.dismiss();
    }




}