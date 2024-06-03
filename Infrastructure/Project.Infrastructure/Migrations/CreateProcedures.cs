using FluentMigrator;

namespace Project.Infrastructure.Migrations
{
    [Migration(2)]
    public class CreateProcedures : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                    CREATE FUNCTION dbo.FN_Direction
                    (
                        @piX FLOAT, @piY FLOAT,
                        @pjX FLOAT, @pjY FLOAT,
                        @pkX FLOAT, @pkY FLOAT
                    )
                    RETURNS FLOAT
                    AS
                    BEGIN
                        RETURN (@pkX - @piX) * (@pjY - @piY) - (@pjX - @piX) * (@pkY - @piY);
                    END");

            Execute.Sql(@"
                    CREATE FUNCTION dbo.FN_OnSegment
                    (
                        @piX FLOAT, @piY FLOAT,
                        @pjX FLOAT, @pjY FLOAT,
                        @pkX FLOAT, @pkY FLOAT
                    )
                    RETURNS BIT
                    AS
                    BEGIN
                        IF (@pkX BETWEEN CASE WHEN @piX < @pjX THEN @piX ELSE @pjX END AND CASE WHEN @piX > @pjX THEN @piX ELSE @pjX END AND
                            @pkY BETWEEN CASE WHEN @piY < @pjY THEN @piY ELSE @pjY END AND CASE WHEN @piY > @pjY THEN @piY ELSE @pjY END)
                            RETURN 1;  -- True
                    
                        RETURN 0;  -- False
                    END");

            Execute.Sql(@"
                    CREATE FUNCTION dbo.FN_Intersects
                    (
                        @p1X FLOAT, @p1Y FLOAT, @p2X FLOAT, @p2Y FLOAT,
                        @p3X FLOAT, @p3Y FLOAT, @p4X FLOAT, @p4Y FLOAT
                    )
                    RETURNS BIT
                    AS
                    BEGIN
                        DECLARE @d1 FLOAT, @d2 FLOAT, @d3 FLOAT, @d4 FLOAT;
                    
                        -- Calculate directions
                        SET @d1 = dbo.FN_Direction(@p3X, @p3Y, @p4X, @p4Y, @p1X, @p1Y);
                        SET @d2 = dbo.FN_Direction(@p3X, @p3Y, @p4X, @p4Y, @p2X, @p2Y);
                        SET @d3 = dbo.FN_Direction(@p1X, @p1Y, @p2X, @p2Y, @p3X, @p3Y);
                        SET @d4 = dbo.FN_Direction(@p1X, @p1Y, @p2X, @p2Y, @p4X, @p4Y);
                    
                        -- Check if the segments straddle each other
                        IF ((@d1 > 0 AND @d2 < 0 OR @d1 < 0 AND @d2 > 0) AND
                            (@d3 > 0 AND @d4 < 0 OR @d3 < 0 AND @d4 > 0))
                            RETURN 1;  -- True
                    
                        -- Check for collinearity
                        IF (@d1 = 0 AND dbo.FN_OnSegment(@p3X, @p3Y, @p4X, @p4Y, @p1X, @p1Y) = 1 OR
                            @d2 = 0 AND dbo.FN_OnSegment(@p3X, @p3Y, @p4X, @p4Y, @p2X, @p2Y) = 1 OR
                            @d3 = 0 AND dbo.FN_OnSegment(@p1X, @p1Y, @p2X, @p2Y, @p3X, @p3Y) = 1 OR
                            @d4 = 0 AND dbo.FN_OnSegment(@p1X, @p1Y, @p2X, @p2Y, @p4X, @p4Y) = 1)
                            RETURN 1;  -- True
                    
                        RETURN 0;  -- False
                    END");
            
            Execute.Sql(@"
                    CREATE PROCEDURE dbo.SP_GetAllIntersectedRectangles
                    @segmentStartX FLOAT,
                    @segmentStartY FLOAT,
                    @segmentEndX FLOAT,
                    @segmentEndY FLOAT
                AS
                BEGIN
                    SET NOCOUNT ON;
                
                    -- Temporary table to store intersecting rectangles
                    CREATE TABLE #IntersectingRectangles
                    (
                        RectangleId INT
                    );
                
                    -- Cursor to iterate through rectangles
                    DECLARE @rectangleId INT;
                    DECLARE cur CURSOR FOR 
                        SELECT Id FROM Rectangles;
                
                    OPEN cur;
                
                    FETCH NEXT FROM cur INTO @rectangleId;
                
                    WHILE @@FETCH_STATUS = 0
                    BEGIN
                        DECLARE @trX FLOAT, @trY FLOAT, @tlX FLOAT, @tlY FLOAT,
                                @brX FLOAT, @brY FLOAT, @blX FLOAT, @blY FLOAT;
                
                        -- Fetch points for the current rectangle
                        SELECT 
                            @trX = tr.X, @trY = tr.Y,
                            @tlX = tl.X, @tlY = tl.Y,
                            @brX = br.X, @brY = br.Y,
                            @blX = bl.X, @blY = bl.Y
                        FROM Rectangles r
                        JOIN Points tr ON r.TopRightPointId = tr.Id
                        JOIN Points tl ON r.TopLeftPointId = tl.Id
                        JOIN Points br ON r.BottomRightPointId = br.Id
                        JOIN Points bl ON r.BottomLeftPointId = bl.Id
                        WHERE r.Id = @rectangleId;
                
                        -- Check intersection for each segment of the rectangle
                        IF (dbo.FN_Intersects(@segmentStartX, @segmentStartY, @segmentEndX, @segmentEndY, @tlX, @tlY, @trX, @trY) = 1 OR
                            dbo.FN_Intersects(@segmentStartX, @segmentStartY, @segmentEndX, @segmentEndY, @trX, @trY, @brX, @brY) = 1 OR
                            dbo.FN_Intersects(@segmentStartX, @segmentStartY, @segmentEndX, @segmentEndY, @brX, @brY, @blX, @blY) = 1 OR
                            dbo.FN_Intersects(@segmentStartX, @segmentStartY, @segmentEndX, @segmentEndY, @blX, @blY, @tlX, @tlY) = 1)
                        BEGIN
                            INSERT INTO #IntersectingRectangles (RectangleId)
                            VALUES (@rectangleId);
                        END;
                
                        FETCH NEXT FROM cur INTO @rectangleId;
                    END;
                
                    CLOSE cur;
                    DEALLOCATE cur;
                
                    -- Return all intersecting rectangles
                    SELECT r.Id AS RectangleId, 
                           r.TopRightPointId AS TopRightPointId, 
                           r.TopLeftPointId AS TopLeftPointId, 
                           r.BottomRightPointId AS BottomRightPointId, 
                           r.BottomLeftPointId AS BottomLeftPointId
                    FROM Rectangles r
                    JOIN #IntersectingRectangles ir ON r.Id = ir.RectangleId;
                
                    DROP TABLE #IntersectingRectangles;
                END;
                    ");

        }

        public override void Down()
        {
            Execute.Sql(@"DROP PROCEDURE IF EXISTS dbo.SP_GetAllIntersectedRectangles;");
            Execute.Sql(@"DROP PROCEDURE IF EXISTS dbo.FN_Intersects;");
            Execute.Sql(@"DROP PROCEDURE IF EXISTS dbo.FN_OnSegment;");
            Execute.Sql(@"DROP PROCEDURE IF EXISTS dbo.FN_Direction;");
        }
    }
}
