using Project.Service.ViewModels;

namespace Project.Service.Dtos
{
    public class SearchRectangleBySegmentDto
    {
        public PointDto FirstPair { get; set; }
        public PointDto SecondPair { get; set; }
    }
}
