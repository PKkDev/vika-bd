import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Guest } from '../model/guest';

@Component({
  selector: 'app-main-card',
  templateUrl: './main-card.component.html',
  styleUrls: ['./main-card.component.scss']
})
export class MainCardComponent implements OnInit {

  public name: string = 'none';
  public key: string = 'key';

  public isUserSayNo = false;
  public isInitialLoading = true;

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    activateRoute: ActivatedRoute,
    private http: HttpClient) {
    this.key = activateRoute.snapshot.params["key"];
  }

  ngOnInit() {
    this.http.get<Guest>(`${this.baseUrl}/birth-day/check-guest?ident=${this.key}`)
      .subscribe({
        next: (value: Guest) => {
          this.isInitialLoading = false;

          if (value) {
            this.name = value.name;
            if (value.answer) {
              this.router.navigateByUrl(`accepted/${this.key}`);
            } else {
            }
          } else {
            this.router.navigateByUrl(`error/${this.name}`);
          }
        },
        error: (err) => {
          console.error(err);
          this.router.navigateByUrl(`error/${this.name}`);
        },
      })
  }

  public onYes() {
    this.http.put(`${this.baseUrl}/birth-day/guest-say-yes?ident=${this.key}`, null)
      .subscribe({
        next: (value) => {
          console.log(value);
          this.router.navigateByUrl(`accepted/${this.key}`);
        },
        error: (err) => console.error(err),
      })
  }

  public onNo() {
    this.http.put(`${this.baseUrl}/birth-day/guest-say-no?ident=${this.key}`, null)
      .subscribe({
        next: (value) => {
          console.log(value);
          this.isUserSayNo = true;
        },
        error: (err) => console.error(err),
      })
  }
}
