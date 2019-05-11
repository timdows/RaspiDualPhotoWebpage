import { Component, OnInit } from '@angular/core';
import { ImageManagementService, DisplayImage } from 'api';
import { $ } from 'protractor';

@Component({
	selector: 'app-resize-photos',
	templateUrl: './resize-photos.component.pug',
	styleUrls: ['./resize-photos.component.scss']
})
export class ResizePhotosComponent implements OnInit {

	displayImages: Array<DisplayImage>;

	constructor(private imageManagementService: ImageManagementService) { }

	ngOnInit() {
		this.getOverview();
	}

	getOverview() {
		this.imageManagementService.getDisplayImageDetails()
			.subscribe(data => {
				this.displayImages = data;
			});
	}

}
