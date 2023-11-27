// See https://aka.ms/new-console-template for more information

using System.Device.I2c;
using TwisterTestProjekt;

Console.WriteLine("Hello, World!");

var twist = new Twister();

int sleepTime = 40;
int i = 0;


while (true)
{
    //i2cQueue.Enqueue(() =>
    //{
    try
    {
        bool moving = twist.isMoved();
        Thread.Sleep(sleepTime);
        if (moving)
        {
            i++;
            //lcd.Clear();
            //Thread.Sleep(sleepTime);
            //lcd.Write(Convert.ToString(i));
            Console.WriteLine(Convert.ToString(i));
            //Thread.Sleep(sleepTime);
        }

        bool clicking = twist.isClicked();
        Thread.Sleep(sleepTime);
        if (clicking)
        {
            //lcd.Clear();
            //Thread.Sleep(sleepTime);
            Console.WriteLine("Valgt:" + Convert.ToString(i));
            //lcd.Write("Valgt:" + Convert.ToString(i));
            //Thread.Sleep(sleepTime);
        }
    }

    catch (IOException e)
    {
        Console.WriteLine(e);
        Console.WriteLine("Error catched");
    }

}

