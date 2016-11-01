#Make a PPP internet connection with 3G/LTE Shields on Raspberry Pi

You can use Sixfab 3G/LTE Shields to connect internet with PPP connection. You can use following transactions for work.

1. Firstly, Connect your Raspberry Pi to internet and run `sudo apt-get update` to update your Raspberry Pi
2. Make [First Tutorial](https://github.com/sixfab/rpiShields/tree/master/tutorials/tutorial1) to add Quectel Module support to your kernel.
3. Install ppp application with `sudo apt-get install ppp`
4. Edit /etc/ppp/peers/gprs file and add the following:
  ```
  connect "/usr/sbin/chat -v -f /etc/chatscripts/gprs -T INTERNET" //INTERNET is my APN
  ttyUSB3
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
