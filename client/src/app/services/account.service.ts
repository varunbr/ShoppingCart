import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { BehaviorSubject, debounceTime, first, map, of, switchMap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../modal/user';

@Injectable({
  providedIn: 'root',
})
export class AccountService {
  private userSource = new BehaviorSubject<User>(null);
  user$ = this.userSource.asObservable();
  baseUrl = environment.apiUrl + 'account/';

  constructor(private http: HttpClient) {}

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'login', model).pipe(
      map((user: User) => {
        if (user) {
          this.setUser(user);
        }
      })
    );
  }

  register(model) {
    return this.http.post<User>(this.baseUrl + 'register', model).pipe(
      map((user: User) => {
        if (user) {
          this.setUser(user);
        }
      })
    );
  }

  getProfile() {
    return this.http.get<any>(this.baseUrl + 'profile');
  }

  updateProfile(body) {
    return this.http.post<any>(this.baseUrl + 'profile', body).pipe(
      map((response) => {
        this.updateToken();
        return response;
      })
    );
  }

  updateToken() {
    this.http.get<User>(this.baseUrl + 'token-update').subscribe((user) => {
      if (user) {
        this.setUser(user);
      }
    });
  }

  changePhoto(fileToUpload: File) {
    const formData: FormData = new FormData();
    formData.append('file', fileToUpload);
    return this.http.post<any>(this.baseUrl + 'change-photo', formData).pipe(
      map((response) => {
        this.updateToken();
        return response;
      })
    );
  }

  removePhoto() {
    const formData: FormData = new FormData();
    formData.append('remove', 'true');
    return this.http.post<any>(this.baseUrl + 'change-photo', formData).pipe(
      map((response) => {
        this.updateToken();
        return response;
      })
    );
  }

  logout() {
    localStorage.removeItem('user');
    this.userSource.next(null);
  }

  userExist(userName: string) {
    return this.http.get(this.baseUrl + userName, {
      headers: { Background: 'true' },
    });
  }

  userExistAsync(exist = true, currentUserName = ''): AsyncValidatorFn {
    return (control: AbstractControl) => {
      if (control?.value && control.dirty) {
        if (currentUserName === control.value) return of(null);
        return control.valueChanges
          .pipe(
            debounceTime(2000),
            switchMap((value) => this.userExist(value)),
            map((response) => {
              if (response.toString() === exist.toString()) return null;
              return { alreadyExist: response };
            })
          )
          .pipe(first());
      }
      return of(null);
    };
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
