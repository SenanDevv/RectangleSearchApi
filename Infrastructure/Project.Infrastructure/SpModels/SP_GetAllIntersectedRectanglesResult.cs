namespace Project.Infrastructure.SpModels
{
    public class SP_GetAllIntersectedRectanglesResult
    {
        public int RectangleId { get; set; }
        public int TopRightPointId { get; set; }
        public int TopLeftPointId { get; set; }
        public int BottomRightPointId { get; set; }
        public int BottomLeftPointId { get; set; }
    }
}
