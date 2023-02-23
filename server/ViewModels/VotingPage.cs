using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using DotNetify;
using DotNetify.Security;
using votingapp.server.Model;

namespace votingapp;

[Authorize]
public class VotingPage : MulticastVM
{
    private readonly IVotersService _votersService;
    private readonly ICandidatesService _candidatesService;
    public VotingPage(ICandidatesService candidatesService, IVotersService votersService)
    {
      _candidatesService = candidatesService;
      _votersService = votersService;
    }

    public IEnumerable<Voter> Voters =>
         _votersService
             .GetAll();
    public IEnumerable<Candidate> Candidates =>
      _candidatesService
          .GetAll();
    public Action<Voting> Add => voting =>
    {
      var voter = _votersService.GetById(voting.VotedBy);
      var candidate = _candidatesService.GetById(voting.VotedFor);

      voter.Voted = true;
      voter.CandidateId = voting.VotedFor;
      _votersService.Update(voter);

      candidate.Votes = candidate.Votes + 1;
      _candidatesService.Update(candidate);
    };


}
