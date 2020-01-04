import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, flatMap, catchError } from 'rxjs/operators';
import { Observable, of } from 'rxjs';
import { Tag } from '../models/tag';

@Injectable({ providedIn: 'root' })

export class TagService {

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) { }

  getAll() {
    return this.http.get<Tag[]>(`${this.baseUrl}api/tags`).pipe(
        map(res => res.map(t => new Tag(t)))
      );
  }

  find(filter: string) {
    return this.http.get<Tag[]>(`${this.baseUrl}api/tags/find/${filter}`).pipe(
        map(res => res.map(t => new Tag(t)))
      );
  }

  getByName(name: string) {

    return this.http.get<Tag>(`${this.baseUrl}api/tags/${name}`).pipe(
      map(t => !t ? null : new Tag(t)),
      catchError(err => of(null))
    );
  }


  public update(t: Tag): Observable<boolean> {

    return this.http.put<Tag>(`${this.baseUrl}api/tags/${t.id}`, t).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

  public delete(t: Tag): Observable<boolean> {

    return this.http.delete<boolean>(`${this.baseUrl}api/tags/${t.id}`).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

  public add(t: Tag): Observable<boolean> {

    return this.http.post<Tag>(`${this.baseUrl}api/tags`, t).pipe(
      map(res => true),
      catchError(err => {
        console.error(err);
        return of(false);
      })
    );
  }

}