using APMASync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace ConsoleApp1
{
    internal static class Program
    {
        static void Main(string[] args)
        { 
            try
            {
                ApmaConnection c = new ApmaConnection();
                var reponse = c.AuthenticationResponse();
                var e=c.Authentication(reponse.Token);
                string startDate = "2022/08/01";
                string enddate = "2022/08/05";
                DateTime StartDate = Convert.ToDateTime(startDate);
                DateTime Enddate = Convert.ToDateTime(enddate);
                c.HcsRptSummary(e,"TIE", StartDate, Enddate);
                Console.WriteLine("Successful");
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }


        }
    }
}
