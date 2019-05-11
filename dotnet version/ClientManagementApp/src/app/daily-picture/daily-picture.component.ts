import { Component, OnInit, Inject } from '@angular/core';
import { Hour1400Service, Hour1400File } from 'api';

@Component({
	selector: 'app-daily-picture',
	templateUrl: './daily-picture.component.pug',
	styleUrls: ['./daily-picture.component.scss']
})
export class DailyPictureComponent implements OnInit {

	hour1400Files: Array<Hour1400File>

	constructor(
		private hour1400Service: Hour1400Service,
		@Inject('BASE_URL') private baseUrl: string) { }

	ngOnInit() {
		var body = document.getElementsByTagName('body')[0];
		body.classList.add('landing-page');
		var navbar = document.getElementsByTagName('nav')[0];
		navbar.classList.add('navbar-transparent');

		this.hour1400Service.getThumbnails({})
			.subscribe(data => {
				this.hour1400Files = data.hour1400Files;
			});
	}

	ngOnDestroy() {
		var body = document.getElementsByTagName('body')[0];
		body.classList.remove('landing-page');
		var navbar = document.getElementsByTagName('nav')[0];
		navbar.classList.remove('navbar-transparent');
	}

	getImagePath(hour1400File: Hour1400File) {
		return `${this.baseUrl}api/Hour1400/GetImage?fileName=${hour1400File.fileName}`;
	}

}
