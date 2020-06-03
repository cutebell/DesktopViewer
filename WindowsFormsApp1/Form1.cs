using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private delegate void DelegateSetPicture(Bitmap image);
        private DelegateSetPicture DelegateSetPictureFunction;
        FormDeskTop formDeskTop;
        private readonly object picLock = new object();
        public Form1()
        {
            InitializeComponent();
            this.DelegateSetPictureFunction = new DelegateSetPicture(this.SetControlPicturePub);
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        public Size PictureSize
        {
            get
            {
                return this.pictureBox1.Size;
            }
        }

        public void SetControlPicturePub(Bitmap image)
        {
            if (this.pictureBox1.InvokeRequired)
            {
                lock (this.picLock)
                {
                    this.Invoke(this.DelegateSetPictureFunction, new object[] { image });
                }
                return;
            }
            Image destroyBitMap = null;
            if (null != this.pictureBox1.Image)
            {
                destroyBitMap = this.pictureBox1.Image;
            }
            this.pictureBox1.Image = image;

            if (null != destroyBitMap)
            {
                destroyBitMap.Dispose();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            this.formDeskTop = new FormDeskTop(this);
            this.formDeskTop.Show();
            this.button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.button2.Enabled = false;
            this.formDeskTop.Cancel();
            this.formDeskTop.Hide();
            this.button1.Enabled = true;
        }

        
    }
}
