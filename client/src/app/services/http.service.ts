import {
  HttpClient,
  HttpParameterCodec,
  HttpParams,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Params } from '@angular/router';
import { map, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  private responseCache = new Map<string, any>();
  constructor(private http: HttpClient) {}

  get<T>(
    url: string,
    { params = {}, cache = false, background = false } = {}
  ): Observable<T> {
    let identifier = this.getIdentifier(url, params);
    let cacheResponse = this.responseCache.get(identifier);
    if (cache && cacheResponse) {
      return of(cacheResponse);
    }

    return this.http
      .get<T>(url, {
        params: this.getHttpParams(params),
        headers: { Background: background.toString() },
      })
      .pipe(
        map((response) => {
          this.setCache(response, url, params, cache);
          return response;
        })
      );
  }

  post<T>(url: string, body = {}, { background = false } = {}) {
    return this.http.post<T>(url, body, {
      headers: { Background: background.toString() },
    });
  }

  delete<T>(url: string, body = {}, { background = false } = {}) {
    return this.http.delete<T>(url, {
      body,
      responseType: 'json',
      headers: { Background: background.toString() },
    });
  }

  setCache<T>(response: T, url: string, params: Params, cache = true) {
    if (!cache) return;
    let identifier = this.getIdentifier(url, params);
    this.responseCache.set(identifier, response);
  }

  private getIdentifier(url: string, params: Params) {
    let identifier = url;
    for (const [key, value] of Object.entries(params)) {
      identifier = identifier + `&${key}=${value}`;
    }
    return identifier.toLocaleLowerCase();
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
