
import requests
import json

API_KEY = "Your API-Key"#Мой API-ключ

longitude = 37.6156  # долгота
latitude  = 55.7522  # широта

temperature_arr = []

api_response = requests.get(f"https://api.openweathermap.org/data/2.5/onecall?lat={latitude}&lon"
+f"={longitude}&exclude=minutely,hourly,alerts&appid={API_KEY}&units=metric").text
json_response = json.loads(api_response)
for i in range(5):    
    temperature_arr.append(json_response["daily"][i]["temp"]["morn"])
    

print("Средняя температура за пять дней равна", sum(temperature_arr) / 5,
      "\nМаксимальная температура за пять дней равна", max(temperature_arr))

