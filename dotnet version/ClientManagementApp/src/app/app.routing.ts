import { NgModule } from '@angular/core';
import { CommonModule, } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ResizePhotosComponent } from './resize-photos/resize-photos.component';
import { ScreenManagementComponent } from './screen-management/screen-management.component';
import { FullScreenAlbumComponent } from './full-screen-album/full-screen-album.component';


const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'resizephotos', component: ResizePhotosComponent },
    { path: 'screenmanagement', component: ScreenManagementComponent },
    { path: 'fullscreenalbum', component: FullScreenAlbumComponent },
    { path: '**', redirectTo: '/' }
];

@NgModule({
    imports: [
        CommonModule,
        BrowserModule,
        RouterModule.forRoot(routes)
    ],
    exports: [
    ],
})
export class AppRoutingModule { }
