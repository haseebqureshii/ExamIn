import requests
import json
import cv2


url = "http://104.197.61.59/predict" # add object_detection url
path = r'D:\FYP\ExamIn\ExamInDesktopUI.Library\images\frame.jpg'
headers = {"content-type": "image/jpg"}

# encode image
image = cv2.imread(path)
_, img_encoded = cv2.imencode(".jpg", image)

# send HTTP request to the server
response = requests.post(url, data=img_encoded.tobytes(), headers=headers)
predictions = response.json()

# annotate the image
for pred in predictions:
	# print prediction
	print(pred)

	# extract the bounding box coordinates
	(x, y) = (pred["boxes"][0], pred["boxes"][1])
	(w, h) = (pred["boxes"][2], pred["boxes"][3])

	# draw a bounding box rectangle and label on the image
	cv2.rectangle(image, (x, y), (x + w, y + h), pred["color"], 2)
	text = "{}: {:.4f} \n".format(pred["label"], pred["confidence"])
	cv2.putText(
		image, 
		text, 
		(x, y - 5), 
		cv2.FONT_HERSHEY_SIMPLEX,
	    0.5, 
	    pred["color"], 
	    2
	)

	# save pred in text file
	text_file = open("D:\FYP\ExamIn\ExamInDesktopUI.Library\ObjectsDetected\prediction.txt", "a")
	text_file.write(json.dumps(text))
	text_file.close()

# save annotated image
#cv2.imwrite("annotated_image.jpg", image)