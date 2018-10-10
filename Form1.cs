using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        static ReaderWriterLockSlim rw1 = new ReaderWriterLockSlim();
        static ReaderWriterLockSlim rw2 = new ReaderWriterLockSlim();
        static ReaderWriterLockSlim rw3 = new ReaderWriterLockSlim();

        static SemaphoreSlim sem1 = new SemaphoreSlim(0, 8);
        static SemaphoreSlim sem2 = new SemaphoreSlim(4, 4);
        static SemaphoreSlim sem3 = new SemaphoreSlim(3, 3);

        Skier n1;
        Skier n2;
        Skier n3;
        Skier n4;
        Skier n5;
        Skier n6;
        Skier n7;
        Skier n8;

        Service serwis1;
        Service serwis2;
        Service serwis3;

        private static object o1 = new object();
        private static object o2 = new object();
        private static object o3 = new object();
        private static object o4 = new object();
        private static object o5 = new object();
        private static object o6 = new object();
        private static object parkB0 = new object();
        private static object parkB1 = new object();
        private static object parkB2 = new object();

        public Form1()
        {
            InitializeComponent();   

            serwis1 = new Service(rw1, service1, 0);
            Thread serwis1Th = new Thread(new ThreadStart(serwis1.Start));
            serwis2 = new Service(rw2, service2, 1);
            Thread serwis2Th = new Thread(new ThreadStart(serwis2.Start));
            serwis3 = new Service(rw3, service3, 2);
            Thread serwis3Th = new Thread(new ThreadStart(serwis3.Start));

            n1 = new Skier(rw1, rw2, rw3, sem1, sem2, sem3, o1, o2, o3, o4, o5, o6, parkB0, parkB1, parkB2);
            n2 = new Skier(rw1, rw2, rw3, sem1, sem2, sem3, o1, o2, o3, o4, o5, o6, parkB0, parkB1, parkB2);
            n3 = new Skier(rw1, rw2, rw3, sem1, sem2, sem3, o1, o2, o3, o4, o5, o6, parkB0, parkB1, parkB2);
            n4 = new Skier(rw1, rw2, rw3, sem1, sem2, sem3, o1, o2, o3, o4, o5, o6, parkB0, parkB1, parkB2);
            n5 = new Skier(rw1, rw2, rw3, sem1, sem2, sem3, o1, o2, o3, o4, o5, o6, parkB0, parkB1, parkB2);
            n6 = new Skier(rw1, rw2, rw3, sem1, sem2, sem3, o1, o2, o3, o4, o5, o6, parkB0, parkB1, parkB2);
            n7 = new Skier(rw1, rw2, rw3, sem1, sem2, sem3, o1, o2, o3, o4, o5, o6, parkB0, parkB1, parkB2);
            n8 = new Skier(rw1, rw2, rw3, sem1, sem2, sem3, o1, o2, o3, o4, o5, o6, parkB0, parkB1, parkB2);

            Thread n1Th = new Thread(new ThreadStart(n1.Start));
            Thread n2Th = new Thread(new ThreadStart(n2.Start));
            Thread n3Th = new Thread(new ThreadStart(n3.Start));
            Thread n4Th = new Thread(new ThreadStart(n4.Start));
            Thread n5Th = new Thread(new ThreadStart(n5.Start));
            Thread n6Th = new Thread(new ThreadStart(n6.Start));
            Thread n7Th = new Thread(new ThreadStart(n7.Start));
            Thread n8Th = new Thread(new ThreadStart(n8.Start));

            serwis1Th.Start();
            serwis2Th.Start();
            serwis3Th.Start();

            n1Th.Start();
            n2Th.Start();
            n3Th.Start();
            n4Th.Start();
            n5Th.Start();
            n6Th.Start();
            n7Th.Start();
            n8Th.Start();

            n1.slotNumber = 0;
            n2.slotNumber = 5;
            n3.slotNumber = 6;
            n4.slotNumber = 3;
            n5.slotNumber = 4;
            n6.slotNumber = 1;
            n7.slotNumber = 2;
            n8.slotNumber = 7;

            n1.SetSkierPosition(Skier.B0x[n1.slotNumber], Skier.B0y[n1.slotNumber]);
            n2.SetSkierPosition(Skier.B0x[n2.slotNumber], Skier.B0y[n2.slotNumber]);
            n3.SetSkierPosition(Skier.B0x[n3.slotNumber], Skier.B0y[n3.slotNumber]);
            n4.SetSkierPosition(Skier.B0x[n4.slotNumber], Skier.B0y[n4.slotNumber]);
            n5.SetSkierPosition(Skier.B0x[n5.slotNumber], Skier.B0y[n5.slotNumber]);
            n6.SetSkierPosition(Skier.B0x[n6.slotNumber], Skier.B0y[n6.slotNumber]);
            n7.SetSkierPosition(Skier.B0x[n7.slotNumber], Skier.B0y[n7.slotNumber]);
            n8.SetSkierPosition(Skier.B0x[n8.slotNumber], Skier.B0y[n8.slotNumber]);

            timer1.Start();
            timer1.Interval = 100;
        }

        private Point UpdatePos(Skier n)
        {
            return new Point(n.GetX(), n.GetY());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            n1box.Location = new Point(n1.x, n1.y);
            n2box.Location = new Point(n2.x, n2.y);
            n3box.Location = new Point(n3.x, n3.y);
            n4box.Location = new Point(n4.x, n4.y);
            n5box.Location = new Point(n5.x, n5.y);
            n6box.Location = new Point(n6.x, n6.y);
            n7box.Location = new Point(n7.x, n7.y);
            n8box.Location = new Point(n8.x, n8.y);

            service1.Location = new Point(serwis1.x, serwis1.y);
            service2.Location = new Point(serwis2.x, serwis2.y);
            service3.Location = new Point(serwis3.x, serwis3.y);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void n3box_Click(object sender, EventArgs e)
        {

        }

        private void n5box_Click(object sender, EventArgs e)
        {

        }

        private void n6box_Click(object sender, EventArgs e)
        {

        }
    }
}
