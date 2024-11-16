import { Component, HostListener, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['app.component.scss']
})
export class AppComponent implements OnInit {

  public isMobile = false; 

  ngOnInit() {
    this.checkIsPhone();
  }

  @HostListener("window:resize") onMouseEnter() {
    this.checkIsPhone();
  }

  private checkIsPhone() {
    const userAgent = navigator.userAgent.toLowerCase();

    const iphone = userAgent.search('iphone') > -1
    const android = userAgent.search('android') > -1

    const test = (iphone || android) && screen.availWidth < 480

    // console.log(test, userAgent);

    if (test) {
      this.isMobile = true;
      return;
    }

    this.isMobile = false;
  }

}
