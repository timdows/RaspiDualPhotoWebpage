export * from './control.service';
import { ControlService } from './control.service';
export * from './imageManagement.service';
import { ImageManagementService } from './imageManagement.service';
export * from './images.service';
import { ImagesService } from './images.service';
export const APIS = [ControlService, ImageManagementService, ImagesService];
