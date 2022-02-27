import { Injectable } from '@angular/core';
import { Params } from '@angular/router';
import { environment } from 'src/environments/environment';
import { ResponseList } from '../base/modal';
import { LocationInfo } from '../modal/address';
import {
  BaseAgent,
  BaseAgentContext,
  StoreAgent,
  StoreAgentContext,
  TrackAgent,
  TrackAgentContext,
} from '../modal/agent';
import { BaseRole, StoreRole, TrackRole } from '../modal/role';
import { StoreInfo } from '../modal/store';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root',
})
export class AdminService {
  baseUrl = environment.apiUrl + 'admin/';
  adminUrl = this.baseUrl + 'moderate/admin-role';
  moderateTrackUrl = this.baseUrl + 'moderate/track-role';
  moderateStorekUrl = this.baseUrl + 'moderate/store-role';
  trackAdminUrl = this.baseUrl + 'track-role';
  storeAdminUrl = this.baseUrl + 'store-role';

  constructor(private http: HttpService) {}

  getModeratorsForAdmin(params: Params) {
    return this.http.get<ResponseList<BaseAgent, BaseAgentContext>>(
      this.adminUrl,
      { params: params }
    );
  }

  getTrackAgentsForModerator(params: Params) {
    return this.http.get<ResponseList<TrackAgent, TrackAgentContext>>(
      this.moderateTrackUrl,
      { params: params }
    );
  }

  getStoreAgentsForModerator(params: Params) {
    return this.http.get<ResponseList<StoreAgent, StoreAgentContext>>(
      this.moderateStorekUrl,
      { params: params }
    );
  }

  getTrackAgentsForTrackAdmin(params: Params) {
    return this.http.get<ResponseList<TrackAgent, TrackAgentContext>>(
      this.trackAdminUrl,
      { params: params }
    );
  }

  getStoreAgentsForStoreAdmin(params: Params) {
    return this.http.get<ResponseList<StoreAgent, StoreAgentContext>>(
      this.storeAdminUrl,
      { params: params }
    );
  }

  addModeratorByAdmin(role: BaseRole) {
    return this.http.post<BaseAgent>(this.adminUrl, role);
  }

  addTrackRoleByModerator(role: TrackRole) {
    return this.http.post<TrackAgent>(this.moderateTrackUrl, role);
  }

  addStoreRoleByModerator(role: StoreRole) {
    return this.http.post<StoreAgent>(this.moderateStorekUrl, role);
  }

  addRoleByTrackAdmin(role: TrackRole) {
    return this.http.post<TrackAgent>(this.trackAdminUrl, role);
  }

  addRoleByStoreAdmin(role: BaseRole) {
    return this.http.post<StoreAgent>(this.storeAdminUrl, role);
  }

  removeModeratorByAdmin(role: BaseRole) {
    return this.http.delete(this.adminUrl, role);
  }

  removeTrackRoleByModerator(role: TrackRole) {
    return this.http.delete(this.moderateTrackUrl, role);
  }

  removeStoreRoleByModerator(role: StoreRole) {
    return this.http.delete(this.moderateStorekUrl, role);
  }

  removeRoleByTrackAdmin(role: TrackRole) {
    return this.http.delete(this.trackAdminUrl, role);
  }

  removeRoleByStoreAdmin(role: StoreRole) {
    return this.http.delete(this.storeAdminUrl, role);
  }

  searchLocations(name: string, type: string, role: string) {
    return this.http.get<LocationInfo[]>(this.baseUrl + 'search-locations', {
      background: true,
      params: { name, type, for: role },
    });
  }

  searchStores(name: string, role: string) {
    return this.http.get<StoreInfo[]>(this.baseUrl + 'search-stores', {
      background: true,
      params: { name, for: role },
    });
  }
}
