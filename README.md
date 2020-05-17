## How to test the bot

### Create own bot 
1. You should as tester create your [own bot](https://www.sohamkamani.com/blog/2016/09/21/making-a-telegram-bot/) we should do this because nothing will work 
2. After creating bo you copy bot api key for example
```
777845702:AAFdPS_taJ3pTecEFv2jXkmbQfeOqVZGER
```
3. Paste this key to our aplication in file appsetting in TaraBot project (API) 


### Ngrok
Ngrok gives you the opportunity to access your local machine from a temporary subdomain provided by ngrok. This domain can later send to the telegram API as URL for the webhook.
Install ngrock from this page [ngrok - download](https://ngrok.com/download)
Telegram API only supports the ports 443, 80, 88 or 8443. Feel free to change the port in the config of the project.
After instaletion open ngrok file in our API project and start ngrok on port 8443.
```
ngrok http 8443 
```
From ngrok you get an URL to your local server. It’s important to use the https one.
You should paste ngrok link into appsettign and start our project after than everything will work
