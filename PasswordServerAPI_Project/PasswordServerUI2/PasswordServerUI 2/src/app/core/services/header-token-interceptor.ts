import { Injectable } from "@angular/core";
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from "@angular/common/http";
import { Observable } from "rxjs";
import { ConfigurationService } from "./configuration.service";



@Injectable()
export class HeaderTokenInterceptor implements HttpInterceptor {

  constructor(
    private configurationService: ConfigurationService,
  ) { }


  intercept(req: HttpRequest<any>, next: HttpHandler):
    Observable<HttpEvent<any>> {

    let token = this.configurationService.getToken() // auth is provided via constructor.
    if (token) {
      // Logged in. Add Bearer token.
      return next.handle(
        req.clone({
          headers: req.headers.append('Authorization', 'Bearer ' + token)
        })
      );

    }
    return next.handle(req);
  }
}