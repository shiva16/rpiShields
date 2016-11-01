# Kernel update for 3G and 4G shield's usb drivers

Original Raspbian Jessie kernel doesn't support Quectel EC20/UC20 modules, then we fixed kernel, you can replace your module files with new files.

1. Firstly, Download  [drivers.tar.gz](https://github.com/sixfab/rpiShields/blob/master/tutorials/tutorial1/drivers.tar.gz) file and Extract it.

2. Copy files under /lib/modules/4.4.21-v7+/kernel/drivers

3. Reboot your machine

4. When you run `dmesg` command, You can see outputs like this:

  ```
  ...
  [    8.199312] usb 1-1.5: New USB device found, idVendor=05c6, idProduct=9215
  [    8.199330] usb 1-1.5: New USB device strings: Mfr=1, Product=2, SerialNumber=0
  [    8.199337] usb 1-1.5: Product: Quectel LTE Module
  [    8.199343] usb 1-1.5: Manufacturer: Quectel
  [    8.261386] usbcore: registered new interface driver usbserial
  [    8.261459] usbcore: registered new interface driver usbserial_generic
  [    8.261505] usbserial: USB Serial support registered for generic
  [    8.265611] usbcore: registered new interface driver cdc_wdm
  [    8.268292] usbcore: registered new interface driver qcserial
  [    8.268375] usbserial: USB Serial support registered for Qualcomm USB modem
  [    8.274727] qcserial 1-1.5:1.0: Qualcomm USB modem converter detected
  [    8.275426] usb 1-1.5: Qualcomm USB modem converter now attached to ttyUSB0
  [    8.275945] qcserial 1-1.5:1.1: Qualcomm USB modem converter detected
  [    8.276344] usb 1-1.5: Qualcomm USB modem converter now attached to ttyUSB1
  [    8.276735] qcserial 1-1.5:1.2: Qualcomm USB modem converter detected
  [    8.277433] usb 1-1.5: Qualcomm USB modem converter now attached to ttyUSB2
  [    8.277808] qcserial 1-1.5:1.3: Qualcomm USB modem converter detected
  [    8.278124] usb 1-1.5: Qualcomm USB modem converter now attached to ttyUSB3
  [    8.279606] qmi_wwan 1-1.5:1.4: cdc-wdm0: USB WDM device
  ...
  ```
  
5. You can use your modem with /dev/ttyUSB3
