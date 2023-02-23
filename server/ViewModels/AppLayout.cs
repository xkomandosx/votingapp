using System.Security.Claims;
using DotNetify;
using DotNetify.Routing;
using DotNetify.Security;

namespace votingapp;

[Authorize]
public class AppLayout : BaseVM, IRoutable
{
   private enum Route
   {
      Home,
      Dashboard,
      CandidatePage,
      VoterPage
  };

  //public static string ThirdPagePath => "Form";
  public static string FormPagePath => "Form";
  public RoutingState RoutingState { get; set; }

   public object Menus => new List<object>()
      {
         new { Title = "Dashboard",    Icon = "assessment", Route = this.GetRoute(nameof(Route.Dashboard)) },
         new { Title = "Candidate Page",   Icon = "grid_on",    Route = this.GetRoute(nameof(Route.CandidatePage)) },
         new { Title = "Voter Page",   Icon = "grid_on",    Route = this.GetRoute(nameof(Route.VoterPage)) }
         //new { Title = "Third Page",    Icon = "web",        Route = this.GetRoute(nameof(Route.ThirdPage), $"{ThirdPagePath}/1") },
      };

   public string UserName { get; set; }
   public string UserAvatar { get; set; }

   public AppLayout(IPrincipalAccessor principalAccessor)
   {
      var userIdentity = principalAccessor.Principal.Identity as ClaimsIdentity;

      UserName = userIdentity.Name;
      UserAvatar = userIdentity.Claims.FirstOrDefault(i => i.Type == ClaimTypes.Uri)?.Value;

      this.RegisterRoutes("/", new List<RouteTemplate>
            {
                new RouteTemplate(nameof(Route.Home)) { UrlPattern = "", ViewUrl = nameof(Route.Dashboard) },
                new RouteTemplate(nameof(Route.Dashboard)),
                new RouteTemplate(nameof(Route.CandidatePage)),
                new RouteTemplate(nameof(Route.VoterPage))
                //new RouteTemplate(nameof(Route.ThirdPage)) { UrlPattern = $"{ThirdPagePath}(/:id)" }
            });
   }
}
