using System;
using System.Reactive.Linq;
using DotNetify;
using DotNetify.Routing;
using DotNetify.Security;

namespace votingapp;

[Authorize]
public class Dashboard : BaseVM, IRoutable
{
    public RoutingState RoutingState { get; set; }
    private readonly ICandidatesService _candidatesService;
    private readonly IVotersService _votersService;

    public Dashboard(ICandidatesService candidatesService, IVotersService votersService)
    {
      _candidatesService = candidatesService;
      _votersService = votersService;
    }

    //public override void OnSubVMCreated(BaseVM vm)
    //{
    //  if (vm is CandidateTable)
    //  {
    //    var candidateList = vm as CandidateTable;
    //    candidateList.Candidates = _candidatesService.GetAll();
    //    candidateList.Pages;

    //  }
    //  else if (vm is VoterTable)
    //  {
    //    var voterList = vm as VoterTable;
    //    voterList.Voters = _votersService.GetAll();
    //    voterList.Pages;
    //  }
    //}

}
