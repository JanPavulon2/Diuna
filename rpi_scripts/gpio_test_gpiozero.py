from gpiozero import Device, GPIODevice
from gpiozero.pins.native import NativeFactory

import os

def scan_gpio_pins():
    # Initialize gpiozero with NativeFactory
    Device.pin_factory = NativeFactory()
    all_pins = list(range(2, 28))  # Raspberry Pi GPIO pins 2-27

    pin_info = {}

    for pin in all_pins:
        try:
            device = GPIODevice(pin)
            device.pin.mode = 'in'  # Set as input
            value = device.pin.state
            function = device.pin.function
            pin_info[pin] = {
                'mode': function,
                'value': value,
            }
        except Exception as e:
            pin_info[pin] = {'error': str(e)}

    return pin_info

def release_all_pins():
    # Release all GPIOs by closing the pin factory
    Device.pin_factory.close()

if __name__ == "__main__":
    pin_info = scan_gpio_pins()
    
    for pin, info in pin_info.items():
        print(f"Pin {pin}: {info}")
    
    # Release all GPIOs
    release_all_pins()
    print("Released all GPIO pins.")
