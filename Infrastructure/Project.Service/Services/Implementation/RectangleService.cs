using Microsoft.Data.SqlClient;
using Project.Core.Utilities.Results;
using Project.Infrastructure.Repositories.Abstraction;
using Project.Infrastructure.SpModels;
using Project.Service.Dtos;
using Project.Service.Services.Abstraction;
using System.Net;

namespace Project.Service.Services.Implementation
{
    public class RectangleService : IRectangleService
    {
        private readonly IRectangleRepository _rectangleRepository;

        public RectangleService(IRectangleRepository rectangleRepository)
        {
            _rectangleRepository = rectangleRepository;
        }

        public Result GetAllRectangles(SearchRectangleBySegmentDto segment)
        {
            var result = new Result();
            try
            {
                var sqlParameters = new List<SqlParameter>();
                sqlParameters.AddParam("segmentStartX", segment.FirstPair.X);
                sqlParameters.AddParam("segmentStartY", segment.FirstPair.Y);
                sqlParameters.AddParam("segmentEndX", segment.SecondPair.X);
                sqlParameters.AddParam("segmentEndY", segment.SecondPair.Y);

                var rectangles = EfDbTools.ExecuteProcedure<SP_GetAllIntersectedRectanglesResult>("dbo.SP_GetAllIntersectedRectangles", sqlParameters);

                result.Data = rectangles;
                result.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
                result.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return result;
        }

        public async Task<Result> GetRectangleAsync(int id)
        {
            var result = new Result();

            try
            {
                var rectangle = await _rectangleRepository.GetAsync(r => r.Id == id);
                result.Data = rectangle;
                result.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
                result.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            return result;
        }
    }
}