using AutoMapper;
using MovieLibrary.API.DTOs.Requests.MyList;
using MovieLibrary.API.DTOs.Responses.MyList;
using MovieLibrary.API.Interfaces.Repositories;
using MovieLibrary.API.Interfaces.Services;
using MovieLibrary.API.Models;

namespace MovieLibrary.API.Services
{
  public class MyListService : IMyListService
  {
    private readonly IMyListRepository _myListRepository;
    private readonly ITitleRepository _titleRepository;
    private readonly IMapper _mapper;

    public MyListService(IMyListRepository myListRepository, ITitleRepository titleRepository, IMapper mapper)
    {
      _myListRepository = myListRepository;
      _titleRepository = titleRepository;
      _mapper = mapper;
    }

    public async Task<IEnumerable<MyListResponseDto>> GetAllAsync()
    {
      var myList = await _myListRepository.GetAllAsync();
      return _mapper.Map<IEnumerable<MyListResponseDto>>(myList);
    }

    public async Task<MyListResponseDto?> GetByIdAsync(int id)
    {
      var myListItem = await _myListRepository.GetByIdAsync(id);
      return myListItem == null ? null : _mapper.Map<MyListResponseDto>(myListItem);
    }

    public async Task<IEnumerable<MyListResponseDto>> GetByUserIdAsync(int userId)
    {
      var myList = await _myListRepository.GetByUserIdAsync(userId);
      return _mapper.Map<IEnumerable<MyListResponseDto>>(myList);
    }

    public async Task<MyListResponseDto> AddToMyListAsync(AddToMyListDto dto)
    {
      var titleExists = await _titleRepository.ExistsAsync(dto.TitleId);
      if (!titleExists)
        throw new KeyNotFoundException($"Title with ID {dto.TitleId} not found");

      var existingItem = await _myListRepository.GetByUserAndTitleAsync(dto.UserId, dto.TitleId);
      if (existingItem != null)
        throw new ArgumentException("This title is already in your list");

      var myListItem = new MyList
      {
        UserId = dto.UserId,
        TitleId = dto.TitleId,
        Status = dto.Status,
        AddedAt = DateTime.UtcNow,
        StatusUpdatedAt = DateTime.UtcNow
      };

      var createdItem = await _myListRepository.CreateAsync(myListItem);
      var itemWithDetails = await _myListRepository.GetByIdAsync(createdItem.Id);
      return _mapper.Map<MyListResponseDto>(itemWithDetails!);
    }

    public async Task<MyListResponseDto> UpdateStatusAsync(int id, UpdateMyListStatusDto dto)
    {
      var myListItem = await _myListRepository.GetByIdAsync(id);
      if (myListItem == null)
        throw new KeyNotFoundException($"My list item with ID {id} not found");

      myListItem.Status = dto.Status;
      myListItem.StatusUpdatedAt = DateTime.UtcNow;

      var updatedItem = await _myListRepository.UpdateAsync(myListItem);
      var itemWithDetails = await _myListRepository.GetByIdAsync(updatedItem.Id);
      return _mapper.Map<MyListResponseDto>(itemWithDetails!);
    }

    public async Task<bool> RemoveFromMyListAsync(int id)
    {
      return await _myListRepository.DeleteAsync(id);
    }
  }
}
