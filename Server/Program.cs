using Server;
using SuperSimpleTcp;

Console.WriteLine("");

Database.Initialize();

SimpleTcpServer server = new SimpleTcpServer(System.Net.IPAddress.Any.ToString(), 25566);
CommandSender.Initialize(ref server);

Server.ServerLogic logic = new Server.ServerLogic(server);
server.Start();

Console.WriteLine("Server started on "+ System.Net.IPAddress.Any.ToString() + ":25566");

string command = "";
while (command != "stop")
{
    command = Console.ReadLine();
}

logic.Stop();
server.Stop();
Database.Close();
Console.WriteLine("Server stopped");