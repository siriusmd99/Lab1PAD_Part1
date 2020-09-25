using Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Config.ConnectionInfo;
namespace Subscriber
{
    public partial class Form1 : Form
    {
        Subscriber subscriber;
        private int tooltip_lastX;
        private int tooltip_lastY;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            subscriber = new Subscriber(radioButton1.Text, this);
            subscriber.Connect(BrokerIP, BrokerPort);
            radioButton1.Checked = true;
        }

        public void UpdateMeme(Meme meme)
        {
            pictureBox2.Load(meme.url);
            pictureBox2.Tag = meme;
        }

        private void PictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            Meme meme = (Meme) pictureBox2.Tag;
            if(meme !=  null)
            {
                if (e.X != tooltip_lastX || e.Y != tooltip_lastY)
                {
                    toolTip1.SetToolTip(pictureBox2, $"Title: {meme.title}\nAuthor: {meme.author}");

                    tooltip_lastX = e.X;
                    tooltip_lastY = e.Y;
                }

            }
               
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked && subscriber.subreddit != radioButton1.Text)
                subscriber.Subscribe(radioButton1.Text);
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked && subscriber.subreddit != radioButton2.Text)
                subscriber.Subscribe(radioButton2.Text);
        }

        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked && subscriber.subreddit != radioButton3.Text)
                subscriber.Subscribe(radioButton3.Text);
        }
    }
}
