using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using serviceReference = ConsoleApp1.ServiceReference1;
using System.Net;
using Newtonsoft.Json;
using ConsoleApp1.ServiceReference1;
using System.Xml;

namespace APMASync
{
    public class ApmaConnection
    {

        serviceReference.HybridCloudEngineSoapClient client = new serviceReference.HybridCloudEngineSoapClient();
        //apmahce.HybridCloudEngineSoapClient client = new apmahce.HybridCloudEngineSoapClient();
        //developer account APMA HCE
        string pmsUser = "Valk_ETL_22";
        string pmsPassword = "O86n9JPr8jFG";
        string pmsVendorId = "0013200001Dj3LD";
        string pmsCrsProperty = "VALKINT"; 
         
        public serviceReference.AuthenticationResponse AuthenticationResponse()
        {
            //properties
            string pmsToken = string.Empty;

            //request
            serviceReference.AuthenticationRequest authenticationRequest = new serviceReference.AuthenticationRequest
            {
                User = pmsUser,
                Password = pmsPassword,
                VendorId = pmsVendorId
            };
            //update TLS version
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            //response
            serviceReference.AuthenticationResponse authenticationResponse = client.HceAuthenticate(authenticationRequest);
            return authenticationResponse;
        }

        public serviceReference.Authentication Authentication(string pmsToken)
        {
            ApmaConnection acon = new ApmaConnection();
            serviceReference.Authentication authentication = new serviceReference.Authentication
            {
                User = pmsUser,
                Password = pmsPassword,
                VendorId = pmsVendorId,
                CrsProperty = pmsCrsProperty,
                Token = pmsToken
            };
            return authentication;
        }

        public string[] pmsGetProperties(ConsoleApp1.ServiceReference1.Authentication pmsAuthentication)
        {
            serviceReference.ListProperties ListProperties = client.HcsListProperties(pmsAuthentication, "", "");

            List<string> propertyList = new List<string>();
            foreach (ConsoleApp1.ServiceReference1.ListProperty item in ListProperties.Properties)
            {
                propertyList.Add(item.PropertyCode);
            }
            string[] properties = propertyList.ToArray();
            return properties;
        }

        public serviceReference.ReportManagementSummary HcsRptSummary(ConsoleApp1.ServiceReference1.Authentication pmsAuthentication, string pmsProperty, DateTime StartDate, DateTime EndDate)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            
            serviceReference.ReportManagementSummary reportManagementSummary = client.HcsReportManagementSummary(pmsAuthentication, PropertyCode: "TIE", StartDate: StartDate, EndDate: EndDate, "");
            var trimDate = JsonConvert.SerializeObject(reportManagementSummary, formatting: Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(trimDate);
            //insert into DB
            return reportManagementSummary;
        }

    }
}
