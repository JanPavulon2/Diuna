import pigpio
import time
import DHT

# Set up pigpio
pi = pigpio.pi()

# Define the GPIO pin connected to the DHT21 data pin (change if using a different GPIO)
DHT_PIN = 4  # GPIO4 (Pin 7 on Raspberry Pi header)

# Set up the DHT sensor using pigpio's DHT functionality
dht_sensor = DHT.sensor(pi, DHT_PIN, model=DHT.DHT21)  # Change model to DHT22 if using that

try:
    while True:
        # Trigger a reading from the sensor
        dht_sensor.trigger()

        # Get temperature and humidity
        temperature = dht_sensor.temperature()  # In Celsius
        humidity = dht_sensor.humidity()

        # Only print data if valid
        if temperature is not None and humidity is not None:
            print(f"Temperature: {temperature:.1f}°C, Humidity: {humidity:.1f}%")
        else:
            print("Failed to retrieve data from sensor")

        time.sleep(2)  # Wait for 2 seconds before the next reading

except KeyboardInterrupt:
    print("Exiting the program")

finally:
    # Stop pigpio connection and cleanup
    dht_sensor.cancel()
    pi.stop()
