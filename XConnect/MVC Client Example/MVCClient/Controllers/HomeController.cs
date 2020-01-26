using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVCClient.Models;
using MVCClient.Xconnect;
using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Collection.Model;

namespace MVCClient.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        public HomeController() : base()
        {
        }
        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }

        public IActionResult AchiveGoal()
        {
            GoalsCollection obj = new GoalsCollection();

            var cfg = base.xconnectObject;
            string CurrentIdentifier = User.Claims.Where(x => x.Type == "name").Select(x => x.Value).First();
            string Source = "MvcClient";
            try
            {
                Contact knownContact = XconnectService.GetContact(cfg, Source, CurrentIdentifier);

                var GoalsList=knownContact.Interactions.Where(x => x.Events.OfType<Goal>().Any()).Select(x=>x.Events.OfType<Goal>());
                foreach(var v in GoalsList)
                {
                    foreach(var m in obj.Goals)
                    {
                        if(v.Where(x => x.DefinitionId.ToString().ToUpperInvariant() == m.GoalID).Any())
                        {
                            m.isAchived = true;
                        }
                    }
                }
                
            }
            catch(Exception ex)
            {

            }
            return View(obj);
        }

        public IActionResult Trigger(string id)
        {
            var cfg=base.xconnectObject;
            string CurrentIdentifier = User.Claims.Where(x => x.Type == "name").Select(x => x.Value).First();
            string Source = "MvcClient";
            try
            {
                Contact knownContact = XconnectService.GetContact(cfg, Source, CurrentIdentifier);
                
                using (var client = new XConnectClient(cfg))
                {
                    if (knownContact == null)
                    {
                        knownContact = XconnectService.CreateContact(cfg, Source, CurrentIdentifier);
                        client.AddContact(knownContact);
                    }
                    
                    PersonalInformation _PersonalInformation = knownContact.GetFacet<PersonalInformation>();
                    if (_PersonalInformation == null)
                    {
                        PersonalInformation _personalInformation = new PersonalInformation()
                        {
                            FirstName = "John",
                            LastName = "Cena"
                        };
                        client.SetFacet<PersonalInformation>(knownContact, PersonalInformation.DefaultFacetKey, _personalInformation);
                    }

                    var offlineGoal = Guid.Parse(id);
                    var channelId = Guid.Parse("54771D90-BFFB-4532-ACE8-4D390F564A3D"); // "Other event" channel

                    // Create a new interaction for that contact
                    Interaction interaction = new Interaction(knownContact, InteractionInitiator.Brand, channelId, "");

                    // Create new Goal event and add into interaction
                    var xConnectEvent = new Goal(offlineGoal, DateTime.UtcNow);
                    interaction.Events.Add(xConnectEvent);

                    ////Add interaction to client
                    client.AddInteraction(interaction);

                    client.Submit();
                    TempData["Triggered"] = true;
                }
            }
            catch (Exception ex)
            {
                TempData["Triggered"] = "Exception: " + ex.Message.ToString();
            }


            return RedirectToAction("AchiveGoal");
        }
    }
}
