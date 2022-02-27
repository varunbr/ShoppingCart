import { Component, OnInit } from '@angular/core';
import { TrackAgent } from 'src/app/modal/agent';
import { TrackRole } from 'src/app/modal/role';
import { AdminService } from 'src/app/services/admin.service';
import { ToastrService } from 'src/app/services/toastr.service';

@Component({
  selector: 'app-track-admin-add',
  templateUrl: './track-admin-add.component.html',
  styleUrls: ['./track-admin-add.component.css'],
})
export class TrackAdminAddComponent implements OnInit {
  agent: TrackAgent;
  constructor(
    private adminService: AdminService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  addRole(trackRole: TrackRole) {
    this.adminService.addRoleByTrackAdmin(trackRole).subscribe((response) => {
      this.agent = response;
      this.toastr.success(`${response.name} is added to role ${response.role}`);
    });
  }
}
