# NoteMeSenpai
note taking discord bot

# How to run

dont forget to init submodules.

create a api.key file in the NoteMeSenpai folder and put your API secret in it.
then execute the command "dotnet run"

# How to run with docker

create a api.key file in the docker folder and put your API secret in it.
then execute the command "docker build . --tag notemesenpai" to build the image.
wait for completion and run "docker run --name nmsBot notemesenpai"

# Bot usage

Save notes on users “-note {ID/uniqueName} {Note}”
Response Example: 
Note on {ID/uniqueName} saved

Show notes on a certain users “-notes {ID}”
Response Example: 
Note #XY by {noteCreator} on {DateTime}:
{SomeNote}
Note #XY+1 by {noteCreator} on {DateTime}:
{SomeNote}
…

Show all notes “-noteall”
Response Example:
Note #1 by {noteCreator} on {DateTime} for {user}:
{SomeNote}
Note #2 by {noteCreator} on {DateTime} for {user}:
{SomeNote}
…

Delete a specific note “-delnote {ID/uniqueName} {NoteID}”
Response Example:
Note #XY by {noteCreator} on {DateTime}:
{SomeNote}
DELETED

Delete all notes for user “-deleteallnotes {ID/uniqueName}”
Requires special Role
Response Example:
{NumberOfNotes} Notes on {ID/uniqueName} DELETED

# Administrative commands

addrole, removerole, addchannel, setdefaultchannel, removechannel