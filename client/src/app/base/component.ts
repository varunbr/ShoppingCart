import { Params } from '@angular/router';
import { BaseContext, BaseModal } from './modal';
import { BaseListService } from './service';

export abstract class BaseListComponent<
  Modal extends BaseModal,
  Context extends BaseContext
> {
  items: Modal[];
  context: Context;

  constructor(private modalService: BaseListService<Modal, Context>) {}

  loadItems(params: Params, cache = true) {
    this.modalService.getModals(params, cache).subscribe((response) => {
      this.items = response.items;
      this.context = response.context;
    });
  }
}
