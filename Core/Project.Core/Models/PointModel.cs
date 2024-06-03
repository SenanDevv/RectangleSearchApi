using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Core.Models
{
    [Table("Points")]
    public class PointModel
    {
        public int Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public PointModel(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
