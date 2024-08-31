from gpiozero import LED, RGBLED, Button, OutputDevice
from gpiozero.pins.rpigpio import RPiGPIOFactory
from signal import pause
from time import sleep

from utilities import load_relay_state, save_relay_state

led1 = LED(21)
led2 = LED(20)
led3 = LED(16)
led4 = LED(12)

# 14 15 18 23
# 21 20 16 12
button1 = Button(23)  # Use GPIO5 (BCM)
button2 = Button(18)  # Use GPIO5 (BCM)
button3 = Button(15)  # Use GPIO5 (BCM)
button4 = Button(14)  # Use GPIO5 (BCM)

relay1 = OutputDevice(26, active_high=False)
relay2 = OutputDevice(19, active_high=False)
relay3 = OutputDevice(13, active_high=False)
relay4 = OutputDevice(6, active_high=False)

relay1.on()
relay2.on()
relay3.on()
relay4.on()

led1.on()
led2.on()
led3.on()
led4.on()

def button1_pressed():
    led1.toggle()
    relay1.toggle()

def button2_pressed():
    led2.toggle()
    relay2.toggle()

def button3_pressed():
    led3.toggle()
    relay3.toggle()

def button4_pressed():
    led4.toggle()
    relay4.toggle()

button1.when_pressed = button1_pressed
button2.when_pressed = button2_pressed
button3.when_pressed = button3_pressed
button4.when_pressed = button4_pressed


try:
    while True:
        # print('Is button held?')
        # print(button.is_held)
        sleep(0.1)
        
except KeyboardInterrupt:
    print("Exiting")

