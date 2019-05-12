import { Component, OnInit } from '@angular/core';
import { ImageManagementService, DisplayImage } from 'api';
import { $ } from 'protractor';
import { HttpClient } from '@angular/common/http';

@Component({
	selector: 'app-resize-photos',
	templateUrl: './resize-photos.component.pug',
	styleUrls: ['./resize-photos.component.scss']
})
export class ResizePhotosComponent implements OnInit {

	displayImages: Array<DisplayImage>;
	isResizing: boolean;
	currentResizeIndex = 0;
	showThumbs = false;

	constructor(private imageManagementService: ImageManagementService, private http: HttpClient) { }

	ngOnInit() {
		this.getOverview();
	}

	getOverview() {
		this.imageManagementService.getDisplayImageDetails()
			.subscribe(data => {
				this.displayImages = data;

				this.startResizeProcess();
			});
	}

	async startResizeProcess() {
		this.isResizing = true;

		for (let i = 0; i < this.displayImages.length; i++) {
			this.currentResizeIndex = i;
			if (this.displayImages[i].isResized) {
				continue;
			}

			this.displayImages[i].isResizing = true;
			this.displayImages[i] = await this.doResizeImage(this.displayImages[i]);
			this.displayImages[i].isResizing = false;
		}

		this.isResizing = false;
	}

	async doResizeImage(displayImage: DisplayImage) {
		const response = await this.http.post("api/ImageManagement/ResizeImage", displayImage).toPromise();
		return response;
	}
}
