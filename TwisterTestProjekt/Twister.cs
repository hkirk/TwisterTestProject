using System.Device.I2c;
using System.Net.Http.Headers;

namespace TwisterTestProjekt;

public class Twister
{
    private class I2CWRapper
    {
        private I2cDevice _i2C;
        private int sleepTime = 4;

        public I2CWRapper()
        {
            I2cConnectionSettings i2cS = new I2cConnectionSettings(1, 0x3F);
            _i2C = I2cDevice.Create(i2cS);
            
        }
        
        public byte readRegister(byte addr)
        {
            byte[] writeBuffer = { addr }, readBuffer = new byte[1];

            retryOperation(() =>
            {
                _i2C.WriteRead(writeBuffer, readBuffer);
            });

            return readBuffer[0];
        }
        
        private void retryOperation(Action action)
        {
            try
            {
                action();
                Thread.Sleep(sleepTime);
            }
            catch (Exception)
            {
                Thread.Sleep(sleepTime*sleepTime);
                action();
                Thread.Sleep(sleepTime);
            }
        }
    
        public bool writeRegister(byte addr, byte value)
        {
            byte[] writeBuffer = { (byte)addr, (byte)value }, readBuffer = new byte[1];
            
            retryOperation(() =>
            {
                _i2C.WriteRead(writeBuffer, readBuffer);
            });

            return value == readBuffer[0];
        }
    }

    private enum encoderRegisters
    {
        TWIST_ID = 0x00,
        TWIST_STATUS = 0x01,
        TWIST_VERSION = 0x02,
        TWIST_ENABLE_INTS = 0x04,
        TWIST_COUNT = 0x05,
        TWIST_DIFFERENCE = 0x07,
        TWIST_LAST_ENCODER_EVENT = 0X09,
        TWIST_LAST_BUTTON_EVENT = 0x0B,

        TWIST_RED = 0x0D,
        TWIST_GREEN = 0x0E,
        TWIST_BLUE = 0x0F,

        TWIST_CONNECT_RED = 0x10,
        TWIST_CONNECT_GREEN = 0x12,
        TWIST_CONNECT_BLUE = 0x14,

        TWIST_TURN_INT_TIMEOUT = 0x16,
        TWIST_CHANGE_ADDRESS = 0x18,
        TWIST_LIMIT = 0x19
    }
    
    private const byte statusButtonClickedBit = 2;
    private const byte statusButtonPressedBit = 1;
    private const byte statusEncoderMoveBit = 0;

    private I2CWRapper _i2cWrapper = new();

    public bool isMoved()
    {
        Console.WriteLine("<isMoved>");
        byte status = _i2cWrapper.readRegister(Convert.ToByte(encoderRegisters.TWIST_STATUS));
        Console.WriteLine(status.ToString());
        byte statusMoved = (1 << statusEncoderMoveBit);
        bool moved = (status & statusMoved) != 0;
        byte reset = (byte)(status & (~statusMoved));
        Console.WriteLine(reset.ToString());

        _i2cWrapper.writeRegister(Convert.ToByte(encoderRegisters.TWIST_STATUS), reset);
        Console.WriteLine("</isMoved>");

        return moved;
    }
    
    public bool isClicked()
    {
        Console.WriteLine("<isClicked>");

        byte status = _i2cWrapper.readRegister(Convert.ToByte(encoderRegisters.TWIST_STATUS));
        Console.WriteLine(status.ToString());
        byte statusClicked = (1 << statusButtonClickedBit);
        bool pressed = (status & statusClicked) != 0;
        byte reset = (byte)(status & (~statusClicked));
        Console.WriteLine(reset.ToString());

        _i2cWrapper.writeRegister(Convert.ToByte(encoderRegisters.TWIST_STATUS), reset);
        Console.WriteLine("</isClicked>");

        return pressed;
    }
    


}