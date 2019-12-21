import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Vote } from '../models/vote';
import { map, flatMap, catchError, timestamp } from 'rxjs/operators';
import { Observable, of } from 'rxjs';

@Injectable({ providedIn: 'root' })

export class VoteService {
    constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }


    public newVote(v: Vote): Observable<boolean> {

        return this.http.post<Vote>(`${this.baseUrl}api/votes`, v).pipe(
            map(res => true),
            catchError(err => {
              console.error(err);
              return of(false);
            })
          );
    }

    public delete(postId: string): Observable<boolean> {

        return this.http.delete<boolean>(`${this.baseUrl}api/votes/${postId}`).pipe(
          map(res => true),
          catchError(err => {
            console.error(err);
            return of(false);
          })
        );
      }



      public update(v: Vote): Observable<boolean> {

        return this.http.put<Vote>(`${this.baseUrl}api/votes/${v.postId}`, v).pipe(
          map(res => true),
          catchError(err => {
            console.error(err);
            return of(false);
          })
        );
      }

}
