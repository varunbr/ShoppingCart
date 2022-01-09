import {
  HttpClient,
  HttpParameterCodec,
  HttpParams,
} from '@angular/common/http';
import { Params } from '@angular/router';
import { map } from 'rxjs';
import { BaseContext, BaseModal, ResponseList } from './modal';

export function getPaginatedResult<M extends BaseModal, C extends BaseContext>(
  http: HttpClient,
  url: string,
  params: Params
) {
  return http
    .get<ResponseList<M, C>>(url, {
      observe: 'response',
      params: getHttpParams(params),
    })
    .pipe(
      map((response) => {
        return response.body;
      })
    );
}

export function getHttpParams(params: Params) {
  let httpParams = new HttpParams({ encoder: new HttpParamEncoder() });

  for (const key in params) {
    httpParams = httpParams.append(key, params[key]);
  }
  return httpParams;
}

export class HttpParamEncoder implements HttpParameterCodec {
  encodeKey(key: string): string {
    return encodeURIComponent(key);
  }
  encodeValue(value: string): string {
    return encodeURIComponent(value);
  }
  decodeKey(key: string): string {
    return decodeURIComponent(key);
  }
  decodeValue(value: string): string {
    return decodeURIComponent(value);
  }
}
