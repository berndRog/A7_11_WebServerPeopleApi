using System.Threading.Tasks;
namespace PeopleApi.Core;

public interface IDataContext {
   Task<bool> SaveAllChangesAsync(); 
   void       ClearChangeTracker();
   void       LogChangeTracker(string text);
}