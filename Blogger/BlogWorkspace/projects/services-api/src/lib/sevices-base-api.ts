import { Inject } from '@angular/core';
export class SevicesBaseAPI {
  //constructor(@Inject('env') private environment, ) { }
  constructor(private env: any) { }

  getBaseUrl(servicePath: string): string {
    return this.env.ServicesBaseAPI + servicePath;
  }
}
