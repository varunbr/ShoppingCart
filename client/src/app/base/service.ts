import { HttpClient } from '@angular/common/http';
import { Params } from '@angular/router';
import { map, of } from 'rxjs';
import { getPaginatedResult } from './helper';
import { BaseContext, BaseModal, BaseParams, ResponseList } from './modal';

export abstract class BaseListService<
  Modal extends BaseModal,
  Param extends BaseParams,
  Context extends BaseContext
> {
  private cache: boolean;
  private modalCache = new Map();
  private modalMapCache = new Map();
  params: Param;
  abstract baseUrl: string;

  constructor(public http: HttpClient, enableCache: boolean = true) {
    this.cache = enableCache;
    this.resetParams();
  }

  getModals(params: Params) {
    if (false && this.cache) {
      //To-Do
      var response = this.modalMapCache.get(this.params.getIdentifier());
      if (response) {
        return this.getCacheData(response);
      }
    }

    return getPaginatedResult<Modal, Context>(
      this.http,
      this.baseUrl,
      params
    ).pipe(
      map((response) => {
        this.updateCacheData(response);
        return response;
      })
    );
  }

  getModal(url: string, id) {
    var modal = this.modalCache.get(id);
    if (modal) {
      return of(this.modalCache.get(id));
    }

    return this.http.get<Modal>(url + '/' + id).pipe(
      map((response) => {
        this.cacheModal(response);
        return response;
      })
    );
  }

  updateCacheData(response: ResponseList<Modal, Context>) {
    if (this.cache) {
      this.modalMapCache.set(this.params.getIdentifier(), response);
      response.items?.forEach((element) => {
        this.modalCache.set(element.id, element);
      });
    }
  }

  cacheModal(modal: Modal) {
    if (this.cache) {
      this.modalCache.set(modal.id, modal);
    }
  }

  getCacheData(response: ResponseList<Modal, Context>) {
    let items = response.items;
    for (let i = 0; i < items.length; i++) {
      items[i] = this.modalCache.get(items[i].id);
    }
    return of(response);
  }

  abstract resetParams();
  abstract getQueryString(): string;
}
