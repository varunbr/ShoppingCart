export class BaseModal {
  id: number = 0;
}
export abstract class BaseParams {
  pageNumber = 1;
  pageSize = 12;
  orderBy: string;
}

export interface BaseContext {
  pageNumber: string;
  pageSize: string;
  totalPages: string;
  totalCount: string;
  orderBy: string;
}

export interface ResponseList<
  Modal extends BaseModal,
  Context extends BaseContext
> {
  context: Context;
  items: Modal[];
}
