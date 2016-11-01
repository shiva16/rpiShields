# Twitter bot for Raspberry Pi with Sixfab GPRS Shield

Connect internet over Mobile and send tweets with image and text. You can use following transactions for work.

1. Firstly, Connect your Raspberry Pi to internet and run `sudo apt-get update` to update your Raspberry Pi
2. Make [Tutorial 2](https://github.com/sixfab/rpiShields/tree/master/tutorials/tutorial2) to connect internet over Mobile.
3. Register a Twitter App with [this](https://iag.me/socialmedia/how-to-create-a-twitter-app-in-8-easy-steps/) tutorial
4. Install Python and required libraries with following commands
  1. `sudo apt-get install python-setuptools`
  2. `sudo easy_install pip`
  3. `sudo pip install twython`
5. Create python application with following commands
  1. `mkdir Tiwitting`<br/>
  2. `cd Tiwitting`<br/>
  3. `sudo nano Tiwitting.py`<br/>
    ```
    #!/usr/bin/env python
    import sys
    from twython import Twython
    CONSUMER_KEY = '***************YOUR DATA*****************'
    CONSUMER_SECRET = '***************YOUR DATA*****************'
    ACCESS_KEY = '***************YOUR DATA*****************'
    ACCESS_SECRET = '***************YOUR DATA*****************'

    api = Twython(CONSUMER_KEY,CONSUMER_SECRET,ACCESS_KEY,ACCESS_SECRET) 

    api.update_status(status=sys.argv[1])
    ```
    Hit Ctrl-X, and press Y to exit and save the file. 
  4. Make it executable with the `sudo chmod +x Tiwitting.py` command 
  5. Send tweet with `python Tiwitting.py 'Hello Everyone, this is my Raspberry Pi tweeting you more nonsense'` command
