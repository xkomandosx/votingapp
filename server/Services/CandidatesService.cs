using System.Text;
using Newtonsoft.Json;
using votingapp.server.Model;

namespace votingapp;

public interface ICandidatesService
{
  IList<Candidate> GetAll();

  Candidate GetById(int id);

  int Add(Candidate record);

  void Update(Candidate record);

  void Delete(int id);
}

public class CandidatesService : ICandidatesService
{
  private List<Candidate> _candidates;
  private int _newId;

  public CandidatesService()
  {
    _candidates = JsonConvert.DeserializeObject<List<Candidate>>(this.GetEmbeddedResource("candidates.json"));
    _newId = _candidates.Count;
  }

  public IList<Candidate> GetAll() => _candidates;

  public Candidate GetById(int id) => _candidates.FirstOrDefault(i => i.Id == id);

  public int Add(Candidate record)
  {
    record.Id = ++_newId;
    _candidates.Add(record);
    return record.Id;
  }

  public void Update(Candidate record)
  {
    var idx = _candidates.FindIndex(i => i.Id == record.Id);
    if (idx >= 0)
      _candidates[idx] = record;
  }

  public void Delete(int id) => _candidates.Remove(_candidates.FirstOrDefault(i => i.Id == id));

  private string GetEmbeddedResource(string resourceName)
  {
    var assembly = GetType().Assembly;
    var name = assembly.GetManifestResourceNames().Where(i => i.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    if (string.IsNullOrEmpty(name))
      throw new FileNotFoundException();

    using (var reader = new StreamReader(assembly.GetManifestResourceStream(name), Encoding.UTF8))
      return reader.ReadToEnd();
  }
}
