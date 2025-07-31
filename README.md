# Guess the Sender

## About

Guess the Sender is a little game where you try to guess who sent a message in a WhatsApp group chat, based only on the content and time of the message.

This project is not affiliated with or endorsed by WhatsApp or its parent company. All trademarks and copyrights are the property of their respective owners.

Messages can be uploaded to this site for gameplay, but all processing happens locally in your browser. No messages are ever stored or sent to any server.

This page was built using Blazor.

## How to play

### Home page

<img width="1920" height="962" alt="image" src="https://github.com/user-attachments/assets/2c8a0c31-a085-49e2-ba15-8952c7333f57" />

From the start page you first need to get to the upload page. It can be reached over the navigation on the left site or by following the link in the warning box.

### Upload page

<img width="1920" height="962" alt="image" src="https://github.com/user-attachments/assets/08d3b77d-c55e-4d8a-ab72-99b910075a46" />

On the upload page, you can select a groupchat text file, which you need to generate within WhatsApp. You can find a tutorial on how to do this within the page.
Before uploading your chat, you can select filter options. These include:
- Banned users (Their messages will be skipped when collecting the messages)
- Min. and max. message lengths
- Start and end for the time period you want to include

You may be warned that your chat is too big, which will happen if you surpass the maximum size of 10GB. In case this happens, you need to manually remove entries from your file.

If you get an error talking about not being able to find a valid parser for your messages, create a github issue containing the format of your messages text file.
WhatsApp can produce many different formats when exporting a chat, so only a few are suppported at the moment.

### Game

<img width="1920" height="962" alt="image" src="https://github.com/user-attachments/assets/0da687bb-4d05-4fd0-a795-159d8483599c" />

After you upload your messages, you can begin the game. The message will be displayed as a card with the sender removed. You can guess who sent the message using the buttons on the right of the card.

<img width="1628" height="274" alt="image" src="https://github.com/user-attachments/assets/5cc392b0-d827-497d-9834-b9d345793d23" />

After your guess, the card will be pushed to the stack where it reveals if you guessed correctly or not. In case of failure, the right name will be given next to your wrong answear.

<img width="1633" height="522" alt="image" src="https://github.com/user-attachments/assets/5d6da486-3017-4a94-b9a2-2281aeecbd70" />

After 10 messages you can see your results and the stack of all messages you encountered

