import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Post } from '../models/post';
import { map, flatMap, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })

export class PostService{
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }


    getQuestions() {
        return this.http.get<[Post]>(`${this.baseUrl}api/posts`).pipe(
          map(res => res.map(p => new Post(p)))
        );
      }
}
