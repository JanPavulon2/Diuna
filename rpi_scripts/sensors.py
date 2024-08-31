import adafruit_dht
import board
from time import sleep

# Set up the DHT22 sensor on GPIO4
dht_sensor = adafruit_dht.DHT22(board.D4, use_pulseio=False)

while True:
    try:
        # Read temperature and humidity
        temperature = dht_sensor.temperature
        humidity = dht_sensor.humidity

        if temperature is not None and humidity is not None:
            print(f"Temperature: {temperature:.1f}°C, Humidity: {humidity:.1f}%")
        else:
            print("Failed to retrieve data from sensor")

    except RuntimeError as error:
        # Handle occasional errors from the sensor
        print(f"Error reading sensor: {error.args[0]}")

    # Wait before the next reading
    sleep(2)
