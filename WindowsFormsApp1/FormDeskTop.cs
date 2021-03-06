﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.model;
using WindowsFormsApp1.Properties;

namespace WindowsFormsApp1
{
    public partial class FormDeskTop : Form
    {
        private int MinLeft = 0;
        private int MinTop = 0;
        private Size BmpSize;
        private FormPropertyModel formPropertyModel;
        private readonly object picLock = new object();

        private delegate void DelegateSetPicture(Bitmap image);
        DelegateSetPicture delSetPicture;
        public FormDeskTop(FormPropertyModel formPropertyModel)
        {
            InitializeComponent();
            this.formPropertyModel = formPropertyModel;
            this.delSetPicture = new DelegateSetPicture(this.SetPicture);
        }

        private void SetPicture(Bitmap image)
        {
            if (this.pictureBox1.InvokeRequired)
            {
                this.Invoke(this.delSetPicture, new object[] { image });
                return;

            }

            Image destroyBitMap = null;
            if (null != this.pictureBox1.Image)
            {
                destroyBitMap = this.pictureBox1.Image;
            }
            this.pictureBox1.Image = image;
            this.pictureBox1.Refresh();

            if (null != destroyBitMap)
            {
                destroyBitMap.Dispose();
            }
        }

        public void Cancel()
        {
            this.backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            Rectangle desRect = new Rectangle(0, 0, this.formPropertyModel.PictureSize.Width, this.formPropertyModel.PictureSize.Height);
            
            while (!this.backgroundWorker1.CancellationPending)
            {
                try
                {
                    Bitmap wallPaperBmp = new Bitmap(this.BmpSize.Width, this.BmpSize.Height);
                    using (Graphics graphics = Graphics.FromImage(wallPaperBmp))
                    {
                        foreach (Screen screen in Screen.AllScreens)
                        {

                            Point dwowPoint = new Point(Math.Abs(this.MinLeft) + screen.Bounds.Location.X, Math.Abs(this.MinTop) + screen.Bounds.Location.Y);
                            graphics.CopyFromScreen(new Point(screen.Bounds.Left, screen.Bounds.Top), dwowPoint, screen.Bounds.Size);
                        }
                        Point mousePoint = new Point(Math.Abs(this.MinLeft) + MousePosition.X, Math.Abs(this.MinTop) + MousePosition.Y);

                        graphics.DrawImage(Resources.Cursol, mousePoint);
                    }
                    this.SetPicture(wallPaperBmp);

                    //切り取る部分の範囲を決定する。マウスの位置を中心とする
                    Rectangle srcRect = new Rectangle(Math.Abs(this.MinLeft) + MousePosition.X - this.formPropertyModel.PictureSize.Width / 2, Math.Abs(this.MinTop) + MousePosition.Y - this.formPropertyModel.PictureSize.Height / 2, this.formPropertyModel.PictureSize.Width, this.formPropertyModel.PictureSize.Height);
                    Bitmap controlBitMap = new Bitmap(this.formPropertyModel.PictureSize.Width, this.formPropertyModel.PictureSize.Height);
                    using (Graphics graphics = Graphics.FromImage(controlBitMap))
                    {
                        graphics.DrawImage(wallPaperBmp, desRect, srcRect, GraphicsUnit.Pixel);
                    }

                   this.formPropertyModel.DelegateSetPicture(controlBitMap);

                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void FormDeskTop_Load(object sender, EventArgs e)
        {
            // 上下左右のMAXを取得し四角形を作成する
            int minLeft = 0;
            int minTop = 0;
            int maxRight = 0;
            int maxBottom = 0;

            int calcBottom = 0;
            int calcRight = 0;
            foreach (Screen screen in Screen.AllScreens)
            {
                if (minLeft > screen.Bounds.Location.X)
                {
                    minLeft = screen.Bounds.Location.X;
                }

                if (minTop > screen.Bounds.Location.Y)
                {
                    minTop = screen.Bounds.Location.Y;
                }

                calcRight = screen.Bounds.Location.X + screen.Bounds.Width;
                calcBottom = screen.Bounds.Location.Y + screen.Bounds.Height;


                if (maxRight < calcRight)
                {
                    maxRight = calcRight;
                }

                if (maxBottom < calcBottom)
                {
                    maxBottom = calcBottom;
                }
            }

            this.BmpSize = new Size(Math.Abs(minLeft) + Math.Abs(maxRight), Math.Abs(minTop) + Math.Abs(maxBottom));

            this.MinLeft = minLeft;
            this.MinTop = minTop;

            this.backgroundWorker1.RunWorkerAsync();
        }
    }
}
