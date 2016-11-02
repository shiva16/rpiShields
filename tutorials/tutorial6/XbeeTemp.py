import serial, time, os, glob, subprocess
from threading import Timer

base_dir = '/sys/bus/w1/devices/'
device_folder = glob.glob(base_dir + '10*')[0]
device_file = device_folder + '/w1_slave'

ser = 0



def init_serial():
    global ser
    ser = serial.Serial()
    ser.baudrate = 115200
    ser.port = '/dev/serial0'  # starts at 0, so subtract 1

    ser.timeout = 1
    ser.open()

    if ser.isOpen():
        print
        'Open: ' + ser.portstr


def read_temp_raw():
    catdata = subprocess.Popen(['cat', device_file], stdout=subprocess.PIPE, stderr=subprocess.PIPE)
    out, err = catdata.communicate()
    out_decode = out.decode('utf-8')
    lines = out_decode.split('\n')
    return lines


def read_temp():
    lines = read_temp_raw()
    while lines[0].strip()[-3:] != 'YES':
        time.sleep(0.2)
        lines = read_temp_raw()
    equals_pos = lines[1].find('t=')
    if equals_pos != -1:
        temp_string = lines[1][equals_pos + 2:]
        temp_c = float(temp_string) / 1000.0
        temp_f = temp_c * 9.0 / 5.0 + 32.0
        return str(temp_c)


counter = 0
init_serial()

while 1:
    print(read_temp())
    ser.write("C: " + str(counter) + " | T: " + read_temp() + '\r\n')
    counter += 1
    time.sleep(1)
