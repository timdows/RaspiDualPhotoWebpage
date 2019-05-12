export * from './control.service';
import { ControlService } from './control.service';
export * from './fullScreenAlbum.service';
import { FullScreenAlbumService } from './fullScreenAlbum.service';
export * from './imageManagement.service';
import { ImageManagementService } from './imageManagement.service';
export * from './images.service';
import { ImagesService } from './images.service';
export const APIS = [ControlService, FullScreenAlbumService, ImageManagementService, ImagesService];
