import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-main-card',
  templateUrl: './main-card.component.html',
  styleUrls: ['./main-card.component.scss']
})
export class MainCardComponent implements OnInit {

  public name: string = 'asdsa';

  constructor(private router: Router, private activateRoute: ActivatedRoute) {
    this.name = activateRoute.snapshot.params["name"];
  }

  ngOnInit() {
  }

  public onYes() { }
  public onNo() { }
}
