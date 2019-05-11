import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DisplayImage } from '../DisplayImage';

@Component({
	selector: 'app-image-management',
	templateUrl: './image-management.component.pug',
	styleUrls: ['./image-management.component.scss']
})
export class ImageManagementComponent implements OnInit {

	displayImages = Array<DisplayImage>();

	constructor(private http: HttpClient) { }

	ngOnInit() {
		this.getAvailableImages();
	}

	private getAvailableImages(): void {
		this.http.get("api/imagemanagement/getdisplayimagedetails")
			.subscribe((data) => {
				this.displayImages = data as Array<DisplayImage>;

				this.startResizeProcess();
			});
	}

	async startResizeProcess() {
		for (let i = 0; i < this.displayImages.length; i++) {
			if (this.displayImages[i].isResized) {
				continue;
			}

			this.displayImages[i] = await this.doResizeImage(this.displayImages[i]);
		}
	}

	async doResizeImage(displayImage: DisplayImage) {
		const response = await this.http.post("api/ImageManagement/ResizeImage", displayImage).toPromise();
		return response;
	}
}
