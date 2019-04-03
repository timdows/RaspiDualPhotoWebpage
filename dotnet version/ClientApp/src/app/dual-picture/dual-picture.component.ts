import { Component, OnInit } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { DisplayImage } from '../DisplayImage';

@Component({
  selector: 'app-dual-picture',
  templateUrl: './dual-picture.component.pug',
  styleUrls: ['./dual-picture.component.scss']
})
export class DualPictureComponent implements OnInit {

  private displayImages = Array<DisplayImage>();
  private imageNumber = -1;

  topImage: DisplayImage;
  bottomImage: DisplayImage;

  countdownMinutes: number;
  countdownSeconds: string;

  private imageSwitchNumber = 0;
  private firstRunCompleted = false;

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
      this.topImage = this.displayImages[imageNumber];
    }
    else if (this.imageSwitchNumber == 1) {
      this.bottomImage = this.displayImages[imageNumber];
    }

    this.imageSwitchNumber++;
    if (this.imageSwitchNumber == 2) {
      this.imageSwitchNumber = 0;
    }
  }

  private getImageNumber(): number {
    this.imageNumber++;
    if (this.imageNumber >= this.displayImages.length) {
      this.getAvailableImages();
      this.imageNumber = 0;
    }

    return this.imageNumber;
  }

  // Gets the countdown value from nodejs (server.js) with the seconds the screen will be on
  private getCountdown(): void {
    this.http.get("api/control/getcountdown")
      .subscribe((data) => {
        let countdown = data as number;
        this.countdownMinutes = Math.floor(countdown / 60);
        this.countdownSeconds = ('0000' + countdown % 60).slice(-2);
      });
  }

}
