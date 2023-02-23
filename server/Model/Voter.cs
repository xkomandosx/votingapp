namespace votingapp.server.Model
{
  public class Voter
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool Voted { get; set; }
    public int CandidateId { get; set; }
    public string FullName => $"{FirstName} {LastName}";
  }
}
