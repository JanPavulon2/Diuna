import adafruit_dht
import board
import time

# Set up the DHT21 sensor on GPIO24 (Pin 18 on Raspberry Pi)
dht_sensor = adafruit_dht.DHT21(board.D4, use_pulseio=False)

try:
    while True:
        try:
            # Read temperature and humidity from the sensor
            temperature = dht_sensor.temperature
            humidity = dht_sensor.humidity

            if temperature is not None and humidity is not None:
                print(f"Temperature: {temperature:.1f}°C, Humidity: {humidity:.1f}%")
            else:
                print("Failed to retrieve data from the sensor")

        except RuntimeError as e:
            # Handle occasional sensor errors (normal for DHT sensors)
            print(f"Error reading sensor: {e.args[0]}")

        time.sleep(2)  # Wait before the next reading

except KeyboardInterrupt:
    print("Program stopped")
