# Wybieramy bazowy obraz Pythona
FROM python:3.9-slim

# Ustawiamy katalog roboczy w kontenerze
WORKDIR /usr/src/app

# Kopiujemy plik requirements.txt do kontenera
COPY requirements.txt ./

# Instalujemy zależności
RUN pip install --no-cache-dir -r requirements.txt

# Kopiujemy wszystkie pliki do kontenera
COPY . .

# Określamy polecenie do uruchomienia skryptu
CMD ["python", "./app.py"]