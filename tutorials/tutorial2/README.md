# Make a PPP internet connection with GPRS Shield on Raspberry Pi

Gprs shield use Uart connection on Raspberry Pi. You can use following transactions for work. 

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
      
3. Install ppp application with `sudo apt-get install ppp`
4. Edit /etc/ppp/peers/gprs file and add the following:
  ```
  connect "/usr/sbin/chat -v -f /etc/chatscripts/gprs -T INTERNET" //INTERNET is my APN
  serial0
  115200
  lock
  crtscts
  modem
  passive
  novj
  defaultroute
  noipdefault
  usepeerdns
  noauth
  hide-password
  persist
  holdoff 10
  maxfail 0
  debug

  ```
5. Edit /etc/network/interfaces  and add the following: 
  ```
  auto gprs
  iface gprs inet ppp
  provider gprs
  
  ```
6. Reboot your machine and Let's connect ;)
  - run `ifconfig ppp0` at terminal window to see following outputs and see your ip<br/>
  ```
  ppp0      Link encap:Point-to-Point Protocol
            inet addr:10.XX.XXX.XXX  P-t-P:192.168.254.254  Mask:255.255.255.255
            UP POINTOPOINT RUNNING NOARP MULTICAST  MTU:1500  Metric:1
            RX packets:38 errors:0 dropped:0 overruns:0 frame:0
            TX packets:39 errors:0 dropped:0 overruns:0 carrier:0
            collisions:0 txqueuelen:3
            RX bytes:3065 (2.9 KiB)  TX bytes:2657 (2.5 KiB)
  ```
  
