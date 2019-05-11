import { NgModule } from '@angular/core';
import { CommonModule, } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ResizePhotosComponent } from './resize-photos/resize-photos.component';


const routes: Routes = [
    { path: '', component: HomeComponent },
    // { path: 'dailypictures', component: DailyPictureComponent },
    { path: 'resize', component: ResizePhotosComponent },
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
