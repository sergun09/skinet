import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const resquest = req.clone({ withCredentials:true});

  return next(resquest);
};
