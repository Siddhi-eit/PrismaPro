using MDFusionLabHaute.Domain.Entities;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDFusionLabHaute.DesktopSurface
{
    public class PublicFile
    {
        public static HubConnection _signalRConnection;
        public static HubConnection hubConnection
        {
            get
            {
                return _signalRConnection;
            }
            set
            {
                _signalRConnection = value;
            }
        }

        public static Users _user = new Users();
        public static Users users
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
            }
        }

        public static Int32? _ApplicationMode;
        public static Int32? ApplicationMode
        {
            get
            {
                return _ApplicationMode;
            }
            set
            {
                _ApplicationMode = value;
            }
        }


        public static Machine _machine = new Machine();
        public static Machine machine
        {
            get
            {
                return _machine;
            }
            set
            {
                _machine = value;
            }
        }
    }
}