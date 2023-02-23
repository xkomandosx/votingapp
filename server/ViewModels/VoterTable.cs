using System;
using System.Collections.Generic;
using System.Linq;
using DotNetify;
using DotNetify.Security;
using votingapp.server.Model;

namespace votingapp;

[Authorize]
public class VoterTable : MulticastVM
{
    private readonly IVotersService _votersService;
    private readonly int _recordsPerPage = 8;
    public VoterTable(IVotersService votersService)
    {
      _votersService = votersService;
    }
  // If you use CRUD methods on a list, you must set the item key prop name of that list
  // by defining a string property that starts with that list's prop name, followed by "_itemKey".
  public string Voters_itemKey => nameof(Voter.Id);

  public IEnumerable<Voter> Voters => Paginate(
      _votersService
          .GetAll());

  //public IEnumerable<Voter> Voters
  //{
  //  get => Get<IEnumerable<Voter>>();
  //  set => Set(value);
  //}
  public Action<string> Add => fullName =>
    {
       var names = fullName.Split(new char[] { ' ' }, 2);
       var newRecord = new Voter
       {
          FirstName = names.First(),
          LastName = names.Length > 1 ? names.Last() : ""
       };

       this.AddList(nameof(Voters), new Voter
       {
          Id = _votersService.Add(newRecord),
          FirstName = newRecord.FirstName,
          LastName = newRecord.LastName
       });

       SelectedPage = GetPageCount(_votersService.GetAll().Count);
    };

   public Action<Voter> Update => changes =>
    {
       var record = _votersService.GetById(changes.Id);
       if (record != null)
       {
          record.FirstName = changes.FirstName ?? record.FirstName;
          record.LastName = changes.LastName ?? record.LastName;
          _votersService.Update(record);

          ShowNotification = true;
       }
    };

   public Action<int> Remove => id =>
    {
       _votersService.Delete(id);
       this.RemoveList(nameof(Voters), id);

       ShowNotification = true;
       Changed(nameof(SelectedPage));
       Changed(nameof(Voters));
    };

   // Whether to show notification that changes have been saved.
   // Once this property is accessed, it will revert itself back to false.
   private bool _showNotification;

   public bool ShowNotification
   {
      get
      {
         var value = _showNotification;
         _showNotification = false;
         return value;
      }
      set
      {
         _showNotification = value;
         Changed(nameof(ShowNotification));
      }
   }

   public int[] Pages
   {
      get => Get<int[]>();
      set
      {
         Set(value);
         SelectedPage = 1;
      }
   }

   public int SelectedPage
   {
      get => Get<int>();
      set
      {
         Set(value);
         Changed(nameof(Voters));
      }
   }

   private IEnumerable<Voter> Paginate(IEnumerable<Voter> employees)
   {
      // Use base method to check whether user has changed the SelectedPage property value by clicking a pagination button.
      if (this.HasChanged(nameof(SelectedPage)))
         return employees.Skip(_recordsPerPage * (SelectedPage - 1)).Take(_recordsPerPage);
      else
      {
         Pages = Enumerable.Range(1, GetPageCount(employees.Count())).ToArray();
         return employees.Take(_recordsPerPage);
      }
   }

   private int GetPageCount(int records) => (int)Math.Ceiling(records / (double)_recordsPerPage);

}
