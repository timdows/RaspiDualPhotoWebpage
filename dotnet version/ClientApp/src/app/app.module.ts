import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { DualPictureComponent } from './dual-picture/dual-picture.component';
import { QuatroPictureComponent } from './quatro-picture/quatro-picture.component';
import { ImageManagementComponent } from './image-management/image-management.component';
import { ScreenManagementComponent } from './screen-management/screen-management.component';

@NgModule({
  declarations: [
    AppComponent,
    DualPictureComponent,
    QuatroPictureComponent,
    ImageManagementComponent,
    ScreenManagementComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: DualPictureComponent, pathMatch: 'full' },
      { path: 'dual', component: DualPictureComponent },
      { path: 'quatro', component: QuatroPictureComponent },
      { path: 'imagemanagement', component: ImageManagementComponent },
      { path: 'screenmanagement', component: ScreenManagementComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
