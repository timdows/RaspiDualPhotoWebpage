import { Component, OnInit } from '@angular/core';
import { FullScreenAlbumService } from 'api/api/fullScreenAlbum.service';
import { AlbumInfo } from 'api/model/albumInfo';

@Component({
	selector: 'app-full-screen-album',
	templateUrl: './full-screen-album.component.pug',
	styleUrls: ['./full-screen-album.component.scss']
})
export class FullScreenAlbumComponent implements OnInit {

	albumsInfo: Array<AlbumInfo>;

	constructor(private fullScreenAlbumService: FullScreenAlbumService) { }

	ngOnInit() {
		this.getOverview();
	}

	getOverview() {
		this.fullScreenAlbumService.getAlbums()
			.subscribe(data => {
				this.albumsInfo = data;
			});
	}
}
