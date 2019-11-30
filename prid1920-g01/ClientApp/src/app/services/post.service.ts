import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Post } from '../models/post';
import { map, flatMap, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })

export class PostService {
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }


  getQuestions() {
    return this.http.get<[Post]>(`${this.baseUrl}api/posts`).pipe(
      map(res => res.map(p => new Post(p)))
    );
  }

  getQuestionsByNewest() {
    return this.http.get<[Post]>(`${this.baseUrl}api/posts`).pipe(
      map(res => res.map(p => new Post(p)).sort)
    );
  }


  public update(p: Post): Observable<boolean> {
    return this.http.put<Post>(`${this.baseUrl}api/posts/${p.id}`, p).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

  public delete(p: Post): Observable<boolean> {

    return this.http.delete<boolean>(`${this.baseUrl}api/posts/${p.id}`).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

  public add(p: Post): Observable<boolean> {

    return this.http.post<Post>(`${this.baseUrl}api/posts`, p).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

  public addNewPost(p: Post): Observable<boolean> {

    return this.http.post<Post>(`${this.baseUrl}api/posts/newPost`, p).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

}
