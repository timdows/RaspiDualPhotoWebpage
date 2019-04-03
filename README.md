# RaspiDualPhotoWebpage

Most of the code here is displayed and explained on https://timdows.com/projects/dual-photo-webpage-with-angular/


## Autostart
sudo nano /etc/xdg/lxsession/LXDE-pi/autostart

## Latest node and NPM
https://github.com/cncjs/cncjs/wiki/Setup-Guide:-Raspberry-Pi-%7C-Install-Node.js-via-Package-Manager-*(Recommended)*

npm install express --save
npm install node-dir --save

## Display
sudo chmod u+s /bin/chvt
sudo vcgencmd display_power 1