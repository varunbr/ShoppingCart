import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, NgForm, Validators } from '@angular/forms';
import { finalize } from 'rxjs';
import { Address, LocationInfo } from '../modal/address';
import { AccountService } from '../services/account.service';
import { ToastrService } from '../services/toastr.service';
import { UtilityService } from '../services/utility.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css'],
})
export class EditProfileComponent implements OnInit {
  user;
  profileUpdate: UntypedFormGroup;
  address: Address = new Address();
  states: LocationInfo[];
  cities: LocationInfo[];
  areas: LocationInfo[];
  loading: string;
  constructor(
    private fb: UntypedFormBuilder,
    public accountService: AccountService,
    private toastr: ToastrService,
    utility: UtilityService
  ) {
    utility.setTitle('Edit profile');
  }

  ngOnInit(): void {
    this.accountService.getProfile().subscribe((response) => {
      this.user = response;
      this.initilizeProfileForm();
    });
    this.accountService.getAddress().subscribe((response) => {
      this.initilizeAddressForm(response);
    });
  }

  initilizeProfileForm() {
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

  initilizeAddressForm(response: Address) {
    this.address = response;
    this.areas = response.locations.areas;
    this.cities = response.locations.cities;
    this.states = response.locations.states;
  }

  loadLocations(parentId: number, childType: string) {
    this.loading = childType;
    this.accountService
      .getLocations({
        parentId: parentId,
        childType: childType,
      })
      .pipe(
        finalize(() => {
          this.loading = undefined;
        })
      )
      .subscribe((response) => {
        switch (childType) {
          case 'City':
            this.cities = response;
            break;
          case 'Area':
            this.areas = response;
            break;
        }
      });
  }

  onSelect(event, childType: string) {
    let value = event.source.value;
    switch (childType) {
      case 'City':
        if (!event.isUserInput || value === this.address.stateId) return;
        this.areas = this.cities = [];
        this.address.areaId = this.address.cityId = undefined;
        break;
      case 'Area':
        if (!event.isUserInput || value === this.address.cityId) return;
        this.areas = [];
        this.address.areaId = undefined;
        break;
    }
    this.loading = childType;
    this.loadLocations(value, childType);
  }

  onSubmit() {
    this.accountService
      .updateProfile(this.profileUpdate.value)
      .subscribe((response) => {
        this.user = response;
        this.initilizeProfileForm();
        this.toastr.success('Profile updated.');
      });
  }

  updateAddress(form: NgForm) {
    this.accountService.updateAddress(this.address).subscribe((response) => {
      this.initilizeAddressForm(response);
      form.resetForm(response);
      this.toastr.success('Address updated.');
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
