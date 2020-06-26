# Note Me Senpai
a simple note taking discord bot

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

Save notes on users “-note {ID/uniqueName} {Note}”
Response Example: 
```
Note on {ID/uniqueName} saved
```

Show notes on a certain users “-notes {ID}”

Response Example: 
```
Note #XY by {noteCreator} on {DateTime}:
{SomeNote}

Note #XY+1 by {noteCreator} on {DateTime}:
{SomeNote}
…
```

Show all notes “-noteall”

Response Example:
```
Note #1 by {noteCreator} on {DateTime} for {user}:
{SomeNote}

Note #2 by {noteCreator} on {DateTime} for {user}:
{SomeNote}
…
```
Delete a specific note “-delnote {ID/uniqueName} {NoteID}”

Response Example:
```
Note #XY by {noteCreator} on {DateTime}:
{SomeNote}
DELETED
```

Delete all notes for user “-deleteallnotes {ID/uniqueName}”

Response Example:
```
{NumberOfNotes} Notes on {ID/uniqueName} DELETED
```

# Administrative commands

addrole, removerole, addchannel, setdefaultchannel, removechannel, showconfig, version, update, purge
