import { HttpClient } from '@angular/common/http';
import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Guest } from '../model/guest';

@Component({
  selector: 'app-accepted-card',
  templateUrl: './accepted-card.component.html',
  styleUrls: ['./accepted-card.component.scss']
})
export class AcceptedCardComponent implements OnInit {

  public name: string = 'name';

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private router: Router,
    activateRoute: ActivatedRoute,
    private http: HttpClient) {
    this.name = activateRoute.snapshot.params["name"];
  }

  ngOnInit() {
    this.http.get<Guest>(`${this.baseUrl}/birth-day/check-guest?ident=${this.name}`)
      .subscribe({
        next: (value: Guest) => {
          if (value) {
            this.name = value.name;
            if (!value.answer) {
              this.router.navigateByUrl(`${this.name}`);
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

  public onNewAnswer() {
    this.http.put(`${this.baseUrl}/birth-day/guest-say-no?ident=${this.name}`, null)
      .subscribe({
        next: (value) => {
          this.router.navigateByUrl(`${this.name}`);
          console.log(value);
        },
        error: (err) => console.error(err),
      })
  }

  // public onGoToMap() {
  //   window.open('https://yandex.ru/maps/-/CDxeE6mD', '_blank');
  // }

}
