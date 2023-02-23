namespace votingapp.server.Model
{
  public class Candidate
  {
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Votes { get; set; }
    public string FullName => $"{FirstName} {LastName}";
  }
}
