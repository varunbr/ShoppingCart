import { Params } from '@angular/router';
import { BaseContext, BaseModal } from './modal';
import { BaseListService } from './service';

export abstract class BaseListComponent<
  Modal extends BaseModal,
  Context extends BaseContext
> {
  modals: Modal[];
  context: Context;

  constructor(private modalService: BaseListService<Modal, Context>) {}

  loadModals(params: Params) {
    this.modalService.getModals(params).subscribe((response) => {
      this.modals = response.items;
      this.context = response.context;
    });
  }
}