import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AccountService } from '../services/account.service';
import { ToastrService } from '../services/toastr.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css'],
})
export class EditProfileComponent implements OnInit {
  user;
  profileUpdate: FormGroup;
  constructor(
    private fb: FormBuilder,
    public accountService: AccountService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.accountService.getProfile().subscribe((response) => {
      this.user = response;
      this.initilizeForm();
    });
  }

  initilizeForm() {
    this.profileUpdate = this.fb.group({
      id: [this.user.id],
      name: [this.user.name, Validators.required],
      userName: [
        this.user.userName,
        [Validators.required, Validators.minLength(3)],
        this.accountService.userExistAsync(false, this.user.userName),
      ],
      phoneNumber: [this.user.phoneNumber, Validators.required],
      email: [this.user.email, [Validators.email, Validators.required]],
      dateOfBirth: [new Date(this.user.dateOfBirth), Validators.required],
      gender: [this.user.gender, Validators.required],
    });
  }

  onSubmit() {
    this.accountService
      .updateProfile(this.profileUpdate.value)
      .subscribe((response) => {
        this.user = response;
        this.initilizeForm();
        this.toastr.success('Profile updated.');
      });
  }

  changePhoto(files: FileList) {
    if (files.length > 0) {
      this.accountService.changePhoto(files.item(0)).subscribe((response) => {
        this.user.photoUrl = response.photoUrl;
        this.toastr.success('Photo updated.');
      });
    }
  }

  removePhoto() {
    this.accountService.removePhoto().subscribe(() => {
      this.user.photoUrl = null;
      this.toastr.success('Photo removed.');
    });
  }
}
