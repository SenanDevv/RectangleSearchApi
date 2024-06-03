using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Core.Models
{
    [Table("Rectangles")]
    public class RectangleModel
    {
        public int Id { get; set; }
        public PointModel TopRightPoint { get; set; }
        public PointModel TopLeftPoint { get; set; }
        public PointModel BottomRightPoint { get; set; }
        public PointModel BottomLeftPoint { get; set; }
    }
}
