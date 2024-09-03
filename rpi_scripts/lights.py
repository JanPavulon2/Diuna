from gpiozero import LED, RGBLED, Button, OutputDevice
from gpiozero.pins.rpigpio import RPiGPIOFactory
from signal import pause
from time import sleep

from utilities import load_relay_state, save_relay_state

# Path to the JSON file
config_file = 'config.json'
relays_state = load_relay_state(config_file)

gpio_pins = {}
for switch in relays_state['Switches']:
    gpio_pins[switch['Name']] = {
        'button': switch['ButtonPin'],
        'led': switch['LedPin'],
        'relay': switch['RelayPin'],
        'isOn': switch['IsOn']
    }

relays = {}
for key, pins in gpio_pins.items():
    relays[key] = {
        'button': Button(pins['button'], pull_up=True),
        'led': LED(pins['led']),
        'relay': OutputDevice(pins['relay'], active_high=False),
        'isOn': pins['isOn']
    }

# gpio_pins = {
#     'r1': {'button': 23, 'led': 21, 'relay': 26, 'isOn': relays_state['r1']},
#     'r2': {'button': 18, 'led': 20, 'relay': 19, 'isOn': relays_state['r2']},
#     'r3': {'button': 15, 'led': 16, 'relay': 13, 'isOn': relays_state['r3']},
#     'r4': {'button': 14, 'led': 12, 'relay': 6, 'isOn': relays_state['r4']}
# }

# relays = {}
# for key, pins in gpio_pins.items():
#     relays[key] = {
#         'button': Button(pins['button'], pull_up=True),
#         'led': LED(pins['led']),
#         'relay': OutputDevice(pins['relay'], active_high=False),
#         'isOn': pins['isOn']
#     }

# Set the initial state of relays based on loaded state
for relay in relays.values():
    if relay['isOn']:
        relay['relay'].on()
        relay['led'].on()
    else:
        relay['relay'].off()
        relay['led'].off()

# Function to toggle relay, LED, and state
def toggle_relay(relay_name):
    print (f"Toggling {relay_name.upper()} relay.")

    relay = relays[relay_name]
    if relay['isOn']:
        relay['relay'].off()  # Turn off relay
        relay['led'].off()    # Turn off LED
        print(f"Relay {relay_name.upper()} is OFF")
    else:
        relay['relay'].on()   # Turn on relay
        relay['led'].on()     # Turn on LED
        print(f"Relay {relay_name.upper()} is ON")
    
    # Toggle the state
    relay['isOn'] = not relay['isOn']

    # Save the new state to the JSON file
    save_relay_state(config_file, {name: r['isOn'] for name, r in relays.items()})

print (f"Assigning button press events...")

for relay_name, relay in relays.items():
    print (f"Relay: {relay_name.upper()}, content: {relay}.")
    relay['button'].when_released = lambda relay_name=relay_name: toggle_relay(relay_name)

try:
    while True:
        sleep(0.1)

except KeyboardInterrupt:
    print("Exiting and cleaning up...")
    # Explicitly turn off all relays and LEDs
    for relay_name, relay in relays.items():
        relay['relay'].off()
        relay['led'].off()
    print("GPIO cleaned up!")
