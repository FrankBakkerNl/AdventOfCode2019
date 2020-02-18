using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AdventOfCode2019;
using static AdventOfCode2019.Day20;

namespace Visualizer
{
    public partial class Day20Visualizer : Form
    {

        PictureBox pictureBox1 = new PictureBox();
        private Bitmap imageBitmap;

        public Day20Visualizer()
        {
            InitializeComponent();
            CreateBitmapAtRuntime();
            Run();



        }
        static float spacing = 6;
        static SizeF boxSize = new SizeF(spacing, spacing);


        private void Run()
        {
            var input = File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day20.txt").Split(Environment.NewLine);

            //var multiMaze = Day18.UpdateMazeToMultiMaze(input);
            var l1Maze = new L1Maze(input);
            var l2Maze = new L2Maze(l1Maze);
            //var mazeRunner = new MultiMazeRunner(l2Maze, l2Maze.StartPositions, 0);
            var graphics = Graphics.FromImage(imageBitmap);
            DrawL1Maze(l1Maze, graphics);
            DrawL2Maze(l1Maze, l2Maze, graphics);
            pictureBox1.Dock = DockStyle.Fill;

        }

        private void DrawL1Maze(L1Maze l1Maze, Graphics graphics)
        {
            var With = l1Maze.Corridors.Select(c => c.Item1).Max();
            var height = l1Maze.Corridors.Select(c => c.Item1).Max();
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 100, 100, 100)), new RectangleF(new PointF(0,0), new SizeF(With * spacing, height * spacing)));
            foreach (var l1MazeCorridor in l1Maze.Corridors)
            {
                var start = PointFromLocation(l1MazeCorridor);
                var x = start + new Size(10,10);
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 10, 10, 10)), new RectangleF(start, boxSize));
            }
        }
        
        private void DrawL2Maze(L1Maze l1Maze, L2Maze l2Maze, Graphics graphics)
        {

            var reverseIndexMap = l2Maze.IndexLookup.ToDictionary(kv => kv.Value, kv => kv.Key);

            for (int i = 0; i < l2Maze.NeighborLookup.Length; i++)
            {
                var source = reverseIndexMap[i];
                foreach (var neighbor in l2Maze.NeighborLookup[i])
                {
                    if (neighbor.Length != 1) continue;
                    if (neighbor.Target < i) continue;
                    var destination = reverseIndexMap[neighbor.Target];
                    graphics.DrawLine(Pens.Green, PointFromLocation(source) + boxSize /2, PointFromLocation(destination) + boxSize/2);

                    var xLabel = ((source.Item1 + destination.Item1) / 2f) * spacing;
                    var yLabel = ((source.Item2 + destination.Item2) / 2f) * spacing;

                    graphics.DrawString(neighbor.Length.ToString(), DefaultFont, Brushes.White, xLabel, yLabel);
                }
            }

            graphics.FillEllipse(Brushes.DarkBlue, new RectangleF(PointFromLocation(l1Maze.Start), boxSize));
            graphics.FillEllipse(Brushes.DarkOrange, new RectangleF(PointFromLocation(l1Maze.End), boxSize));

            //foreach (var portal in l1Maze.PortalMap)
            //{
            //    //var other = l1Maze.PortalMap[portal.Value.Destination];
            //    DrawNode(graphics, portal.Key , Brushes.OrangeRed);
            //    DrawNode(graphics, portal.Value.Destination , Brushes.OrangeRed);
            //    graphics.DrawLine(Pens.LightGray, PointFromLocation(portal.Key) + boxSize /2, PointFromLocation(portal.Value.Destination) + boxSize/2);

            //}

        }

        private void DrawNode(Graphics graphics, (int, int) location, Brush brush)
        {
            var topLeft = PointFromLocation(location);
            graphics.FillEllipse(brush, new RectangleF(topLeft, boxSize));

            //var size = graphics.MeasureString(Letter(key.Value).ToUpper(), DefaultFont);
            //var offset = (boxSize - size) / 2;
            //graphics.DrawString(Letter(key.Value).ToUpper(), DefaultFont, Brushes.White, topLeft + offset);
        }

        string Letter(uint bit)
        {
            for (int i = 0; i < 27; i++)
            {
                if ((bit >> i) == 1) return ((char) (i+'a')).ToString();
            }

            return "X";
        }

        PointF PointFromLocation((int, int) l) => new PointF(l.Item1 * boxSize.Width, l.Item2 * boxSize.Height);

        public void CreateBitmapAtRuntime()
        {
            pictureBox1.Size = new Size(8000, 8000);
            this.Controls.Add(pictureBox1);

            imageBitmap = new Bitmap(8000, 8000);
            Graphics flagGraphics = Graphics.FromImage(imageBitmap);
            flagGraphics.Clear(Color.Black);
            pictureBox1.Image = imageBitmap;
        }
    }
}
