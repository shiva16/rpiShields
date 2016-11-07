#Import sound files to Sixfab Gprs Shield

1. Download GenBinFile and QFlash applications.
2. Start GenBinFile.exe and import .amr sound files. **Note :if you have ogg etc. file you can convert it from online services.**
  ![alt tag](https://raw.githubusercontent.com/sixfab/rpiShields/master/tutorials/tutorial9/images/1.png)
3. Convert files and get following warning.
  ![alt tag](https://raw.githubusercontent.com/sixfab/rpiShields/master/tutorials/tutorial9/images/2.png)
4. Copy `scat_download.cfg` under `scat` folder to under `audio bin` folder.
5. Start Qflash.exe and Load FW Files, select `scat_download.cfg` files under `audio bin` folder.
  ![alt tag](https://raw.githubusercontent.com/sixfab/rpiShields/master/tutorials/tutorial9/images/3.png)
6. Plug your Sixfab Gprs Shield over usb and select source pin to `USB` on shield.
7. Press start and wait for initialize shield.
  ![alt tag](https://raw.githubusercontent.com/sixfab/rpiShields/master/tutorials/tutorial9/images/4.png)
8. Press button 2+ second on shield to start firmware update.
  ![alt tag](https://raw.githubusercontent.com/sixfab/rpiShields/master/tutorials/tutorial9/images/5.png)
9. See following screen to succesful update.
  ![alt tag](https://raw.githubusercontent.com/sixfab/rpiShields/master/tutorials/tutorial9/images/6.png)
  
