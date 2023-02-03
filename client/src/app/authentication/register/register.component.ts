import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  UntypedFormBuilder,
  UntypedFormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
})
export class RegisterComponent implements OnInit {
  registerForm: UntypedFormGroup;
  redirectUrl: string;
  constructor(
    private fb: UntypedFormBuilder,
    private accountService: AccountService,
    private router: Router,
    private route: ActivatedRoute,
    utility: UtilityService
  ) {
    utility.setTitle('Create New Account');
    this.initilizeForm();
  }

  ngOnInit(): void {
    this.redirectUrl = this.route.snapshot.queryParams.redirectTo;
    if (this.accountService.loggedIn) {
      this.redirect();
    }
  }

  initilizeForm() {
    this.registerForm = this.fb.group({
      name: ['', Validators.required],
      userName: [
        '',
        [Validators.required, Validators.minLength(3)],
        this.accountService.userExistAsync(false),
      ],
      phoneNumber: ['', Validators.required],
      email: ['', [Validators.email, Validators.required]],
      dateOfBirth: ['', Validators.required],
      gender: [, Validators.required],
      password: [
        '',
        [
          Validators.required,
          Validators.minLength(6),
          Validators.maxLength(12),
        ],
      ],
      confirmPassword: [
        '',
        [Validators.required, this.matchValues('password')],
      ],
    });

    this.registerForm.controls.password.valueChanges.subscribe(() => {
      this.registerForm.controls.confirmPassword.updateValueAndValidity();
    });
  }

  matchValues(name: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      return control?.value === control?.parent?.controls[name]?.value
        ? null
        : { notMatching: true };
    };
  }

  redirect() {
    this.router.navigateByUrl(this.redirectUrl ? this.redirectUrl : '/', {
      replaceUrl: true,
    });
  }

  onSubmit() {
    this.accountService.register(this.registerForm.value).subscribe(() => {
      location.reload();
    });
  }
}
