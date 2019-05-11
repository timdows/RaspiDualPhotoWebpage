import { NgModule } from '@angular/core';
import { CommonModule, } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';

import { LandingComponent } from './examples/landing/landing.component';
import { DailyPictureComponent } from './daily-picture/daily-picture.component';
import { ProfileComponent } from './profile/profile.component';

const routes: Routes = [
    { path: '', component: LandingComponent },
    { path: 'dailypictures', component: DailyPictureComponent },
    { path: 'abouttim', component: ProfileComponent },
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
