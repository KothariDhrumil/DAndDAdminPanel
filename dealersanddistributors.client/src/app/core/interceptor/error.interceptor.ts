import { ToastrService } from 'ngx-toastr';
import { AuthService } from "../service/auth.service";
import { Injectable } from "@angular/core";
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from "@angular/common/http";
import { Observable, throwError } from "rxjs";
import { catchError } from "rxjs/operators";
import { MatSnackBar, MatSnackBarVerticalPosition, MatSnackBarHorizontalPosition } from "@angular/material/snack-bar";

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private authenticationService: AuthService,
      private toastr: ToastrService,
      private snackBar: MatSnackBar) { }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((err) => {
        if (err.status === 401) {
          // auto logout if 401 response returned from api
          this.authenticationService.logout();
          location.reload();
        }
        if (err.status === 400) {
          if (err.error?.response?.fieldErrors?.length) {
            // this.toastr.error("documentLocation Created!");
            let errorString = '';
            err.error?.response?.fieldErrors.forEach((element: { errorCode: string; }) => {
              errorString += element.errorCode + ","
            });

            this.showNotification(
              "snackbar-danger",
              errorString,
              "bottom",
              "right"
            );

          }
        }

        const error = err?.error?.message || err?.statusText || (err as string);
        return throwError(error);
      })
    );
  }

  showNotification(colorName: string, text: string, placementFrom: MatSnackBarVerticalPosition, placementAlign: MatSnackBarHorizontalPosition) {
    this.snackBar.open(text, "", {
      duration: 2000,
      verticalPosition: placementFrom,
      horizontalPosition: placementAlign,
      panelClass: colorName,
    });
  }

}
