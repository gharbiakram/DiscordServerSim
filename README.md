# DiscordServerSim

An App that simulates the functionalites of a Discord Server

(1)"DiscordServerSimUI": contains the UI code (using WPF)
-it refrences the ConnectionCore solution , gaining access to the server and client logic.
-it won't be able to connect to the server directly , it must use a client to do so.
-each app instance will be accompanied with a client so it can communicate with the server.
NOTE: the design is still in an early version , scaling it or changing the entirity of it is still an option

(2)"Connections": contains the ServerClient core logic (Inside the ConnectionCore solution)
-will run seperatly from the UI , because the server needs to keep on listening to clients
-contains both the logic for the server (ServerCore) and client (ClientCore)
-the server will be the only one with access to manipulate data from the DataBase , adding Security
NOTE:The Logic and performance of the server/clients relationship is not performance focused to solve that :
-i plan to start using web sockets (after the initial app is done).
