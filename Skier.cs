using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;

namespace WindowsFormsApp2
{
    class Skier
    {
        ReaderWriterLockSlim rw1;
        ReaderWriterLockSlim rw2;
        ReaderWriterLockSlim rw3;
        SemaphoreSlim sem1;
        SemaphoreSlim sem2;
        SemaphoreSlim sem3;

        private object o1;
        private object o2;
        private object o3;
        private object o4;
        private object o5;
        private object o6;

        private object parkB0;
        private object parkB1;
        private object parkB2;

        public int x;
        public int y;

        public int slotNumber;

        static int[] slotsB0 = { 0, 0, 0, 0, 0, 0, 0, 0 };
        static int[] slotsB1 = { 1, 1, 1, 1 };
        static int[] slotsB2 = { 1, 1, 1 };

        public static int[] B0x = { 318, 344, 370, 396, 318, 344, 370, 396 };
        public static int[] B0y = { 521, 521, 521, 521, 547, 547, 547, 547 };

        public static int[] B1x = { 218, 244, 270, 296 };
        public static int[] B1y = { 61, 61, 61, 61 };

        public static int[] B2x = { 569, 595, 621 };
        public static int[] B2y = { 213, 213, 213 };

        int condition;

        public Skier(ReaderWriterLockSlim rw1, ReaderWriterLockSlim rw2, ReaderWriterLockSlim rw3,
            SemaphoreSlim sem1, SemaphoreSlim sem2, SemaphoreSlim sem3, object o1, 
            object o2, object o3, object o4, object o5, object o6, object parkB0, object parkB1, object parkB2)
        {
            this.rw1 = rw1;
            this.rw2 = rw2;
            this.rw3 = rw3;
            this.sem1 = sem1;
            this.sem2 = sem2;
            this.sem3 = sem3;
            this.o1 = o1;
            this.o2 = o2;
            this.o3 = o3;
            this.o4 = o4;
            this.o5 = o5;
            this.o6 = o6;
            this.parkB0 = parkB0;
            this.parkB1 = parkB1;
            this.parkB2 = parkB2;
            condition = 1;
        }
        
        public void Start()
        {
            while (true)
            {
                Random rand2 = new Random();
                int choice = rand2.Next(2);
                switch (condition)
                {
                    case 1:
                        if (choice == 0) Go12();
                        else Go13();
                        break;

                    case 2:
                        if (choice == 0) Go23();
                        else Go21();
                        break;

                    case 3:
                        if (sem2.CurrentCount == 0) Go31();
                        else if (choice == 0) Go32();
                        else Go31();
                        break;

                    case 12:
                        if (this.y > 177) this.MoveSkier(0, -2);
                        else
                        {
                            this.ParkOnB1();
                            rw1.ExitReadLock(); //update
                            Thread.Sleep(4000);

                        }
                        break;

                    case 13:
                        if (this.y > 336) this.MoveSkier(0, -2);
                        else
                        {
                            this.ParkOnB2();
                            rw3.ExitReadLock();
                            Thread.Sleep(4000);
                        }
                        break;

                    case 21:
                        if (this.y < 457) this.MoveSkier(0, 6);
                        else
                        {
                            this.ParkOnB0();
                            Thread.Sleep(4000);
                        }
                        break;

                    case 23:
                        if (this.y < 315) this.MoveSkier(0, 6);
                        else
                        {
                            this.ParkOnB2();
                            Thread.Sleep(4000);
                        }
                        break;

                    case 32:
                        if (this.y > 175) this.MoveSkier(0, -2);
                        else
                        {
                            this.ParkOnB1();
                            rw2.ExitReadLock();
                            Thread.Sleep(4000);
                        }
                        break;

                    case 31:
                        if (this.y < 458) this.MoveSkier(0, 6);
                        else
                        {
                            this.ParkOnB0();
                            Thread.Sleep(4000);
                        }
                        break;
                }

                Thread.Sleep(40);
            }
        }

        public int GetX()
        {
            return this.x;
        }

        public int GetY()
        {
            return this.y;
        }

        public void MoveSkier(int dx, int dy)
        {
            x += dx;
            y += dy;
        }

        public void SetSkierPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        private void Go12()
        {
            rw1.EnterReadLock(); //Wejscie na wyciag przed serwisem

            sem2.Wait(); //Zajecie sobie miejsca na 2. bazie

            lock (parkB0) //zablokowanie dostepu do miejsc parkingowych na bazie startowej i zwolnienie miejsca
            {
                slotsB0[slotNumber] = 1;
            }

            lock (o1) //zablokowanie miejsca startowego wyciagu/zjazdu, ustawienie na pozycji i zmiana stanu na wjazd/zjazd
            { 
                this.SetSkierPosition(170, 480);
                Thread.Sleep(1000);
                this.SetSkierPosition(170, 450);
                condition = 12;
            }

            sem1.Release(); //zwolnienie miejsca w opuszczonej bazie
        }

        private void Go21()
        {
            sem1.Wait();

            lock (parkB1)
            {
                slotsB1[slotNumber] = 1;
            }

            lock (o2)
            {
                this.SetSkierPosition(338, 155);
                Thread.Sleep(1000);
                this.SetSkierPosition(338, 185);
                condition = 21;
            }

            sem2.Release();
        }

        private void Go23()
        {
            sem3.Wait();

            lock (parkB0)
            {
                slotsB1[slotNumber] = 1;
            }

            lock (o3)
            {
                this.SetSkierPosition(386, 154);
                Thread.Sleep(1000);
                this.SetSkierPosition(386, 184);
                condition = 23;
            }

            sem2.Release();
        }

        private void Go32()
        {
            rw2.EnterReadLock();

            sem2.Wait();

            lock (parkB2)
            {
                slotsB2[slotNumber] = 1;
            }

            sem3.Release();

            lock (o4)
            {
                this.SetSkierPosition(434, 335);
                Thread.Sleep(1000);
                this.SetSkierPosition(434, 305);
                condition = 32;
            }
        }

        private void Go31()
        {
            sem1.Wait();

            lock (parkB2)
            {
                slotsB2[slotNumber] = 1;
            }

            lock (o6)
            {
                this.SetSkierPosition(674, 310);
                Thread.Sleep(1000);
                this.SetSkierPosition(674, 340);
                condition = 31;
            }

            sem3.Release();
        }

        private void Go13()
        {
            rw3.EnterReadLock();

            sem3.Wait();

            lock (parkB0)
            {
                slotsB0[slotNumber] = 1;
            }

            lock (o5)
            {
                this.SetSkierPosition(496, 480);
                Thread.Sleep(1000);
                this.SetSkierPosition(496, 464);
                condition = 13;
            }

            sem1.Release();
        }

        private void ParkOnB1()
        {
            lock (parkB1)
            {
                int x, y;
                int i = 0;
                while (slotsB1[i] == 0)
                {
                    i++;
                }
                this.slotNumber = i;
                x = B1x[slotNumber];
                y = B1y[slotNumber];
                this.SetSkierPosition(x, y);
                slotsB1[slotNumber] = 0;
                condition = 2;
            }
        }

        private void ParkOnB0()
        {
            lock (parkB0)
            {
                int x, y;
                int i = 0;
                while (slotsB0[i] == 0)
                {
                    i++;
                }
                this.slotNumber = i;
                x = B0x[slotNumber];
                y = B0y[slotNumber];
                this.SetSkierPosition(x, y);
                slotsB0[slotNumber] = 0;
                condition = 1;
            }
        }

        private void ParkOnB2()
        {
            lock (parkB2)
            {
                int x, y;
                int i = 0;
                while (slotsB2[i] == 0)
                {
                    i++;
                }
                this.slotNumber = i;
                x = B2x[slotNumber];
                y = B2y[slotNumber];
                this.SetSkierPosition(x, y);
                slotsB2[slotNumber] = 0;
                condition = 3;
            }
        }
    }
}
