import RPi.GPIO as GPIO
import time

def scan_gpio_pins():
    GPIO.setmode(GPIO.BCM)  # Use BCM pin numbering
    all_pins = list(range(2, 28))  # Raspberry Pi GPIO pins 2-27

    pin_info = {}

    for pin in all_pins:
        try:
            GPIO.setup(pin, GPIO.IN, pull_up_down=GPIO.PUD_OFF)
            value = GPIO.input(pin)
            function = GPIO.gpio_function(pin)
            pin_info[pin] = {
                'mode': function,
                'value': value,
            }
        except Exception as e:
            pin_info[pin] = {'error': str(e)}

    return pin_info

def release_all_pins():
    GPIO.cleanup()  # Release all GPIOs

if __name__ == "__main__":
    pin_info = scan_gpio_pins()
    
    for pin, info in pin_info.items():
        print(f"Pin {pin}: {info}")
    
    # Release all GPIOs
    release_all_pins()
    print("Released all GPIO pins.")
