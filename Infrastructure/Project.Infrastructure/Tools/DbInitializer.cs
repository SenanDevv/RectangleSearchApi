using Project.Core.Models;
using Project.Core.Settings;
using Project.Infrastructure.DAL;

namespace Project.Infrastructure.Tools
{
    public static class DbInitializer
    {
        private static Random _random = new Random();

        public async static Task SeedAsync(AppDbContext context)
        {
            if (context.Rectangles.Any())
            {
                return;
            }

            var rectangles = GenerateRectangles(AppSettings.Settings.RectangleCount);
            await context.Rectangles.AddRangeAsync(rectangles);
            await context.SaveChangesAsync();
        }

        private static List<RectangleModel> GenerateRectangles(int rectangleCount)
        {
            var rectangles = new List<RectangleModel>();
            const int minValue = -10;
            const int maxValue = 10;

            while (rectangles.Count < rectangleCount)
            {
                var topLeftPoint = GeneratePoint(minValue, maxValue);
                var bottomRightPoint = GeneratePoint(minValue, maxValue);

                var topRightPoint = new PointModel(bottomRightPoint.X, topLeftPoint.Y);
                var bottomLeftPoint = new PointModel(topLeftPoint.X, bottomRightPoint.Y);

                var rectangle = new RectangleModel
                {
                    TopLeftPoint = topLeftPoint,
                    TopRightPoint = topRightPoint,
                    BottomLeftPoint = bottomLeftPoint,
                    BottomRightPoint = bottomRightPoint
                };

                if (!IsValidRectangle(rectangle))
                {
                    continue;
                }

                rectangles.Add(rectangle);
            }

            return rectangles;
        }

        private static PointModel GeneratePoint(int min, int max)
        {
            double x = _random.Next(min, max + 1);
            double y = _random.Next(min, max + 1);

            return new PointModel(x, y);
        }

        public static bool IsValidRectangle(RectangleModel rectangle)
        {
            // Vectors for the sides of the rectangle
            var vectorTop = new Vector(rectangle.TopRightPoint.X - rectangle.TopLeftPoint.X, rectangle.TopRightPoint.Y - rectangle.TopLeftPoint.Y);
            var vectorBottom = new Vector(rectangle.BottomRightPoint.X - rectangle.BottomLeftPoint.X, rectangle.BottomRightPoint.Y - rectangle.BottomLeftPoint.Y);
            var vectorLeft = new Vector(rectangle.BottomLeftPoint.X - rectangle.TopLeftPoint.X, rectangle.BottomLeftPoint.Y - rectangle.TopLeftPoint.Y);
            var vectorRight = new Vector(rectangle.BottomRightPoint.X - rectangle.TopRightPoint.X, rectangle.BottomRightPoint.Y - rectangle.TopRightPoint.Y);

            // Check if opposite sides are equal in length
            bool oppositeSidesEqual = vectorTop.Length == vectorBottom.Length && vectorLeft.Length == vectorRight.Length;

            // Check if adjacent sides are perpendicular
            bool rightAngles = ArePerpendicular(vectorTop, vectorRight) &&
                               ArePerpendicular(vectorRight, vectorBottom) &&
                               ArePerpendicular(vectorBottom, vectorLeft) &&
                               ArePerpendicular(vectorLeft, vectorTop);

            return oppositeSidesEqual && rightAngles;
        }

        private static bool ArePerpendicular(Vector vector1, Vector vector2)
        {
            // Two vectors are perpendicular if their dot product is zero
            return Math.Abs(vector1.X * vector2.X + vector1.Y * vector2.Y) < 1e-10;
        }

        private class Vector
        {
            public double X { get; }
            public double Y { get; }

            public Vector(double x, double y)
            {
                X = x;
                Y = y;
            }

            public double Length => Math.Sqrt(X + X + Y * Y);
        }
    }
}
