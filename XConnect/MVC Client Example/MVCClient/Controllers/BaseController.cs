using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCClient.Xconnect;
using Sitecore.XConnect.Client;

namespace MVCClient.Controllers
{
    public class BaseController : Controller
    {
        public  XConnectClientConfiguration xconnectObject { get; set; }
        public string Identifier { get; set; }
        public BaseController()
        {     
            try
            {
                //string Identifier = User.Claims.Where(x => x.Type == "name").Select(x => x.Value).First();
                xconnectObject = xConnectConfig.GetXconnectConfigObject();
                xconnectObject.Initialize();
            }
            catch(Exception ex)
            {
                //Log error in files
            }        
        }
    }
}