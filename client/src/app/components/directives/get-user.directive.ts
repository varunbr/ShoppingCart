import { Directive, EventEmitter, Output } from '@angular/core';
import {
  AbstractControl,
  AsyncValidator,
  NG_ASYNC_VALIDATORS,
  ValidationErrors,
} from '@angular/forms';
import { debounceTime, first, map, Observable, of, switchMap } from 'rxjs';
import { UserInfo } from 'src/app/modal/user';
import { AccountService } from 'src/app/services/account.service';

@Directive({
  selector: '[appGetUser]',
  providers: [
    {
      provide: NG_ASYNC_VALIDATORS,
      useExisting: GetUserDirective,
      multi: true,
    },
  ],
})
export class GetUserDirective implements AsyncValidator {
  @Output() userChange = new EventEmitter<UserInfo>();

  constructor(private accountService: AccountService) {}

  validate(control: AbstractControl): Observable<ValidationErrors> {
    let fn = (control: AbstractControl) => {
      this.userChange.emit(null);
      if (control?.value && control.dirty) {
        return control.valueChanges
          .pipe(
            debounceTime(2000),
            switchMap((value) => this.accountService.getUserInfo(value)),
            map((response) => {
              this.userChange.emit(response);
              return response.exist ? null : { alreadyExist: response?.exist };
            })
          )
          .pipe(first());
      }
      return of(null);
    };
    return fn(control);
  }
}
