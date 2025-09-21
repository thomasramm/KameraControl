using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using System.Windows;

public static class HttpHelper
{
    // Hex-Befehle laut SMTAV-Dokumentation
    static byte[] powerOnCommand = { 0x81, 0x01, 0x04, 0x00, 0x02, 0xFF };
    static byte[] powerOffCommand = { 0x81, 0x01, 0x04, 0x00, 0x03, 0xFF };
    static string setPositionCommand = "http://{ip}/cgi-bin/ptzctrl.cgi?ptzcmd&poscall&{position}";
    static int panspeed = 8;
    static int tiltspeed = 8;
    static int zoomspeed = 5;

    public static async Task CameraPosition(string ip, string position)
    {
        if (position.Length > 1)
            return;

        string urlCommand = setPositionCommand.Replace("{ip}",ip).Replace("{position}", position);
        await CameraSendCommandByHttp(urlCommand);
    }
    public static async Task CameraPowerOn(string ip, int port)
    {
        await CameraSendCommandByTcp(ip, port, powerOnCommand);
    }

    public static async Task CameraPowerOff(string ip, int port)
    {
        await CameraSendCommandByTcp(ip, port, powerOffCommand);
    }

    public enum MoveCommand
    {
        up,
        down,
        left,
        right,
        zoomin,
        zoomout
    }

    public static async Task CameraMove(string ip, MoveCommand direction)
    {
        string urlCommand = $"http://{ip}/cgi-bin/ptzctrl.cgi?ptzcmd&{direction}&{panspeed}&{tiltspeed}";
        await CameraSendCommandByHttp(urlCommand);
    }

    public static async Task CameraZoom(string ip, MoveCommand zoomCommand)
    {
        string urlCommand = $"http://{ip}/cgi-bin/ptzctrl.cgi?ptzcmd&{zoomCommand}&{zoomspeed}";  
        await CameraSendCommandByHttp(urlCommand);
    }

    private static async Task CameraSendCommandByHttp(string url)
    {
        using HttpClient client = new();
        try
        {
            await client.GetAsync(url);
        }
        catch (Exception ex)
        {
            Debug.Print($"Fehler beim Senden: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private static async Task CameraSendCommandByTcp(string ip, int port, byte[] command)
    {
        try
        {
            using TcpClient client = new TcpClient();
            await client.ConnectAsync(ip, port);
            using NetworkStream stream = client.GetStream();
            await stream.WriteAsync(command, 0, command.Length);        
        }
        catch (Exception ex)
        {
            Debug.Print($"Fehler beim Senden: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

