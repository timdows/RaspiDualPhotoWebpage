import { Component, OnInit } from '@angular/core';

declare var $: any;

@Component({
	selector: 'app-home',
	templateUrl: './home.component.pug',
	styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

	constructor() { }

	ngOnInit() {
		$("nav").addClass("navbar-transparent");
	}

	ngOnDestroy() {
		$("nav").removeClass("navbar-transparent");
	}
}
