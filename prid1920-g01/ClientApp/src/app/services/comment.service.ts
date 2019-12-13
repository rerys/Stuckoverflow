import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Comment } from '../models/comment';
import { map, flatMap, catchError, timestamp } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })

export class CommentService{
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }



    public update(c: Comment): Observable<boolean> {
        return this.http.put<Comment>(`${this.baseUrl}api/comments/${c.id}`, c).pipe(
          map(res => true),
          catchError(err => { 
            console.error(err);
            return of(false);
          })
        );
      }
    
      public delete(c: Comment): Observable<boolean> {
        return this.http.delete<boolean>(`${this.baseUrl}api/comments/${c.id}`).pipe(
          map(res => true),
          catchError(err => { 
            console.error(err);
            return of(false);
          })
        );
      }
    
      public add(c: Comment): Observable<Comment> {
        return this.http.post<Comment>(`${this.baseUrl}api/comments`, c).pipe(
          map(res => !res? null : new Comment(res)),
          catchError(err => 
            of(null)
          )
        );
      }
}