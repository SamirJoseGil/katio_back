using katio.Data.Dto;
using katio.Data.Models;

namespace katio.Business.Interfaces;

public interface INarratorService
{
    Task<BaseMessage<Narrator>> Index();
    Task<BaseMessage<Narrator>> GetNarratorById(int Id);
    Task<BaseMessage<Narrator>> GetNarratorsByName(string Name);
    Task<BaseMessage<Narrator>> GetNarratorsByLastName(string LastName);
    Task<BaseMessage<Narrator>> GetNarratorsByGenre(string Genre);
    Task<BaseMessage<Narrator>> CreateNarrator(Narrator narrator);
    Task<BaseMessage<Narrator>> UpdateNarrator(Narrator narrator);
    Task<BaseMessage<Narrator>> DeleteNarrator(int id);
    
}
