import { Component, OnInit } from '@angular/core';
import { Http } from "@angular/http";
import { DisplayImage } from "app/_models/display-image";

@Component({
	selector: 'app-root',
	templateUrl: './app.component.pug',
	styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

	private allImages = [];
	private imageNumber = -1;

	topLeftImage: DisplayImage;
	topRightImage: DisplayImage;
	bottomLeftImage: DisplayImage;
	bottomRightImage: DisplayImage;
	countdownMinutes: number;
	countdownSeconds: string;

	private switch = true;
	private imageSwitchNumber = 0;
	private firstRunCompleted = false;

	constructor(private http: Http) { }

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
		this.http.get("images.json")
			.subscribe((data) => {
				this.allImages = data.json();

				if (!this.firstRunCompleted) {
					this.firstRunCompleted = true;
					this.changeImages();
					this.changeImages();
					this.changeImages();
					this.changeImages();
				}
			});
	}

	private changeImages(): void {
		let imageNumber = this.getImageNumber();

		if (this.imageSwitchNumber == 0) {
			this.topLeftImage = new DisplayImage(this.allImages[imageNumber], `${imageNumber + 1}/${this.allImages.length}`);
		}
		else if (this.imageSwitchNumber == 1) {
			this.topRightImage = new DisplayImage(this.allImages[imageNumber], `${imageNumber + 1}/${this.allImages.length}`);
		}
		else if (this.imageSwitchNumber == 2) {
			this.bottomLeftImage = new DisplayImage(this.allImages[imageNumber], `${imageNumber + 1}/${this.allImages.length}`);
		}
		else if (this.imageSwitchNumber == 3) {
			this.bottomRightImage = new DisplayImage(this.allImages[imageNumber], `${imageNumber + 1}/${this.allImages.length}`);
		}

		this.imageSwitchNumber++;
		if (this.imageSwitchNumber == 4) {
			this.imageSwitchNumber = 0;
		}
	}

	private getImageNumber(): number {
		this.imageNumber++;
		if (this.imageNumber >= this.allImages.length) {
			this.getAvailableImages();
			this.imageNumber = 0;
		}

		return this.imageNumber;
	}

	// Gets the countdown value from nodejs (server.js) with the seconds the screen will be on
	private getCountdown(): void {
		this.http.get("countdown.json")
			.subscribe((data) => {
				let countdown = data.json().countdown;
				this.countdownMinutes = Math.floor(countdown / 60);
				this.countdownSeconds = ('0000'+countdown % 60).slice(-2);
			});
	}
}