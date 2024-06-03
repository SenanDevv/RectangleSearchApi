using Project.Service.ViewModels;

namespace Project.Service.Dtos
{
    public class RectangleDto
    {
        public int Id { get; set; }
        public PointDto TopRightPoint { get; set; }
        public PointDto TopLeftPoint { get; set; }
        public PointDto BottomRightPoint { get; set; }
        public PointDto BottomLeftPoint { get; set; }
    }
}
