import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { finalize } from 'rxjs';
import { LocationInfo } from 'src/app/modal/address';
import { TrackAgent } from 'src/app/modal/agent';
import { TrackRole } from 'src/app/modal/role';
import { UserInfo } from 'src/app/modal/user';
import { AdminService } from 'src/app/services/admin.service';

@Component({
  selector: 'app-add-track-role',
  templateUrl: './add-track-role.component.html',
  styleUrls: ['./add-track-role.component.css'],
})
export class AddTrackRoleComponent implements OnInit {
  @Output() addRole = new EventEmitter<TrackRole>();
  @Input() for: string;
  private _agent: TrackAgent;
  @Input()
  public get agent(): TrackAgent {
    return this._agent;
  }
  public set agent(value: TrackAgent) {
    this.initilize();
    this._agent = value;
  }
  userName = '';
  user: UserInfo;
  locations: LocationInfo[];
  trackRole = new TrackRole();
  location: string;
  locationType = 'Area';
  locationTypes = ['Area', 'City', 'State', 'Country'];
  roles = ['TrackAgent', 'TrackAdmin'];
  loading = false;

  constructor(private adminService: AdminService) {}

  ngOnInit(): void {}

  initilize() {
    this.userName = '';
    this.location = '';
    this.locations = [];
    this.trackRole = new TrackRole();
  }

  searchLocation() {
    this.locations = [];
    if (this.location?.length >= 3) {
      this.loading = true;
      this.adminService
        .searchLocations(this.location, this.locationType, this.for)
        .pipe(
          finalize(() => {
            this.loading = false;
          })
        )
        .subscribe((response) => {
          this.locations = response;
        });
    }
  }

  userChange(user: UserInfo) {
    this.user = user;
    this.trackRole.userId = user?.id;
  }

  onLocationTypeChange() {
    this.trackRole.locationId = null;
    this.locations = [];
  }

  onSubmit() {
    this.addRole.emit(this.trackRole);
  }
}
