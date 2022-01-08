import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SearchService {
  searchString: string;
  baseUrl = environment.apiUrl + 'search';

  constructor(private http: HttpClient) {}

  searchProducts(search: string) {
    let params = new HttpParams();
    params = params.append('q', search);
    return this.http.get(this.baseUrl, { params: params });
  }
}
