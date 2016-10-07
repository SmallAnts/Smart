using System.Web.Mvc;

namespace Smart.Samples.Web.Areas.AceHelp
{
    public class AceHelpAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AceHelp";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AceHelp_default",
                "AceHelp/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}