using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace IntervalTimer
{
    public class ArcUtil
    {
        public static PathGeometry ArkGeometry(Point center, double distance , double startDegrees, double stopDegrees, SweepDirection direction)
        {
            Point stop = MakePoint(stopDegrees, center, distance);//終点座標

            //IsLargeの判定、
            //開始角度から終了角度までが180度を超えていたらtrue、なければfalse
            double diffDegrees = (direction == SweepDirection.Clockwise) ? stopDegrees - startDegrees : startDegrees - stopDegrees;
            if (diffDegrees < 0) { diffDegrees += 360.0; }
            bool isLarge = (diffDegrees > 180) ? true : false;

            //ArcSegment作成
            var arc = new ArcSegment(stop, new Size(distance, distance), 0, isLarge, direction, true);

            //PathFigure作成
            var fig = new PathFigure();
            Point start = MakePoint(startDegrees, center, distance);//始点座標
            fig.StartPoint = start;//始点座標をスタート地点に
            fig.Segments.Add(arc);//ArcSegment追加

            //PathGeometry作成、PathFigure追加
            var pg = new PathGeometry();
            pg.Figures.Add(fig);
            return pg;
        }

        private static Point MakePoint(double degrees, Point center, double distance)
        {
            if (degrees >= 360) { degrees = 359.99; }
            var rad = Radian(degrees);
            var cos = Math.Cos(rad);
            var sin = Math.Sin(rad);
            var x = center.X + cos * distance;
            var y = center.Y + sin * distance;
            return new Point(x, y);
        }
        private static double Radian(double degree)
        {
            return Math.PI / 180.0 * degree;
        }
    }
}
