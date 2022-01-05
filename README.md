# Chat Room

## Simple real time chat room
### This project allows multiple users to chat in a chat room and also get stock quotes using a specific command.

* User interface using Blazor
* Real time messages using SignalR
* Messages stored on MongoDB
* Declouped bot to process stock commands using RabbitMQ

## Requirements
1. MongoDB
2. RabbitMQ

## Setup
It is possible to resolve dependencies using docker to create MongoDB and RabbitMQ containers.

```bash
# Clone this repository
$ git clone https://github.com/gomespauloho/chat

# In the project's root path, run this command to start the containers using docker
docker-compose up -d

# Restore NuGet packages:

dotnet restore

# Browse to the /src/Chat path and run the application

$ cd src/Chat

$ dotnet run

# The default urls for this application are:

http://localhost:5000

https://localhost:5001
````

## Shutdown

```bash
# Browse to the /src/Chat path and stop the application

$ dotnet stop

# In the project's root path run this command to shutdown the containers

$ docker-compose down
```

## Licence

MIT