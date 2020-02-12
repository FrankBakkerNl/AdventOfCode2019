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
using AdventOfCode2019.Day18Helpers;

namespace Visualizer
{
    public partial class Day18MultiMaze : Form
    {

        PictureBox pictureBox1 = new PictureBox();
        private Bitmap imageBitmap;

        public Day18MultiMaze()
        {
            InitializeComponent();
            CreateBitmapAtRuntime();
            Run();



        }
        static float spacing = 12;
        static SizeF boxSize = new SizeF(spacing, spacing);


        private void Run()
        {
            var input = File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day18.txt").Split(Environment.NewLine);

            var multiMaze = Day18.UpdateMazeToMultiMaze(input);
            var l1Maze = new L1Maze(multiMaze);
            var l2Maze = new L2Maze(l1Maze);
            //var mazeRunner = new MultiMazeRunner(l2Maze, l2Maze.StartPositions, 0);
            var graphics = Graphics.FromImage(imageBitmap);
            DrawL1Maze(l1Maze, graphics);
            DrawL2Maze(l1Maze, l2Maze, graphics);
            pictureBox1.Dock = DockStyle.Fill;

        }

        private void DrawL1Maze(L1Maze l1Maze, Graphics graphics)
        {
            graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 100, 100, 100)), new RectangleF(new PointF(0,0), boxSize*81));
            foreach (var l1MazeCorridor in l1Maze.Corridors)
            {
                var start = PointFromLocation(l1MazeCorridor);
                var x = start + new Size(10,10);
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(128, 10, 10, 10)), new RectangleF(start, boxSize));
            }
        }
        
        private void DrawL2Maze(L1Maze l1Maze, L2Maze l2Maze, Graphics graphics)
        {

            var roots = l1Maze.StartPositions.Concat(l1Maze.Keys.Keys.Concat(l1Maze.Doors.Keys)).ToList();

            // assign an integer value to each root
            var reverseIndexMap = roots.Select((root, index) => (root, index)).ToDictionary(t => t.index, t => t.root);
            var indexMap = roots.Select((root, index) => (root, index)).ToDictionary(t => t.root, t => t.index);

            for (int i = 0; i < l2Maze.NeighborLookup.Length; i++)
            {
                var source = reverseIndexMap[i];
                foreach (var neighbor in l2Maze.NeighborLookup[i])
                {
                    if (neighbor.Target < i) continue;
                    var destination = reverseIndexMap[neighbor.Target];
                    graphics.DrawLine(Pens.Green, PointFromLocation(source) + boxSize /2, PointFromLocation(destination) + boxSize/2);

                    var xLabel = ((source.Item1 + destination.Item1) / 2f) * spacing;
                    var yLabel = ((source.Item2 + destination.Item2) / 2f) * spacing;

                    graphics.DrawString(neighbor.Length.ToString(), DefaultFont, Brushes.White, xLabel, yLabel);
                }
            }

            foreach (var start in l1Maze.StartPositions)
            {
                graphics.FillEllipse(Brushes.DarkOrange, new RectangleF(PointFromLocation(start), boxSize));
            }

            foreach (var door in l1Maze.Doors)
            {
                DrawNode(graphics, door , Brushes.OrangeRed);
            }

            foreach (var key in l1Maze.Keys)
            {
                DrawNode(graphics, key, Brushes.Blue);
            }
        }

        private void DrawNode(Graphics graphics, KeyValuePair<(int, int), uint> key, Brush brush)
        {
            var topLeft = PointFromLocation(key.Key);
            graphics.FillEllipse(brush, new RectangleF(PointFromLocation(key.Key), boxSize));

            var size = graphics.MeasureString(Letter(key.Value).ToUpper(), DefaultFont);
            var offset = (boxSize - size) / 2;
            graphics.DrawString(Letter(key.Value).ToUpper(), DefaultFont, Brushes.White, topLeft + offset);
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
