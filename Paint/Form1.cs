using System.Drawing;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {
        bool freehand = true, rectangle = false, ellipse = false;
        static SolidBrush brush = new SolidBrush(Color.Black);
        Pen myPen = new Pen(brush,1);

        Point lastPoint; // Null
        bool isMouseDown = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            lastPoint = Point.Empty;
            isMouseDown = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            lastPoint = e.Location;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown) // Проверка находится ли мышь на PictureBox'e (Начали ли мы рисовать) 
            {
                if(lastPoint != null) // Если мы рисуем в PictureBox
                {
                    if (freehand) // Если режим от руки
                    {
                        //Если до момента рисования картинки нет (Мы ничего не рисовали), создаём эту самую картинку - bitmap
                        if(pictureBox1.Image == null)
                        {
                            Bitmap bmp = new Bitmap(pictureBox1.Width,pictureBox1.Height);
                            pictureBox1.Image = bmp;
                        }
                        //using - директива создания объекта, для которой потом будет вызван метод Dispose();
                        //метод Dispose() - это метод, который выгружает память, в котором был затрачен этот объект.
                        //То есть, как только данный код у нас закончится метод Dispose вызовется и память освободиться
                        using(Graphics g = Graphics.FromImage(pictureBox1.Image))
                        {
                            g.DrawLine(myPen,lastPoint,e.Location);
                            pictureBox1.Invalidate();
                        }
                    } 
                }
            }
            lastPoint = e.Location;
        }



    }
}
