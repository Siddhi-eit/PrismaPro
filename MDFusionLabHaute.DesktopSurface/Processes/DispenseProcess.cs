using MDFusionLabHaute.Domain.Entities;
using Microsoft.AspNetCore.SignalR.Client;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Point2D = iTextSharp.awt.geom.Point2D; // Alias for the Point2D class

namespace MDFusionLabHaute.DesktopSurface.Processes
{
    public class DispenseProcess
    {
        #region Methods

        public static void DispenseNow(string parameter)
        {
            try
            {
                Dispenser.FmDispense myDispenser = new Dispenser.FmDispense();
                Dispenser.FmDispensedFormula myDispenserFM = new Dispenser.FmDispensedFormula();

                var parameterArray = parameter.Split('/');
                var array = parameterArray[6].Split(',');
                var array1 = parameterArray[7].Split('-');
                string[] myComps;
                string[] myAmounts;
                int myResult = 0;
                int myIntegerResult;
                myComps = new string[array.Count()];
                myAmounts = new string[array1.Count()];

                for (int i = 0; i < array.Count(); i++)
                {
                    if (parameterArray[9] != parameterArray[3])
                    {
                        decimal newamount = Convert.ToDecimal(parameterArray[3].Replace('.', Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)));
                        var finalcoloramount = (Convert.ToDecimal(array1[i]) * newamount) / Convert.ToDecimal(parameterArray[9]);
                        if (parameterArray[4] == "2")
                        {
                            array1[i] = (finalcoloramount / 1000).ToString();
                        }
                        else
                        {
                            array1[i] = finalcoloramount.ToString();
                        }
                    }

                    myComps[i] = array[i].Trim();
                    if (parameterArray[4] == "2")
                    {
                        myAmounts[i] = (Convert.ToDecimal(array1[i].Trim().Replace('.', Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator)))).ToString();
                    }
                    else
                    {
                        myAmounts[i] = array1[i].Trim().Replace('.', Convert.ToChar(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator));
                    }
                }

                if (parameterArray[4] == "2")
                {
                    parameterArray[3] = (Convert.ToDecimal(parameterArray[3]) / 1000).ToString();
                }

                myIntegerResult = myDispenser.Init();
                myResult = myDispenser.DefineTask(Convert.ToDouble(parameterArray[3]), 0, 0, myComps, myAmounts);
                myResult = myDispenser.Dispense(true);

                string AlldispenseAmounts = "";
                foreach (var item in myAmounts)
                {
                    if (AlldispenseAmounts == "")
                    {
                        AlldispenseAmounts = (Convert.ToDecimal(item) * 1000).ToString();
                    }
                    else
                    {
                        AlldispenseAmounts = AlldispenseAmounts + "-" + (Convert.ToDecimal(item) * 1000).ToString();
                    }
                }

                string[] dispenseCompletedParm = new string[6];
                dispenseCompletedParm[0] = PublicFile._user.ID.ToString();
                dispenseCompletedParm[1] = parameterArray[3];
                dispenseCompletedParm[2] = JsonConvert.SerializeObject(myComps);
                dispenseCompletedParm[3] = AlldispenseAmounts;
                dispenseCompletedParm[4] = parameterArray[8];
                dispenseCompletedParm[5] = parameterArray[10];

                if (myResult == 0)
                {
                    PublicFile._signalRConnection.InvokeAsync("DispenseCompleted", dispenseCompletedParm);
                }
                else
                {
                    PublicFile._signalRConnection.InvokeAsync("DispenseError", PublicFile._user.ID.ToString(), parameterArray[8].ToString());
                }

                myDispenser.Shutdown();

                foreach (Process Proc in Process.GetProcesses())
                {
                    if (Proc.ProcessName.Equals("PRISMA~1"))  // Process Excel?
                        Proc.Kill();
                }
            }
            catch (Exception ex)
            {
                foreach (Process Proc in Process.GetProcesses())
                {
                    if (Proc.ProcessName.Equals("PRISMA~1"))  // Process Excel?
                        Proc.Kill();
                }
            }
        }

        #endregion
    }
}
