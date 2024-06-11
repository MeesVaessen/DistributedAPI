using Api.Model;
using Fleck;
using Newtonsoft.Json;
using System.Net.WebSockets;

namespace Api
{
    public class WebSocketHandler
    {
        private readonly List<IWebSocketConnection> _wsConnections = new List<IWebSocketConnection>();
        private readonly Dictionary<string, IWebSocketConnection> _tokenToWebSocketMap = new Dictionary<string, IWebSocketConnection>();
        //public readonly Dictionary<string, (IWebSocketConnection connection, string fileName, string password)> TokenData = new Dictionary<string, (IWebSocketConnection connection, string fileName, string password)>();
        private WebSocketServer? _server;
        public string test { get; set; }
        public Dictionary<string, IWebSocketConnection> TokenToWebSocketMap { get => _tokenToWebSocketMap; }

        public WebSocketHandler()
        {
        }

        public void StartServer()
        {
            _server = new WebSocketServer("wss://0.0.0.0:8181");
            _server.Start(ws =>
            {
                ws.OnOpen = () =>
                {
                    var token = Guid.NewGuid().ToString();
                    _wsConnections.Add(ws);
                    _tokenToWebSocketMap[token] = ws;
                    // Send token to client as a message
                    ws.Send(JsonConvert.SerializeObject(new { Type = "Connection_Token", Token = token }));
                };
                ws.OnMessage = async (message) =>
                {
                    try
                    {
                        Console.WriteLine(message);
                        var parsedMessage = JsonConvert.DeserializeObject<Message>(message);
                        if (parsedMessage != null)
                        {
                            Console.WriteLine(parsedMessage);
                            var wsToken = parsedMessage.WsToken;
                            // Handle different message types
                            switch (parsedMessage.Type)
                            {
                                case "Puzzle_Updated":
                                    Console.WriteLine("Received Send_Update message");
                                    foreach (var wsClient in _wsConnections)
                                    {
                                        await wsClient.Send(message);
                                    }
                                    break;
                                case "Puzzle_Solved_Success":
                                    Console.WriteLine("Received Puzzle_Solved_Success message");
                                    foreach (var wsClient in _wsConnections)
                                    {
                                        await wsClient.Send(message);
                                    }
                                    break;
                                case "Status_Update":
                                    Console.WriteLine("Received Status_Update message");
                                    Console.WriteLine($"Tried Passwords: {parsedMessage.TriedPasswords}");
                                    Console.WriteLine($"Elapsed Time: {parsedMessage.ElapsedTime}");
                                    if (_tokenToWebSocketMap.TryGetValue(wsToken, out var client1))
                                    {
                                        await client1.Send(message);
                                    }
                                    break;
                                case "Password_Found":
                                    Console.WriteLine("Received Password_Found message");
                                    Console.WriteLine($"Content: {parsedMessage.Content}");
                                    if (_tokenToWebSocketMap.TryGetValue(wsToken, out var client2))
                                    {
                                        await client2.Send(message);
                                    }
                                    break;
                                /*case "Status_Update":
                                    Console.WriteLine("Received Status_Update message");
                                    Console.WriteLine($"Tried Passwords: {parsedMessage.TriedPasswords}");
                                    Console.WriteLine($"Elapsed Time: {parsedMessage.ElapsedTime}");
                                    foreach (var wsClient in _wsConnections)
                                    {
                                        await wsClient.Send(message);
                                    }
                                    break;*/
                                /*case "Password_Found":
                                    Console.WriteLine("Received Password_Found message");
                                    Console.WriteLine($"Content: {parsedMessage.Content}");
                                    foreach (var wsClient in _wsConnections)
                                    {
                                        await wsClient.Send(message);
                                    }
                                    break;*/
                                default:
                                    Console.WriteLine($"Received message of unknown type: {parsedMessage.Type}");
                                    break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing message: {ex.Message}");
                    }
                };
                ws.OnClose = () =>
                {
                    _wsConnections.Remove(ws);
                };
                ws.OnError = (ex) =>
                {
                    Console.WriteLine($"WebSocket error: {ex.Message}");
                    // Remove the disconnected client from the list
                    _wsConnections.Remove(ws);
                };
            });
        }

        /*public async Task<string> SendFileMessage(string fileName)
        {
            try
            {
                // Connect to FTP, upload file, etc.

                // Upon successful upload, generate a token and associate it with the client
                var token = Guid.NewGuid().ToString(); // Generate a unique token
                _tokenConnections[token] = clientWebSocket; // Assuming clientWebSocket is the WebSocket connection
                _connectionTokens[clientWebSocket] = token;

                // Return the token to the client
                return token;
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }*/
        public async Task<bool> SendMessageAsync(string messageType, string wsToken, string messageContent)
        {
            try
            {
                if (_tokenToWebSocketMap.ContainsKey(wsToken))
                {
                    foreach (var connection in _wsConnections)
                    {
                        Console.WriteLine(connection.ConnectionInfo.ClientIpAddress);
                        var message = new Message(messageType, wsToken, messageContent);
                        var serializedMessage = JsonConvert.SerializeObject(message);
                        await Task.Run(() => connection.Send(serializedMessage)); // Send message asynchronously
                    }
                    return true;
                }
                else
                {
                    throw new ArgumentException("Invalid or missing token.");
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        /*public async Task<bool> SendMessageToSparkAsync(string messageType, string messageContent)
        {
            var ipAddress = "10.0.0.4";
            Console.WriteLine($"Sending message to {ipAddress}");
            try
            {
                foreach (var connection in _wsConnections)
                    Console.WriteLine(connection.ConnectionInfo.ClientIpAddress);

                var sparkClient = _wsConnections.Where(ws => ws.ConnectionInfo.ClientIpAddress == ipAddress).FirstOrDefault();
                if (sparkClient == null)
                {
                    Console.WriteLine("No sparkClient");
                    return false;
                }

                Console.WriteLine(sparkClient);
                Console.WriteLine(sparkClient.ConnectionInfo.ClientIpAddress);

                var message = new Message(messageType, messageContent);
                var serializedMessage = JsonConvert.SerializeObject(message);
                await Task.Run(() => sparkClient.Send(serializedMessage)); // Send message asynchronously
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }*/
    }
}
