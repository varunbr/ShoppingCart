import { Component, OnInit } from '@angular/core';
import { StoreAgent } from 'src/app/modal/agent';
import { StoreRole } from 'src/app/modal/role';
import { AdminService } from 'src/app/services/admin.service';
import { ToastrService } from 'src/app/services/toastr.service';

@Component({
  selector: 'app-store-moderator-add',
  templateUrl: './store-moderator-add.component.html',
  styleUrls: ['./store-moderator-add.component.css'],
})
export class StoreModeratorAddComponent implements OnInit {
  agent: StoreAgent;

  constructor(
    private adminService: AdminService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  addRole(storeRole: StoreRole) {
    this.adminService
      .addStoreRoleByModerator(storeRole)
      .subscribe((response) => {
        this.agent = response;
        this.toastr.success(
          `${response.name} is added to role ${response.role}`
        );
      });
  }
}
