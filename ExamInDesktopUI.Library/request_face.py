import requests
import json
import cv2


url = "http://104.197.61.59/face"
path = r'D:\FYP\ExamIn\ExamInDesktopUI.Library\faces\frame.jpg'
headers = {"content-type": "image/jpg"}

# encode image
image = cv2.imread(path)
_, img_encoded = cv2.imencode(".jpg", image)

# send HTTP request to the server
response = requests.post(url, data=img_encoded.tobytes(), headers=headers)
prediction = response.json()
print(prediction)

# save pred in text file
text_file = open("D:\FYP\ExamIn\ExamInDesktopUI.Library\FacesDetected\prediction.txt", "w")
text_file.write(prediction)
text_file.close()