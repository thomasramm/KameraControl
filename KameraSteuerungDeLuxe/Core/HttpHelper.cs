using KameraSteuerungDeLuxe.Model;
using OnvifDiscovery;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using System.Windows;

namespace KameraSteuerungDeLuxe.Core
{
    public static class HttpHelper
    {
        private static readonly byte[] powerOffCommand = [0x81, 0x01, 0x04, 0x00, 0x03, 0xFF];

        // Hex-Befehle laut SMTAV-Dokumentation
        private static readonly byte[] powerOnCommand = [0x81, 0x01, 0x04, 0x00, 0x02, 0xFF];

        public enum MoveCommand
        {
            up,
            down,
            left,
            right,
            zoomin,
            zoomout,
            ptzstop,
            zoomstop,
        }

        public static async Task CameraMove(string ip, MoveCommand direction, int speed)
        {
            var panspeed = speed;
            var tiltspeed = speed;
            string urlCommand = $"http://{ip}/cgi-bin/ptzctrl.cgi?ptzcmd&{direction}&{panspeed}&{tiltspeed}";
            await CameraSendCommandByHttp(urlCommand);
        }

        public static async Task CameraPosition(string ip, string position)
        {
            if (position.Length > 1)
                return;

            string urlCommand = $"http://{ip}/cgi-bin/ptzctrl.cgi?ptzcmd&poscall&{position}";
            await CameraSendCommandByHttp(urlCommand);
        }

        public static async Task CameraPowerOff(string ip, int port)
        {
            await CameraSendCommandByTcp(ip, port, powerOffCommand);
        }

        public static async Task CameraPowerOn(string ip, int port)
        {
            await CameraSendCommandByTcp(ip, port, powerOnCommand);
        }

        public static async Task CameraZoom(string ip, MoveCommand zoomCommand, int speed)
        {
            var zoomspeed = speed;
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
                using TcpClient client = new();
                await client.ConnectAsync(ip, port);
                using NetworkStream stream = client.GetStream();
                await stream.WriteAsync(command);
            }
            catch (Exception ex)
            {
                Debug.Print($"Fehler beim Senden: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static async Task<List<Camera>> SearchCamera()
        {
            List<Camera> cameras = [];

            var discovery = new Discovery();

            using CancellationTokenSource cts = new();

            await foreach (var device in discovery.DiscoverAsync(1, cts.Token))
            {
                var c = new Camera(device.Model, device.Address);
                cameras.Add(c);
            }

            return cameras;
        }
    }
}