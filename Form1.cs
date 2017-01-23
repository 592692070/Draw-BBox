using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//F:/something/project/Draw BBox/drawBBox/data/raw/train/YFT/

namespace drawBBox
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }
        private int [] BBox_point=new int[100];
        private int BBox_id = 0,cur_pos_x=0,cur_pos_y=0;
        private int cur_img_w, cur_img_h;
        private string tb_str = "choosed data",cur_pic_name="",out_dir="";
        private string[] files;
        private int files_id = 0;


        private int str2int(string str)
        {
            int ans = 0;
            for(int i=0;i<str.Length;i++)
            {
                ans = ans * 10 + (str[i] - '0');
            }
            return ans;
        }
        private void save_label(string img_name)
        {
            int dot_id=img_name.LastIndexOf("."),xie_id=img_name.LastIndexOf("/");
            string label_name = out_dir + img_name.Substring(xie_id+1, dot_id-xie_id-1)+"_label.txt";
            //MessageBox.Show(label_name);
            string text = "";
            for(int i=0;i<BBox_id;i+=4)
            {
                text += BBox_point[i] + " " + BBox_point[i + 1] + " " + BBox_point[i + 2] + " " + BBox_point[i + 3] + "\n";
            }
            System.IO.File.WriteAllText(@label_name, text);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //pictureBox1.ImageLocation = openFileDialog1.FileName;
            //Image pic = Image.FromFile(openFileDialog1.FileName);
            //Image pic = Image.FromFile("F:/something/project/Draw BBox/drawBBox/data/fish.jpg");
            //Image pic = Image.FromFile("F:/something/project/Draw BBox/drawBBox/data/test_1.jpg");
            if (files_id >= files.Length) return;
            if(files_id>0) save_label(files[files_id-1]);
            BBox_id = 0;
            for(int i=0;i<100;i++)  BBox_point[i] = 0;

            Image pic = Image.FromFile(files[files_id++]);

            cur_pic_name = files[files_id - 1].Substring(files[files_id - 1].LastIndexOf('/') + 1);
            pictureBox1.Image = pic;
            int width = pic.Width;
            int heigh = pic.Height;
            
            cur_img_w = width;
            cur_img_h = heigh;
            Point pos = pictureBox1.PointToScreen(pictureBox1.Location);
            Console.WriteLine(pos);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //Point pos = new Point(e.X, e.Y);
            //draw_2line(pos);
             this.pictureBox1.Invalidate();      
            Graphics g = pictureBox1.CreateGraphics();
 
            Rectangle rec = new Rectangle(5, 5, 200, 25);
            g.DrawRectangle(new Pen(Color.Red), rec);
 
            Pen pen = new Pen(Brushes.White);
            g.DrawLine(pen, 0, e.Y, this.Width, e.Y);
            g.DrawLine(pen, e.X, 0, e.X, this.Height);

            cur_pos_x = e.Location.X;
            cur_pos_y = e.Location.Y;
            
            
            //Point p = e.Location;
            //string X = p.X.ToString();
            //string Y = p.Y.ToString();
            //MessageBox.Show(p.ToString(), X + Y); 

            this.pictureBox1.Update();//立即更新
            Application.DoEvents();          
            //<span style="color: #FF0000;">Point p = myPanel1.PointToClient(MousePosition);//这时这个点和e.X,e.Y不一样</span>            
            //Console.WriteLine("Mousemove客户区位置 X=" + p.X + " Y=" + p.Y);
            //Console.WriteLine("Mousemove赋值前e.X=" + e.X + "  lastX=" + lastX + "  e.Y=" + e.Y + "  lastY=" + lastY + "  remain=" + remained.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Image init_pic = Image.FromFile("fish.jpg");
        }

        private string tbToStr()
        {
            string ans="";
            ans += "current img "+cur_pic_name;
            ans += "\n\rbbox\n\r";
            for(int i=0;i<BBox_id;i+=4)
            {
                ans += "[("+BBox_point[i].ToString() + "," + BBox_point[i+1].ToString()+"),(";
                ans += BBox_point[i+2].ToString() + "," + BBox_point[i + 3].ToString() + ")]\n\r";
            }
            return ans;
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            BBox_point[BBox_id++] = cur_pos_x;
            BBox_point[BBox_id++] = cur_pos_y;
            if(BBox_id%4==0) textBox1.Text = tbToStr();
            //MessageBox.Show(cur_pos_x.ToString()+" "+cur_pos_y.ToString());   
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            //cur_pos_x = e.X;
            //cur_pos_y = e.Y;
            cur_pos_x = e.Location.X;
            cur_pos_y = e.Location.Y;
            //BBox_point[BBox_id] = cur_pos_x;
            //BBox_point[BBox_id++] = cur_pos_y;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show(e.KeyValue.ToString());
            //MessageBox.Show(cur_pos_x.ToString() + " " + cur_pos_y.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(BBox_id>0)
            {
                BBox_id-=2;
                BBox_point[BBox_id] = 0;
                BBox_point[BBox_id + 1] = 0;
                textBox1.Text=tbToStr();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            files_id = 0;
            files = System.IO.Directory.GetFiles(textBox2.Text, "*jpg");
              out_dir = "";
            out_dir+= textBox3.Text;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            save_label(files[files_id-1]);
          
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string text=textBox4.Text;
            int id = str2int(text);
            files_id = id - 1;
            if (files_id >= files.Length) return;
            
            BBox_id = 0;
            for (int i = 0; i < 100; i++) BBox_point[i] = 0;

            Image pic = Image.FromFile(files[files_id++]);

            cur_pic_name = files[files_id - 1].Substring(files[files_id - 1].LastIndexOf('/') + 1);
            pictureBox1.Image = pic;
            int width = pic.Width;
            int heigh = pic.Height;

            cur_img_w = width;
            cur_img_h = heigh;
            Point pos = pictureBox1.PointToScreen(pictureBox1.Location);
            Console.WriteLine(pos);
        }
        


    }
}
