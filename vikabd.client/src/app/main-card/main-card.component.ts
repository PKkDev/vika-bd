import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

type Guest = {
  name: string
}

@Component({
  selector: 'app-main-card',
  templateUrl: './main-card.component.html',
  styleUrls: ['./main-card.component.scss']
})
export class MainCardComponent implements OnInit {

  private readonly _baseurl = 'https://www.vika-birthday.ru/api';

  public name: string = 'Кирилл';

  public isUserFounded = false;
  public isInitialLoading = true;

  constructor(activateRoute: ActivatedRoute, private http: HttpClient) {
    this.name = activateRoute.snapshot.params["name"];
  }

  ngOnInit() {
    this.http.get<Guest>(`${this._baseurl}/birth-day/check-guest?ident=${this.name}`)
      .subscribe({
        next: (value: Guest) => {
          this.isInitialLoading = false;

          this.isUserFounded = !!value;
          if (value) {
            this.name = value.name;
          } else {

          }
          // console.log(value);
        },
        error: (err) => {
          this.isUserFounded = false;
          this.isInitialLoading = true;
          console.error(err);
        },
      })
  }

  public onYes() {
    this.http.put(`${this._baseurl}/birth-day/guest-say-yes?ident=${this.name}`, null)
      .subscribe({
        next: (value) => {
          console.log(value);
        },
        error: (err) => console.error(err),
      })
  }
  public onNo() {
    this.http.put(`${this._baseurl}/birth-day/guest-say-no?ident=${this.name}`, null)
      .subscribe({
        next: (value) => {
          console.log(value);
          this.isUserFounded = false;
        },
        error: (err) => console.error(err),
      })
  }
}
