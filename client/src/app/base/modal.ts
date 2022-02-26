export class BaseModal {
  id: number = 0;
}

export class BaseContext {
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  orderBy: string;
}

export interface ResponseList<
  Modal extends BaseModal,
  Context extends BaseContext
> {
  context: Context;
  items: Modal[];
}
