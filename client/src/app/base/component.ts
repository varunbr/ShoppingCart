import { Params } from '@angular/router';
import { BaseContext, BaseModal, BaseParams } from './modal';
import { BaseListService } from './service';

export abstract class BaseListComponent<
  Modal extends BaseModal,
  Param extends BaseParams,
  Context extends BaseContext
> {
  modals: Modal[] = [];
  params: Param;
  context: Context;

  constructor(private modalService: BaseListService<Modal, Param, Context>) {}

  loadModals(params: Params) {
    this.modalService.getModals(params).subscribe((response) => {
      this.modals = response.items;
      this.context = response.context;
      console.log(response);
    });
  }

  applyFilter(params: Params) {
    //this.loadModals(params);
  }

  resetFilter() {
    this.modalService.resetParams();
    this.params = this.modalService.params;
    //this.loadModals();
  }

  pageChanged(event) {
    this.params.pageNumber = event.page;
    //this.loadModals();
  }
}
