
// Сервісний центр електроніки

#region Main
CRepairService repairService = new CRepairService();

do
{
    repairService.Start();
    Console.WriteLine("New custumer? (+ yes; - no)");
} while (Console.ReadLine() == "+");
#endregion

class CRepairService
{
    static Days _day = CShedule.What_a_day();
    CManager manager = new CManager(_day, "Bill", "Ann", "Olexandr", "Andriy", "Volodymir", "Daryna", "Dmytro");
    CComputer computer = new CComputer();
    CEngineer engineer = new CEngineer();
    CCash cash = CCash.Initialize();
    IObserver robotCleaner;
    CTrash trash;
    public CRepairService()
    {
        trash = new CTrash();
        robotCleaner = new CRobotCleaner(500);
        trash.AttachObserver(robotCleaner);
    }

    public void Start()
    {
        CCustumer custumer = new CCustumer("Custumer1", manager);
        CBroken_device device = custumer.Device;
        manager.AskDevice();
        manager.GetDevice(device);
        manager.Transmission(device, engineer);
        double price = engineer.Check(computer);
        manager.TellPrice(price);
        custumer.Pay(price);
        CPayHistory history = new CPayHistory();
        history.History.Push(cash.SaveState());
        cash.Cash(price);
        Console.WriteLine("Do you want your money to go to charity? (+ yes, - no)");
        if (Console.ReadLine() == "+") cash.RestoreState(history.History.Pop());
        trash.Generation();
    }
    
}

class CShedule
{
    #region Fields
    static string? _day;
    #endregion

    public static Days What_a_day()
    {
        Console.WriteLine("What day of the week is today?");
        for (int i = 1; i < 8; i++)
        {
            Console.WriteLine(i + "-" + (Days)i);
        }
        do
        {
            _day = Console.ReadLine();
            if (_day != "Monday" && _day != "Tuesday" && _day != "Wednesday" && _day != "Thursday" && _day != "Friday" && _day != "Saturday" && _day != "Sunday")
            {
                Console.WriteLine("It's not a day of the week! Try again.");
            }
            else break;
        } while (true);
        Days Day = (Days)Enum.Parse(typeof(Days), _day, true);
        return Day;
    }
}
class CManager
{
    string? _name;
    CBroken_device _device;
    public CManager(Days day, params string[] names)
    {
        _name = names[(int)day - 1];
        if (_name == null) _name = "Manager";
    }

    //public void AskName()
    //{
    //    Console.WriteLine("(Custumer)");
    //    Console.WriteLine("What is your name?");
    //}
    public void AskDevice()
    {
        Console.WriteLine("Hello! My name is " + _name + ". Give me your device!");

    }
    public void GetDevice(CBroken_device device)
    {
        _device = device;
    }
    public void TellPrice(double price)
    {
        Console.WriteLine("Repair costs " + price + " grn.");
    }
    public void Transmission(CBroken_device device, CEngineer engineer)
    {
        engineer.GetDevice(device);
    }
}

class CComputer
{
    static Random rnd = new Random();
    string[] accessories = { "Display", "Mainboard", "Processor", "Videocard", "Power supply", "Software" };
    public double Check(CBroken_device device)
    {
        bool[] deviceAccessories = { device.Display, device.Mainboard, device.Processor, device.Videocard, device.PowerSupply, device.Software };
        double price = 0;
        for (int i = 0; i < deviceAccessories.Length; i++)
        {
            double[] methods = { Display(), Mainboard(), Processor(), Videocard(), Power_supply(), Software() };
            if (deviceAccessories[i] == false)
            {
                price += methods[i];
                Console.WriteLine(accessories[i] + " is broken. Price " + methods[i] + "grn.");
            }
            else Console.WriteLine(accessories[i] + " works.");
        }
        return price;
    }
    public static double Display()
    {
        int x = rnd.Next(1, 4);
        switch (x)
        {
            case 1: return 1000;
            case 2: return 1500;
            default: return 2200;
        }
    }
    public static double Mainboard()
    {
        int x = rnd.Next(1, 6);
        switch (x)
        {
            case 1: return 800;
            case 2: return 1100;
            case 3: return 1650;
            case 4: return 1900;
            default: return 2400;
        }
    }
    public static double Processor()
    {
        int x = rnd.Next(1, 3);
        if (x == 1) return 500;
        else return 1500;
    }
    public static double Videocard()
    {
        int x = rnd.Next(1, 6);
        switch (x)
        {
            case 1: return 900;
            case 2: return 1700;
            case 3: return 3200;
            case 4: return 4500;
            default: return 5000;
        }
    }
    public static double Power_supply()
    {
        int x = rnd.Next(1, 3);
        if (x == 1) return 700;
        else return 1800;
    }
    public static double Software()
    {
        int x = rnd.Next(1, 4);
        if (x == 1) return 500;
        if (x == 2) return 999;
        else return 1350;
    }
}

class CCustumer
{
    public string Name;
    double cash = 30000;
    public CBroken_device Device = new CBroken_device();
    public CCustumer(string name, CManager manager)
    {
        this.Name = name;
        //Device = manager.Transmission(Device, engineer);
        Device.Display = true;
        Device.Processor = true;
    }
    public void Pay(double price)
    {
        cash -= price;
    }
}

struct CBroken_device
{
    public bool Display;
    public bool Mainboard;
    public bool Processor;
    public bool Videocard;
    public bool PowerSupply;
    public bool Software;

}

class CEngineer
{
    CBroken_device _device;
    public double Check(CComputer computer)
    {
        return computer.Check(_device);

    }
    public void GetDevice(CBroken_device device)
    {
        _device = device;
    }
    //public CBroken_device Diagnosis()
    //{
    //    Console.WriteLine("(Engineer)");
    //    _device.Display = Serviceability("display");
    //    _device.Mainboard = Serviceability("mainboard");
    //    _device.Processor = Serviceability("processor");
    //    _device.Videocard = Serviceability("videocard");
    //    _device.PowerSupply = Serviceability("power supply");
    //    _device.Software = Serviceability("software");
    //    return _device;
    //}
    //public bool Serviceability(string component)
    //{
    //    Console.WriteLine("Is the " + component + " working? (+ serviceable; - defective)");
    //    string? x = Console.ReadLine();
    //    if (x == "-") return false;
    //    else return true;
    //}


}

sealed class CCash
{
    double cash = 5320;
    private static CCash? _single = null;
    private CCash() { }
    public static CCash Initialize()
    {
        if (_single == null) _single = new CCash();
        return _single;
    }
    public void Cash(double money)
    {
        cash += money;
    }
    public CCashMemento SaveState()
    {
        return new CCashMemento(cash);
    }
    public void RestoreState(CCashMemento memento)
    {
        this.cash = memento.Cash_service;
    }
}

enum Days
{
    Monday = 1,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}

interface IObserver
{
    void Update(CTrash trash);
}

class CRobotCleaner : IObserver
{
    public double Strength { get; set; }
    public CRobotCleaner(double strength)
    {
        this.Strength = strength;
    }
    public void Update(CTrash trash)
    {
        if (trash.Trash > 15)
        {
            Strength -= trash.Trash;
            trash.Trash = 0;
        }
    }
}

class CTrash
{
    IObserver[] _observer = new IObserver[5];
    int _observerCounter;
    public int Trash = 0;

    public void Generation()
    {
        Random rnd = new Random();
        int x = rnd.Next(5, 20);
        Trash += x;
        Notify();
    }
    public void AttachObserver(IObserver observer)
    {
        _observer[_observerCounter] = observer;
        ++_observerCounter;
    }
    public void Notify()
    {
        for (int i = 0; i < _observerCounter; ++i)
        {
            _observer[i].Update(this);
        }
    }
}

class CCashMemento
{
    public double Cash_service { get; private set; }
    public CCashMemento(double cash_service)
    {
        Cash_service = cash_service;
    }
}

class CPayHistory
{
    public Stack<CCashMemento> History { get; private set; }
    public CPayHistory()
    {
        History = new Stack<CCashMemento>();
    }
}