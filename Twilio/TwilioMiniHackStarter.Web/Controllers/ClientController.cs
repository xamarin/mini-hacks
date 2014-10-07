using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Twilio;

namespace TwilioMiniHackStarter.Web.Controllers
{
    public class ClientController : Controller
    {
        public ActionResult Token()
        {
			var capabilities = new TwilioCapability (
				                   "REPLACE_WITH_YOUR_TWILIO_ACCOUNT_SID", 
				                   "REPLACE_WITH_YOUR_TWILIO_AUTH_TOKEN");

			capabilities.AllowClientIncoming ("REPLACE_WITH_A_CLIENT_NAME");
			capabilities.AllowClientOutgoing ("REPLACE_WITH_YOUR_TWIML_APPLICATION_SID");
			var token = capabilities.GenerateToken ();

            return Content (token);
        }
    }
}
