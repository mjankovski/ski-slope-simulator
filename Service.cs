using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

namespace WindowsFormsApp2
{
    class Service
    {
        static int[] sX = { 143, 417, 470 };
        static int[] sY = { 272, 218, 370 };

        public int i, x, y;

        ReaderWriterLockSlim rw;
        private System.Windows.Forms.PictureBox service;

        public Service(ReaderWriterLockSlim rw, System.Windows.Forms.PictureBox service, int i)
        {
            this.rw = rw;
            this.service = service;
            this.service.Visible = true;
            this.i = i;
        }

        public void Start()
        {
            while (true)
            {
                rw.EnterWriteLock();

                x = sX[i];
                y = sY[i];
                Random rand3 = new Random();
                Thread.Sleep(8337);

                x = 1500;
                y = 1500;

                rw.ExitWriteLock();

                Thread.Sleep(rand3.Next(7777)+4000);
            }
        }
    }
}
