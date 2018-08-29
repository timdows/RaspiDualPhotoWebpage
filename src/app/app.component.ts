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
	private imageNumber = 0;

	topImage: DisplayImage;
	bottomImage: DisplayImage;
	countdownMinutes: number;
	countdownSeconds: string;

	private switch = true;

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

				let topImageNumber = this.getImageNumber();
				let bottomImageNumber = this.getImageNumber();
				this.topImage = new DisplayImage(this.allImages[topImageNumber], `${topImageNumber}/${this.allImages.length}`);
				this.bottomImage = new DisplayImage(this.allImages[bottomImageNumber], `${bottomImageNumber}/${this.allImages.length}`);
			});
	}

	private changeImages(): void {
		if (this.switch) {
			let topImageNumber = this.getImageNumber();
			this.topImage = new DisplayImage(this.allImages[topImageNumber], `${topImageNumber}/${this.allImages.length}`);
		}
		else {
			let bottomImageNumber = this.getImageNumber();
			this.bottomImage = new DisplayImage(this.allImages[bottomImageNumber], `${bottomImageNumber}/${this.allImages.length}`);
		}

		this.switch = !this.switch;
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