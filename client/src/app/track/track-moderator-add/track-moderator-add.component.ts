import { Component, OnInit } from '@angular/core';
import { TrackAgent } from 'src/app/modal/agent';
import { TrackRole } from 'src/app/modal/role';
import { AdminService } from 'src/app/services/admin.service';
import { ToastrService } from 'src/app/services/toastr.service';

@Component({
  selector: 'app-track-moderator-add',
  templateUrl: './track-moderator-add.component.html',
  styleUrls: ['./track-moderator-add.component.css'],
})
export class TrackModeratorAddComponent implements OnInit {
  agent: TrackAgent;

  constructor(
    private adminService: AdminService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {}

  addRole(trackRole: TrackRole) {
    this.adminService
      .addTrackRoleByModerator(trackRole)
      .subscribe((response) => {
        this.agent = response;
        this.toastr.success(
          `${response.name} is added to role ${response.role}`
        );
      });
  }
}
