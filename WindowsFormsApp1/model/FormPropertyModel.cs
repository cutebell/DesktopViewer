using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1.model
{
    public class FormPropertyModel
    {
        public Form1.DelegateSetPicture DelegateSetPicture
        {
            get;
        }
        public Size PictureSize
        {
            get; set;
        }

        public Bitmap newPicture
        {
            get; set;
        }

        public FormPropertyModel(Form1.DelegateSetPicture delegateSetPicture)
        {
            this.DelegateSetPicture = delegateSetPicture;
        }

    }
}
