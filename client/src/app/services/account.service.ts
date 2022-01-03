import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../modal/user';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private userSource = new BehaviorSubject<User>(null);
  user$ = this.userSource.asObservable();
  baseUrl = environment.apiUrl + 'account';

  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http
      .post<User>(environment.apiUrl + 'account/login', model)
      .pipe(
        map((user: User) => {
          if (user) {
            this.setUser(user);
          }
        })
      );
  }

  logout() {
    localStorage.removeItem('user');
    this.userSource.next(null);
  }

  setUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? (user.roles = roles) : (user.roles = [roles]);
    localStorage.setItem('user', JSON.stringify(user));
    //this.userSource.next(null);
    this.userSource.next(user);
  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }
}
