using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdventOfCode2019;

namespace Visualizer
{
    public partial class Form1 : Form
    {
        bool closed;
        PictureBox pictureBox1 = new PictureBox();
        private Bitmap imageBitmap;

        public Form1()
        {
            InitializeComponent();

            CreateBitmapAtRuntime();
            Task.Run(()=> Run());
            this.Closed += (sender, e) => closed = true;
            this.AutoScroll = true;
            pictureBox1.DoubleClick += PictureBox1_DoubleClick;
        }

        private void Run()
        {
            var size = 100;
            var input = File.ReadAllText(@"C:\Users\Bakke\source\repos\AdventOfCode2019\AdventOfCode2019\Data\Day19.txt").Split(",").Select(int.Parse).ToArray();
            var analyser = new Day19.BeamAnalyzer(input);

            bool IsAffected(int r, int c)
            {
                var isAffected = analyser.IsAffected(r, c);
                // draw a pixel for each call
                if (!closed)
                    BeginInvoke((MethodInvoker) delegate { UpdateImage(c, r, isAffected ? Color.Green : Color.Red); });
                return isAffected;
            }


            var (col, row) = Day19.FindBoxStart(size, IsAffected);

            // draw a box around the result to verify
            for (int c1 = col-5; c1 <col+5 + size; c1++)
            {
                for (int r2 = row-5; r2 < row+5+size; r2++)
                {
                    IsAffected(c1, r2);
                }
                
            }

            if (!closed) BeginInvoke((MethodInvoker) delegate { DrawBox(col, row, size); });

        }

        private void DrawBox(int col, int row, int size)
        {
            var gr = Graphics.FromImage(imageBitmap);

            var brush = new HatchBrush(HatchStyle.BackwardDiagonal, Color.FromArgb(20, 0, 0, 255),
                Color.FromArgb(20, 128, 0, 0));
                gr.FillRectangle( brush, row, col, size, size);
                //ZoomTo(col, row, size);
        }

        private void PictureBox1_DoubleClick(object sender, EventArgs e)
        {
            var args = e as MouseEventArgs;
            Bitmap  newBitmap = new Bitmap(imageBitmap.Width, imageBitmap.Height);

            var gr = Graphics.FromImage(newBitmap);
            gr.InterpolationMode = InterpolationMode.NearestNeighbor;
            gr.DrawImage(imageBitmap,
                new Rectangle(0, 0, newBitmap.Width*10, newBitmap.Height*10),
                new Rectangle(0 ,0,  imageBitmap.Width/2, imageBitmap.Height/2), GraphicsUnit.Pixel);

            pictureBox1.Image = newBitmap;
            pictureBox1.AutoScrollOffset = new Point(args.X/2, args.Y/2);
        }

        private void ZoomTo(int col, int row, int size)
        {
            Bitmap  newBitmap = new Bitmap(imageBitmap.Width, imageBitmap.Height);

            var gr = Graphics.FromImage(newBitmap);
            gr.InterpolationMode = InterpolationMode.NearestNeighbor;
            gr.DrawImage(imageBitmap,
                new Rectangle(0, 0, newBitmap.Width, newBitmap.Height),
                new Rectangle(row - 10 , col - 10,  row + size + 10, col+size + 10), 
                GraphicsUnit.Pixel);

            pictureBox1.Image = newBitmap;
        }

        public void UpdateImage(int x, int y, Color color)
        {
            color = Color.FromArgb(128, color.R, color.G, color.B);
            if (x < 0 || y < 0 || x >imageBitmap.Width || y> imageBitmap.Width) return;
            imageBitmap.SetPixel(x,y,color);
            pictureBox1.Image = imageBitmap;
        }

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
