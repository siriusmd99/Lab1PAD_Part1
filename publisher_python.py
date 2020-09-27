import socket
import requests
import time
import json


def js():
    response = requests.get("https://meme-api.herokuapp.com/gimme")
    jsobj = json.dumps(response.json())
    return jsobj 

s = socket.socket()
port = 1337
ip = "127.0.0.1"
s.connect((ip, port))
conected = True
print("connected")
while True:

    try:
        s.send(bytes(js(),encoding="utf-8"))
        print("send data!!!!")
        time.sleep(5)
    except socket.error:
        conected = False
        s = socket.socket()
        print("lost connection reconecting")
        while not conected:
            try: 
                s.connect((ip,port))
                conected = True
                print("succes")
            except socket.error:
                time.sleep(2)


