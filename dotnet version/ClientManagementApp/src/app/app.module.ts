import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // this is needed!
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app.routing';

import { AppComponent } from './app.component';
import { NavbarComponent } from './shared/navbar/navbar.component';
import { FooterComponent } from './shared/footer/footer.component';
import { ApiModule, Configuration, ConfigurationParameters } from 'api';
import { environment } from 'environments/environment';
import { HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './home/home.component';
import { JwBootstrapSwitchNg2Module } from 'jw-bootstrap-switch-ng2';
import { ResizePhotosComponent } from './resize-photos/resize-photos.component';
import { ScreenManagementComponent } from './screen-management/screen-management.component';

export function apiConfigFactory(): Configuration {
    const params: ConfigurationParameters = {
        basePath: environment.serverUrl,
        apiKeys: {}
    }
    return new Configuration(params);
}

@NgModule({
    declarations: [
        AppComponent,
        NavbarComponent,
        HomeComponent,
        FooterComponent,
        ResizePhotosComponent,
        ScreenManagementComponent
    ],
    imports: [
        ApiModule.forRoot(apiConfigFactory),
        HttpClientModule,
        BrowserAnimationsModule,
        NgbModule.forRoot(),
        JwBootstrapSwitchNg2Module,
        FormsModule,
        RouterModule,
        AppRoutingModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
