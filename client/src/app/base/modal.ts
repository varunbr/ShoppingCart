export class BaseModal {
  id: number = 0;
}

export class BaseContext {
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
