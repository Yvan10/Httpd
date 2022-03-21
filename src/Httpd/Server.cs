using System.Net.Sockets;
using System.Text;

namespace Httpd;

public class Server
{
    private TcpListener _listener;
    public int Port { set; get; } 
    public Server(int port)
    {
        Port = port;
        _listener =  TcpListener.Create(Port);
    }
    
    public async Task Start() 
    {
        _listener.Start(); 
        while (true)
        {
            var client2 = await _listener.AcceptTcpClientAsync();
            new Thread(()=>HandleRequest(client2)).Start();
        }
    }
    
    private void HandleRequest(TcpClient client)
    {
        Thread.Sleep(50);
        
        var stream = client.GetStream();
        var socket = stream.Socket; 
        var buffer = new byte[socket.Available]; 
        
        stream.Read(buffer, 0, buffer.Length);
        
        var data = Encoding.UTF8.GetString(buffer);
        Console.WriteLine(data);

        var response = "HTTP/1.1 200 OK\r\n";
        response += "Content-Length: 44";
        response += "Content-Type: text/html\r\n";
        response += "Connection: close\r\n";
        response += "\r\n";
        response += "<html><body><h1>It works!<h1></body></html\r\n";
        
        var responseBytes = Encoding.UTF8.GetBytes(response);
        socket.Send(responseBytes);
    }
}