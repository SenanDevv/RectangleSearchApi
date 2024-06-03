using Project.Core.Utilities.Results;
using Project.Service.Dtos;

namespace Project.Service.Services.Abstraction
{
    public interface IRectangleService
    {
        Result GetAllRectangles(SearchRectangleBySegmentDto segment);
        Task<Result> GetRectangleAsync(int id);
    }
}
