import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class ToastrService {
  constructor(private snackBar: MatSnackBar) {}

  private open(message: string, type: string) {
    this.snackBar.open(message, '', {
      duration: 3 * 1000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom',
      panelClass: type,
    });
  }

  error(message: string) {
    this.open(message, 'error-snack-bar');
  }

  success(message: string) {
    this.open(message, 'success-snack-bar');
  }

  warn(message: string) {
    this.open(message, 'warn-snack-bar');
  }
}
