import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Post } from '../models/post';
import { map, flatMap, catchError, timestamp } from 'rxjs/operators';
import { Observable, of, Subject } from 'rxjs';
import { Router } from "@angular/router";

@Injectable({ providedIn: 'root' })

export class PostService {

  question: Post;
  questionSubject = new Subject<Post>();

  accepted: Post;
  acceptedSubject = new Subject<Post>();

  responses: Post[] = [];
  responsesSubject = new Subject<Post[]>();


  public emitAllResponses() {
    this.responsesSubject.next(this.responses.slice());
  }

  public emitQuestion() {
    this.questionSubject.next(this.question);
  }

  public emitAccepted() {
    this.acceptedSubject.next(this.accepted);
  }

  public emitPost() {
    this.responsesSubject.next(this.responses);
  }


  public getRefrechPost(id: number) {

    this.getQuestionById(id).subscribe(post => {
      if(post != null){
        this.question = post;
        this.responses = post.responses;
        this.accepted = post.accepted;
        this.emitQuestion();
        this.emitAccepted();
        this.emitPost();
      }
       else{
        this.router.navigate(['/posts'])
       }
    
    });

  }



  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string,private router: Router) { }


  getQuestions(filter: string) {
    return this.http.get<[Post]>(`${this.baseUrl}api/posts/${filter}`).pipe(
      map(res => res.map(p => new Post(p)))
    );
  }

  getQuestionsByNewest(filter: string) {
    return this.http.get<[Post]>(`${this.baseUrl}api/posts/newest/${filter}`).pipe(
      map(res => res.map(p => new Post(p)))
    );
  }

  getQuestionsByVotes(filter: string) {
    return this.http.get<[Post]>(`${this.baseUrl}api/posts/votes/${filter}`).pipe(
      map(res => res.map(p => new Post(p)))
    );
  }

  getQuestionsByUnanswered(filter: string) {
    return this.http.get<[Post]>(`${this.baseUrl}api/posts/unanswered/${filter}`).pipe(
      map(res => res.map(p => new Post(p)))
    );
  }

  getQuestionsByTags(filter: string) {
    return this.http.get<[Post]>(`${this.baseUrl}api/posts/tags/${filter}`).pipe(
      map(res => res.map(p => new Post(p)))
    );
  }

  getQuestionById(id: number) {
    return this.http.get<Post>(`${this.baseUrl}api/posts/question/${id}`).pipe(
      map(p => !p ? null : new Post(p)),
      catchError(err => of(null))
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


  public add(p: Post): Observable<Post> {

    return this.http.post<Post>(`${this.baseUrl}api/posts`, p).pipe(
      map(res => !res ? null : new Post(res)),
      catchError(err => {
        console.error(err);
        return of(null);
      }
      )
    );
  }

  public addReply(idParent: number, p: Post): Observable<Post> {

    return this.http.post<Post>(`${this.baseUrl}api/posts/${idParent}`, p).pipe(
      map(res => !res ? null : new Post(res)),
      catchError(err => {
        console.error(err);
        return of(null);
      }
      )
    );
  }

  public acceptPost(response: Post): Observable<Post> {

    return this.http.get<Post>(`${this.baseUrl}api/posts/acceptPost/${response.id}`).pipe(
      map(res => !res ? null : new Post(res)),
      catchError(err => {
        console.error(err);
        return of(null);
      }
      )
    );
  }

  public unAcceptPost(response: Post): Observable<Post> {

    return this.http.get<Post>(`${this.baseUrl}api/posts/unAcceptPost/${response.id}`).pipe(
      map(res => !res ? null : new Post(res)),
      catchError(err => {
        console.error(err);
        return of(null);
      }
      )
    );
  }

}
