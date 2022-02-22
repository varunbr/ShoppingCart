import { Params } from '@angular/router';
import { HttpService } from '../services/http.service';
import { BaseContext, BaseModal, ResponseList } from './modal';

export abstract class BaseListService<
  Modal extends BaseModal,
  Context extends BaseContext
> {
  abstract baseUrl: string;

  constructor(public http: HttpService) {}

  getModals(params: Params, cache = true) {
    return this.http.get<ResponseList<Modal, Context>>(this.baseUrl, {
      params,
      cache,
    });
  }

  getModal(id: number) {
    return this.http.get<Modal>(this.baseUrl + id);
  }
}
