import {
  HttpClient,
  HttpParameterCodec,
  HttpParams,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Params } from '@angular/router';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HttpService<T> {
  private paginatedResponse = new Map<string, T>();
  private modalResponse = new Map<string, any>();
  constructor(private http: HttpClient) {}

  getPaginatedResult(url: string, params: Params, cache = false) {
    let identifier = this.getIdentifier(url, params);
    let cacheResponse = this.paginatedResponse.get(identifier);
    if (cache && cacheResponse) {
      return of(cacheResponse);
    }

    return this.http
      .get<T>(url, {
        params: this.getHttpParams(params),
      })
      .pipe(
        map((response) => {
          this.paginatedResponse.set(identifier, response);
          return response;
        })
      );
  }

  get(url: string, cache = false, identifier = url) {
    let response = this.modalResponse.get(identifier);
    if (cache && response) return of(response);
    return this.http.get<any>(url).pipe(
      map((response) => {
        this.modalResponse.set(identifier, response);
        return response;
      })
    );
  }

  private getIdentifier(url: string, params: Params) {
    let identifier = url;
    for (const [key, value] of Object.entries(params)) {
      identifier = identifier + `&${key}=${value}`;
    }
    return identifier;
  }

  private getHttpParams(params: Params) {
    let httpParams = new HttpParams({ encoder: new HttpParamEncoder() });

    for (const key in params) {
      httpParams = httpParams.append(key, params[key]);
    }
    return httpParams;
  }
}

class HttpParamEncoder implements HttpParameterCodec {
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
