import { Component, OnInit } from '@angular/core';
import { StoreAgent } from 'src/app/modal/agent';
import { StoreRole } from 'src/app/modal/role';
import { AdminService } from 'src/app/services/admin.service';
import { ToastrService } from 'src/app/services/toastr.service';

@Component({
  selector: 'app-store-admin-add',
  templateUrl: './store-admin-add.component.html',
  styleUrls: ['./store-admin-add.component.css'],
})
export class StoreAdminAddComponent implements OnInit {
  agent: StoreAgent;

  constructor(
    private adminService: AdminService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  addRole(storeRole: StoreRole) {
    this.adminService.addRoleByStoreAdmin(storeRole).subscribe((response) => {
      this.agent = response;
      this.toastr.success(`${response.name} is added to role ${response.role}`);
    });
  }
}
