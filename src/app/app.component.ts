import { Component, OnInit } from '@angular/core';
import { Configuration } from "app/app.configuration";
import { Http } from "@angular/http";
import { DisplayImage } from "app/_models/display-image";

@Component({
	selector: 'app-root',
	templateUrl: './app.component.pug',
	styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

	private allImages = [];
	topImages = new Array<DisplayImage>();
	bottomImages = new Array<DisplayImage>();

	private switch = true;

	constructor(
		private configuration: Configuration,
		private http: Http) { }

	ngOnInit(): void {
		this.getAvailableImages();
	}

	private getAvailableImages(): void {
		// Empty the existing arrays
		this.topImages = new Array<DisplayImage>();
		this.bottomImages = new Array<DisplayImage>();

		this.http.get("images.json")
			.subscribe((data) => {
				this.allImages = data.json();

				this.topImages.push(new DisplayImage(this.allImages[this.getRandomNumber()]));
				this.topImages.push(new DisplayImage(this.allImages[this.getRandomNumber()]));
				this.topImages.push(new DisplayImage(this.allImages[this.getRandomNumber()]));

				this.bottomImages.push(new DisplayImage(this.allImages[this.getRandomNumber()]));
				this.bottomImages.push(new DisplayImage(this.allImages[this.getRandomNumber()]));
				this.bottomImages.push(new DisplayImage(this.allImages[this.getRandomNumber()]));

				this.changeImages();
			});

		setTimeout(() => {
			this.getAvailableImages();
		}, 60 * 60 * 1000);
	}

	private changeImages(): void {
		if (this.switch) {
			let newTopImage = new DisplayImage(this.allImages[this.getRandomNumber()]);
			this.topImages.push(newTopImage);
			this.topImages.shift();
		}
		else {
			let newBottomImage = new DisplayImage(this.allImages[this.getRandomNumber()]);
			this.bottomImages.push(newBottomImage);
			this.bottomImages.shift();
		}

		this.switch = !this.switch;

		setTimeout(() => {
			this.changeImages();
		}, 7 * 1000);
	}

	private getRandomNumber(): number {
		return this.randomIntFromInterval(0, this.allImages.length - 1)
	}

	private randomIntFromInterval(min, max): number {
		var number = Math.floor(Math.random() * (max - min + 1) + min);
		return number;
	}
}