# How to run

You have two options to run this bot:
- The manual way
- Docker (recommended)

## General requirements
Create a Discord Application and add a Bot to it.
Create a api.key file in the `NoteMeSenpai` folder and put your API secret in it.

## Docker
Have docker installed on the host (https://www.docker.com/get-started)

Copy and paste the api.key file into the `docker` folder.

Then run the command `docker build . --tag notemesenpai` inside the `docker` folder.

Once it's finished execute the command `docker run --name nmsBot -d notemesenpai`

The bot should now listen to your commands.

To invite the Bot to a server look here (https://discordpy.readthedocs.io/en/latest/discord.html)

### The manual way

Have mongodb installed on the host (localhost only, no auth).

Write a script similar to `docker/entrypoint.sh` suiting your system to enable the automatic update functionality.

Run the bot with your script.

# Bot usage

Save notes on users "-note {ID/uniqueName} {Note}"
Response Example: 
```
Note on {ID/uniqueName} saved
```
<br/>
<br/>

Show notes on a certain users "-notes {ID}"

Response Example: 
```
Note #XY by {noteCreator} on {DateTime}:
{SomeNote}

Note #XY+1 by {noteCreator} on {DateTime}:
{SomeNote}
…
```
<br/>
<br/>

Show all notes "-noteall"

Response Example:
```
Note #1 by {noteCreator} on {DateTime} for {user}:
{SomeNote}

Note #2 by {noteCreator} on {DateTime} for {user}:
{SomeNote}
…
```
<br/>
<br/>

Delete a specific note "-delnote {ID/uniqueName} {NoteID}"

Response Example:
```
Note #XY by {noteCreator} on {DateTime}:
{SomeNote}
DELETED
```
<br/>
<br/>

Delete all notes for user "-deleteallnotes {ID/uniqueName}"

Response Example:
```
{NumberOfNotes} Notes on {ID/uniqueName} DELETED
```

# Administrative commands

Add a role with a command to the permission system "-addrole {RoleName} {OneOrMoreCommands}"

Response Example:
```
role {RoleName} added for {OneOrMoreCommands}
```
When {OneOrMoreCommands} is empty it adds the role to all commands.
<br/>
<br/>

Remove a command for a role from the permission system "-removerole {RoleName} {OneCommand}"

Response Example:
```
role {RoleName} removed for {OneCommand}
```
<br/>
<br/>

When {OneCommand} is empty it removes the role from all commands.

Add a channel to the valid response channels "-addchannel {ChannelName}"

Response Example:
```
channel {ChannelName} added as valid response channel
```
<br/>
<br/>

Remove a channel from the valid response channels "-removechannel {ChannelName}"

Response Example:
```
channel {ChannelName} removed from valid response channel
```
<br/>
<br/>

Set a channel as the default response channel "-setdefaultchannel {ChannelName}" 

Response Example:
```
channel {ChannelName} set as default response channel
```
<br/>
<br/>

Display the configuration "-showconfig"

Response Example:
```
DatabaseConnection = mongodb://localhost:27017
Prefixes = "-", "."
DeletionDelay = 60
Channels the bot is allowed to post in:

Channel1
Channel2 (default)
Channel3

Permissions:

Admin:
    -> *

RestrictedUser:
    -> note
    -> notes
    -> allnotes
    -> next
```
<br/>
<br/>

Show the current version of the bot "-version"

Response Example:
```
Version: 1.0.3.2
```
<br/>
<br/>

Update the bot to the latest version "-update" (only works if set up correctly or run with docker)

Response Example:
```
Bot is shutting down in {X} seconds to update.
```
<br/>
<br/>

Delete all messages sent by the bot from the server "-purge" (no response)

Set the prefix which the bot reacts to "-setprefix {OneOrMorePrefixes}" (no response)

Set the delay before a message by the bot is deleted "-setdeletiondelay {TimeInSeconds}"
