using MDFusionLabHaute.DesktopSurface.Processes;
using MDFusionLabHaute.Domain.Entities;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.DesktopSurface.Helper
{
    public class SignalRConnectionHelper
    {
        #region Declaration
        public static string _hubURL = System.Configuration.ConfigurationSettings.AppSettings["HubURL"];
        public static string destinationFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MDFusionLabHaute");
        public static string machineDataFileName = destinationFile + "\\" + "machinedata.json";
        #endregion

        #region Methods
        public static async void Connect_To_Server()
        {
            await ConnectAsync();
        }

        public static async System.Threading.Tasks.Task ConnectAsync()
        {
            Machine machine = new Machine();
            machine = getMachineData();
            PublicFile._signalRConnection = new HubConnectionBuilder().WithUrl(_hubURL).Build();
            PublicFile._signalRConnection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await PublicFile._signalRConnection.StartAsync();

                await PublicFile._signalRConnection.InvokeAsync("SetUserID", "M-" + machine.ID.ToString());
            };

            try
            {
                await PublicFile._signalRConnection.StartAsync();
                await PublicFile._signalRConnection.InvokeAsync("SetUserID", "M-" + machine.ID.ToString());

                PublicFile._signalRConnection.On<string>("DispenseNow", (parameter) => DispenseProcess.DispenseNow($"{parameter}"));
            }
            catch (Exception ex)
            {
                Connect_To_Server();
            }
        }

        public static Machine getMachineData()
        {
            Machine machine = new Machine();
            // Check if the file exists
            if (File.Exists(machineDataFileName))
            {
                // Read the JSON data from the file
                string jsonData = File.ReadAllText(machineDataFileName);

                // Deserialize the JSON data into a C# object
                machine = JsonConvert.DeserializeObject<Machine>(jsonData);
                Console.WriteLine("ID: " + machine.ID);
                Console.WriteLine("MachineRegNo: " + machine.MachineRegNo);
            }
            else
            {
                Console.WriteLine("File does not exist: " + machineDataFileName);
            }
            return machine;
        }
        #endregion
    }
}
