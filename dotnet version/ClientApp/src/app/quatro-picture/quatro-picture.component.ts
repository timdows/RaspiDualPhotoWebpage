import { Component, OnInit } from '@angular/core';
import { DisplayImage } from '../DisplayImage';
import { HttpClient } from '@angular/common/http';

@Component({
	selector: 'app-quatro-picture',
	templateUrl: './quatro-picture.component.pug',
	styleUrls: ['./quatro-picture.component.scss']
})
export class QuatroPictureComponent implements OnInit {

	displayImages = Array<DisplayImage>();
	private imageNumber = -1;

	topLeftImage: DisplayImage;
	topRightImage: DisplayImage;
	bottomLeftImage: DisplayImage;
	bottomRightImage: DisplayImage;

	showCountdown: boolean = false;
	countdownMinutes: number;
	countdownSeconds: string;

	isInitialized = false;

	private switch = true;
	private imageSwitchNumber = 0;

	constructor(private http: HttpClient) { }

	ngOnInit(): void {
		this.getAvailableImages();

		// Get the countdown every second
		setInterval(() => {
			this.getCountdown();
		}, 1000);
		// Change image every 7 seconds
		setInterval(() => {
			this.changeImages();
		}, 7000);
	}

	private getAvailableImages(): void {
		this.http.get("api/images/getimages")
			.subscribe((data) => {
				this.displayImages = data as Array<DisplayImage>;

				this.isInitialized = true;

				this.changeImages();
				this.changeImages();
				this.changeImages();
			});
	}

	private refreshAvailableImages(): void {
		if (!this.isInitialized) {
			return;
		}

		this.http.get("api/images/getimages")
			.subscribe((data) => {
				this.displayImages = data as Array<DisplayImage>;
			});
	}

	private changeImages(): void {
		let imageNumber = this.getImageNumber();

		if (this.imageSwitchNumber == 0) {
			this.topLeftImage = this.displayImages[imageNumber];
		}
		else if (this.imageSwitchNumber == 1) {
			this.topRightImage = this.displayImages[imageNumber];
		}
		else if (this.imageSwitchNumber == 2) {
			this.bottomLeftImage = this.displayImages[imageNumber];
		}
		// else if (this.imageSwitchNumber == 3) {
		// 	this.bottomRightImage = this.displayImages[imageNumber];
		// }

		this.imageSwitchNumber++;
		if (this.imageSwitchNumber == 3) {
			this.imageSwitchNumber = 0;
		}
	}

	private getImageNumber(): number {
		this.imageNumber++;
		if (this.imageNumber >= this.displayImages.length) {
			this.refreshAvailableImages();
			this.imageNumber = 0;
		}

		return this.imageNumber;
	}

	// Gets the countdown value from nodejs (server.js) with the seconds the screen will be on
	private getCountdown(): void {
		
		// this.http.get("countdown.json")
		// 	.subscribe((data) => {
		// 		let countdown = data.json().countdown;
		// 		this.countdownMinutes = Math.floor(countdown / 60);
		// 		this.countdownSeconds = ('0000' + countdown % 60).slice(-2);
		// 	});
	}

}
