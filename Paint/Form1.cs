using System.Drawing;
using System.Windows.Forms;

namespace Paint
{
    public partial class Form1 : Form
    {
        bool freehand = true, rectangle = false, ellipse = false;
        static SolidBrush brush = new SolidBrush(Color.Black);
        Pen myPen = new Pen(brush,1);

        Point startPoint; // Null
        bool isMouseDown = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            
            if(rectangle || ellipse)
            {
                if(pictureBox1.Image == null)
                {
                    Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                    pictureBox1.Image = bmp;
                }
                using(Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    if (rectangle)
                    {
                        if(e.Location.X > startPoint.X && e.Location.Y > startPoint.Y)
                        {
                            g.DrawRectangle(myPen, startPoint.X,startPoint.Y, e.Location.X - startPoint.X, e.Location.Y - startPoint.Y);
                            pictureBox1.Invalidate();
                        }
                    }
                    if (ellipse)
                    {
                        g.DrawEllipse(myPen, startPoint.X, startPoint.Y, e.Location.X - startPoint.X, e.Location.Y - startPoint.Y);
                        
                    }
                    pictureBox1.Invalidate();
                }
            }
            startPoint = Point.Empty;
            isMouseDown = false;
        }

        private void numericUpDown1_ValueChanged(object sender, System.EventArgs e)
        {
            myPen.Width = (float)numericUpDown1.Value;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                myPen.Color = colorDialog1.Color;
                panel1.BackColor = colorDialog1.Color;
            }
        }

        private void radioButton_CheckedChanged(object sender, System.EventArgs e)
        {
            ChangeInstrument();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            
            if(!rectangle && !ellipse)
            {
                startPoint = e.Location;
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            if(pictureBox1.Image != null)
            {
                pictureBox1.Image = null;
            }
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            saveFileDialog1.Title = "Сохранить картинку";
            saveFileDialog1.FileName = "MyImage";
            saveFileDialog1.DefaultExt = "jpg";
            saveFileDialog1.Filter = "JPEG FILES(*.jpg)|*.jpg";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                pictureBox1.DrawToBitmap(bmp, new Rectangle(0,0,pictureBox1.Width,pictureBox1.Height));
                bmp.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }

        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            openFileDialog1.Title = "Открыть картинку";
            openFileDialog1.FileName = "MyImage";
            openFileDialog1.DefaultExt = "jpg";
            openFileDialog1.Filter = "JPEG FILES(*.jpg)|*.jpg";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Bitmap openedImg = new Bitmap(openFileDialog1.FileName);
                if(pictureBox1.Image == null)
                {
                    pictureBox1.Image = bmp;
                }
                using(Graphics g = Graphics.FromImage(pictureBox1.Image))
                {
                    g.DrawImage(openedImg,0,0,pictureBox1.Width,pictureBox1.Height);
                }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown) // Проверка находится ли мышь на PictureBox'e (Начали ли мы рисовать) 
            {
                if(startPoint != null) // Если мы рисуем в PictureBox
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
                            if(e.Button == MouseButtons.Left)
                            {
                                myPen.Color = panel1.BackColor;
                            }
                            else if(e.Button == MouseButtons.Right)
                            {
                                myPen.Color = Color.White;
                            }
                            g.DrawLine(myPen,startPoint,e.Location);
                            pictureBox1.Invalidate();
                        }
                    } 
                }
            }
            if (freehand)
            {
                startPoint = e.Location;
            }
        }

        private void ChangeInstrument()
        {
            if (radioButton1.Checked)
            {
                freehand = true;
                rectangle = ellipse = false;
            }
            else if (radioButton2.Checked)
            {
                rectangle = true;
                freehand = ellipse = false;
            }
            else if (radioButton3.Checked)
            {
                ellipse = true;
                freehand = rectangle = false;
            }
        }
    }
}
