using System.Text;
using Newtonsoft.Json;
using votingapp.server.Model;

namespace votingapp;

public interface IVotersService
{
  IList<Voter> GetAll();

  Voter GetById(int id);

  int Add(Voter record);

  void Update(Voter record);

  void Delete(int id);
}

public class VotersService : IVotersService
{
  private List<Voter> _voters;
  private int _newId;

  public VotersService()
  {
    _voters = JsonConvert.DeserializeObject<List<Voter>>(this.GetEmbeddedResource("voters.json"));
    _newId = _voters.Count;
  }

  public IList<Voter> GetAll() => _voters;

  public Voter GetById(int id) => _voters.FirstOrDefault(i => i.Id == id);

  public int Add(Voter record)
  {
    record.Id = ++_newId;
    _voters.Add(record);
    return record.Id;
  }

  public void Update(Voter record)
  {
    var idx = _voters.FindIndex(i => i.Id == record.Id);
    if (idx >= 0)
      _voters[idx] = record;
  }

  public void Delete(int id) => _voters.Remove(_voters.FirstOrDefault(i => i.Id == id));

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
