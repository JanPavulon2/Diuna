import pigpio
import time

# Initialize pigpio
pi = pigpio.pi('localhost', 8888)

# Define the GPIO pins to iterate through (this covers GPIO0 to GPIO27)
gpio_pins = list(range(0, 28))  # Adjust as needed for other GPIOs

# Iterate through each GPIO pin and print its state
for pin in gpio_pins:
    # Get the current mode (input, output, etc.)
    mode = pi.get_mode(pin)
    
    # Read the current level (high or low)
    level = pi.read(pin)

    # Translate the mode to a readable format
    if mode == pigpio.INPUT:
        mode_str = "INPUT"
    elif mode == pigpio.OUTPUT:
        mode_str = "OUTPUT"
    else:
        mode_str = f"OTHER (Mode: {mode})"

    # Print the current state of the pin
    print(f"GPIO{pin}: Mode = {mode_str}, Level = {'HIGH' if level else 'LOW'}")

    # Optionally, reset the GPIO pin to INPUT for cleanup
    pi.set_mode(pin, pigpio.INPUT)

print("GPIO status printed and cleaned up.")

# Stop the pigpio connection
pi.stop()
