// See https://aka.ms/new-console-template for more information

using System.Device.I2c;
using TwisterTestProjekt;

Console.WriteLine("Hello, World!");

var twist = new Twister();

int sleepTime = 40;
int i = 0;


while (true)
{
    try
    {
        bool moving = twist.isMoved();
        Thread.Sleep(sleepTime);
        if (moving)
        {
            i++;
            Console.WriteLine(Convert.ToString(i));
        }

        bool clicking = twist.isClicked();
        Thread.Sleep(sleepTime);
        if (clicking)
        {
            Console.WriteLine("Valgt:" + Convert.ToString(i));
        }
    }

    catch (IOException e)
    {
        Console.WriteLine(e);
        Console.WriteLine("Error catched");
    }

}

