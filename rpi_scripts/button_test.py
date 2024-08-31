from gpiozero import Button
from time import sleep

button = Button(5)  # Check with any GPIO number

def button_pressed():
    print("Button pressed!")

button.when_pressed = button_pressed

try:
    while True:
        sleep(0.1)
        
except KeyboardInterrupt:
    print("Exiting")
