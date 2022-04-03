from xml.etree.ElementTree import fromstring
import cv2
import numpy as np;
import face_recognition;
import os;
from numpy import asarray;
from PIL import Image;
from datetime import datetime

def findEncodings(images):
  # print(cv2.__version__)
  encodeList=[]
  for img in images:
    img= cv2.cvtColor(img,cv2.COLOR_BGR2RGB)
    encode = face_recognition.face_encodings(img)[0]
    #encodeList[i for i if i not in classNames]
    encodeList.append(encode)
    #print("encoding")
  return encodeList

def predictFace():
  image = Image.open('./images/frame.jpg')
  data = asarray(image)
  path='SavedImages'
  images=[]
  #print("Images",images)  
  classNames=[]
  myList = os.listdir(path)
  #print(myList)
  for cl in myList:
    curImg = cv2.imread(f'{path}/{cl}')
    images.append(curImg)
    classNames.append(os.path.splitext(cl)[0])
    #print(classNames)  
   
  encodeListKnown = findEncodings(images)
  #print(len(encodeListKnown))
  img =data
  img=cv2.cvtColor(img,cv2.COLOR_BGR2RGB)
  faces_curr_frame = face_recognition.face_locations(img)
  #print("faces",faces_curr_frame)
  if  not faces_curr_frame:
    #print("No face  found")
    return "Face is not detected"
  
  encode_curr_frame = face_recognition.face_encodings(img , faces_curr_frame)
  for encodeface , faceloc in zip(encode_curr_frame , faces_curr_frame ):
    match = face_recognition.compare_faces([encodeListKnown] , encodeface)
    facedist= face_recognition.face_distance(encodeListKnown , encodeface)
    #print("Face distance",facedist)
    facedist2= min(facedist)
    #print("FACE DISTANCE",facedist2)
    if(facedist2>=0.5):
      y1,x2,y2,x1 = faceloc
      y1,x2,y2,x1 =  y1*4,x2*4,y2*4,x1*4
      cv2.rectangle(img , (x1,y1) , (x2,y2), (0,255,0) , 2 )
      cv2.rectangle(img , (x1,y2-35) , (x2,y2), (0,255,0) , cv2.FILLED )
      cv2.putText(img ,"UNKNOWN",(x1+6 , y2-6),  cv2.FONT_HERSHEY_COMPLEX , 1 ,(255,255,255), 2 )
      #print("Unknown" )
      return "Unknown"
    # cv2_imshow(img )
      cv2.waitKey(1)
    else:
      matchIndex = np.argmin(facedist)
      #print(matchIndex)
      name = classNames[matchIndex].upper()
      #print("Name",name)
      return name

if __name__ == '__main__':
    result = predictFace()
    #print(result)
    with open("D:\FYP\ExamIn\ExamInDesktopUI.Library\FacePrediction.txt", 'a') as f:
        f.write(datetime.now().strftime("%d/%m/%Y, %H:%M:%S - " + result + "\n"))
    