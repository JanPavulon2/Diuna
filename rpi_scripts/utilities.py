import os
import json

# Load relay state from JSON file
def load_relay_state(file_path):
    if os.path.exists(file_path):
        with open(file_path, 'r') as file:
            content = json.load(file)
            print("State loaded from file: ")
            print(content)
            return content
    else:
        print("No config file found. Using default config.")
        # Return default state if no file exists
        return {
            "r1": False,
            "r2": False,
            "r3": False,
            "r4": False
        }

def save_relay_state(file_path, state):
    with open(file_path, 'w') as file:
        json.dump(state, file)
    print("State file updated.")
