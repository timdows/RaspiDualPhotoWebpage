import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
	selector: 'app-screen-management',
	templateUrl: './screen-management.component.pug',
	styleUrls: ['./screen-management.component.scss']
})
export class ScreenManagementComponent implements OnInit {

	constructor(private http: HttpClient) { }

	ngOnInit() {
	}

	smallView() {
		this.http.get("http://192.168.1.127:8080/api/modules/clock/hide")
			.subscribe();
	}

	bigView() {
		this.http.get("http://192.168.1.127:8080/api/modules/clock/show")
			.subscribe();
	}

}
