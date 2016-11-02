# Raspberry Pi Gps Tracker with Sixfab Gps Shield

1. Firstly, Connect your Raspberry Pi to internet and run `sudo apt-get update` to update your Raspberry Pi
2. We should stop getty service on Raspbian.
  1. For non Raspberry Pi 3 machines, remember it’s /dev/ttyAMA0 that is linked to the getty (console) service. So you need to perform this command from a terminal window:
    - `sudo systemctl disable serial-getty@ttyAMA0.service`
    - `sudo systemctl disable serial-getty@ttyAMA0.service`
  
  2. For Raspberry Pi 3’s the command is similar but referencing /dev/ttyS0:
    - `sudo systemctl stop serial-getty@ttyS0.service`
    - `sudo systemctl disable serial-getty@ttyS0.service`
    
  3. You also need to remove the console from the cmdline.txt. If you edit this with:
    - `sudo nano /boot/cmdline.txt`<br/><br/>
      ```
      dwc_otg.lpm_enable=0 console=serial0,115200 console=tty1 root=/dev/mmcblk0p2 rootfstype=ext4 elevator=deadline fsck.repair=yes rootwait quiet splash plymouth.ignore-serial-consoles
      ```<br/><br/>
      remove the line: console=serial0,115200 and save and reboot for changes to take effect.
  4. You also need to enable uart with edit /boot/config.txt file
    - `sudo nano /boot/config.txt` and add `enable_uart=1` to bottom of file then save and reboot for changes to take effect.
4. Download source code 
  - `wget https://raw.githubusercontent.com/sixfab/rpiShields/master/tutorials/tutorial8/GpsTracker.py`

5. Make it executable
  - `sudo chmod +x GpsTracker.py`
  
6. Run it
  - `sudo python GpsTracker.py`
