import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DisplayImage } from '../DisplayImage';

@Component({
	selector: 'app-image-management',
	templateUrl: './image-management.component.pug',
	styleUrls: ['./image-management.component.scss']
})
export class ImageManagementComponent implements OnInit {

	private displayImages = Array<DisplayImage>();

	constructor(private http: HttpClient) { }

	ngOnInit() {
		this.getAvailableImages();
	}

	private getAvailableImages(): void {
		this.http.get("api/imagemanagement/getdisplayimagedetails")
			.subscribe((data) => {
				this.displayImages = data as Array<DisplayImage>;
			});
	}
}
