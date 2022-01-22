import { Directive, Input } from '@angular/core';
import {
  AbstractControl,
  AsyncValidator,
  NG_ASYNC_VALIDATORS,
  ValidationErrors,
} from '@angular/forms';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/services/account.service';

@Directive({
  selector: '[appUserExist]',
  providers: [
    {
      provide: NG_ASYNC_VALIDATORS,
      useExisting: UserExistDirective,
      multi: true,
    },
  ],
})
export class UserExistDirective implements AsyncValidator {
  @Input('appUserNameValidator') exist = true;
  constructor(private accountService: AccountService) {}
  validate(
    control: AbstractControl
  ): Promise<ValidationErrors> | Observable<ValidationErrors> {
    return this.accountService.userExistAsync(this.exist)(control);
  }
}
