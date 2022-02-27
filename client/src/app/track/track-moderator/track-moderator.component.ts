import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { TrackAgent, TrackAgentContext } from 'src/app/modal/agent';
import { AdminService } from 'src/app/services/admin.service';
import { ToastrService } from 'src/app/services/toastr.service';
import { UtilityService } from 'src/app/services/utility.service';

@Component({
  selector: 'app-track-moderator',
  templateUrl: './track-moderator.component.html',
  styleUrls: ['./track-moderator.component.css'],
})
export class TrackModeratorComponent implements OnInit {
  context: TrackAgentContext;
  agents: TrackAgent[];

  constructor(
    private adminService: AdminService,
    private route: ActivatedRoute,
    private router: Router,
    private toastrService: ToastrService,
    utility: UtilityService
  ) {
    utility.setTitle('Moderate - Track Roles');

    this.route.queryParams.subscribe((params) => {
      this.getRoles(params);
    });
  }

  ngOnInit(): void {}

  getRoles(params: Params) {
    this.adminService
      .getTrackAgentsForModerator(params)
      .subscribe((response) => {
        this.context = response.context;
        this.agents = response.items;
      });
  }

  removeRole(agent: TrackAgent) {
    this.adminService
      .removeTrackRoleByModerator({
        locationId: agent.locationId,
        role: agent.role,
        userId: agent.userId,
      })
      .subscribe(() => {
        this.agents.splice(this.agents.indexOf(agent), 1);
        this.toastrService.success(
          `${agent.name} is removed from role ${agent.role}`
        );
      });
  }

  applyParams(params: Params) {
    this.router.navigate(['/admin/moderate/track-role'], {
      queryParams: params,
    });
  }

  navigateToAddRole() {
    this.router.navigate(['/admin/moderate/track-role/add']);
  }
}
