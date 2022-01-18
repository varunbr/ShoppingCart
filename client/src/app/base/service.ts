import { Params } from '@angular/router';
import { HttpService } from '../services/http.service';
import { BaseContext, BaseModal, ResponseList } from './modal';

export abstract class BaseListService<
  Modal extends BaseModal,
  Context extends BaseContext
> {
  abstract baseUrl: string;

  constructor(public http: HttpService<ResponseList<Modal, Context>>) {}

  getModals(params: Params, cache = false) {
    return this.http.getPaginatedResult(this.baseUrl, params, true);
  }
}
