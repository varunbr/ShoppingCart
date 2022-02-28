import { Component, OnInit } from '@angular/core';
import { BaseRole } from 'src/app/modal/role';
import { UserInfo } from 'src/app/modal/user';
import { AdminService } from 'src/app/services/admin.service';
import { ToastrService } from 'src/app/services/toastr.service';

@Component({
  selector: 'app-add-admin-role',
  templateUrl: './add-admin-role.component.html',
  styleUrls: ['./add-admin-role.component.css'],
})
export class AddAdminRoleComponent implements OnInit {
  roles = ['TrackModerator', 'StoreModerator', 'Admin'];
  adminRole = new BaseRole();
  userName = '';
  user: UserInfo;

  constructor(
    private adminService: AdminService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  userChange(user: UserInfo) {
    this.user = user;
    this.adminRole.userId = user?.id;
  }

  onSubmit() {
    this.adminService
      .addModeratorByAdmin(this.adminRole)
      .subscribe((response) => {
        this.userName = '';
        this.toastr.success(
          `${response.name} is added to role ${response.role}`
        );
      });
  }
}
