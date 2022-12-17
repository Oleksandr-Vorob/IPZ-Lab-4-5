
// Сервісний центр електроніки

#region Main
CRapairService repairService = new CRapairService();
do
{
    repairService.Start();
    Console.WriteLine("New custumer? (+ yes; - no)");
} while (Console.ReadLine() == "+");
#endregion

class CRapairService
{
    static Days _day = CShedule.What_a_day();
    CManager manager = new CManager(_day, "Bill", "Ann", "Olexandr", "Andriy", "Volodymir", "Daryna", "Dmytro");
    CComputer computer = new CComputer();
    CEngineer engineer = new CEngineer();
    CCash cash = CCash.Initialize();

    CBroom broom = new CBroom(500);
    CTrash trash = new CTrash();

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
        trash.Clean(broom.Strength);
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
    Random rnd = new Random();
    public double Check(CBroken_device device)
    {
        double price = 0;
        if (device.Display == false)
        {
            price += Display();
            Console.WriteLine("Display is broken. Price " + Display() + "grn.");
        }
        else Console.WriteLine("Display works.");
        if (device.Mainboard == false)
        {
            price += Mainboard();
            Console.WriteLine("Mainboard is broken. Price " + Mainboard() + "grn.");
        }
        else Console.WriteLine("Mainboard works.");

        if (device.Processor == false)
        {
            price += Processor();
            Console.WriteLine("Processor is broken. Price " + Processor() + "grn.");
        }
        else Console.WriteLine("Processor works.");

        if (device.Videocard == false)
        {
            price += Videocard();
            Console.WriteLine("Videocard is broken. Price " + Videocard() + "grn.");
        }
        else Console.WriteLine("Videocard works.");

        if (device.PowerSupply == false)
        {
            price += Power_supply();
            Console.WriteLine("Power supply is broken. Price " + Power_supply() + "grn.");
        }
        else Console.WriteLine("Power supply works.");

        if (device.Software == false)
        {
            price += Software();
            Console.WriteLine("Software is broken. Price " + Software() + "grn.");
        }
        else Console.WriteLine("Software works.");

        return price;
    }
    public double Display()
    {
        int x = rnd.Next(1, 4);
        if (x == 1) return 1000;
        if (x == 2) return 1500;
        else return 2200;
    }
    public double Mainboard()
    {
        int x = rnd.Next(1, 6);
        if (x == 1) return 800;
        if (x == 2) return 1100;
        if (x == 3) return 1650;
        if (x == 4) return 1900;
        else return 2400;
    }
    public double Processor()
    {
        int x = rnd.Next(1, 3);
        if (x == 1) return 500;
        else return 1500;
    }
    public double Videocard()
    {
        int x = rnd.Next(1, 6);
        if (x == 1) return 900;
        if (x == 2) return 1700;
        if (x == 3) return 3200;
        if (x == 4) return 4500;
        else return 5000;
    }
    public double Power_supply()
    {
        int x = rnd.Next(1, 3);
        if (x == 1) return 700;
        else return 1800;
    }
    public double Software()
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
    private static CCash _single = null;
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

class CBroom
{
    public double Strength { get; set; }
    public CBroom(double strength)
    {
        this.Strength = strength;
    }

}

class CTrash
{
    int _trash = 0;
    public void Generation()
    {
        Random rnd = new Random();
        int x = rnd.Next(5, 20);
        _trash += x;
    }
    public void Clean(double strenght)
    {
        if (_trash > 15)
        {
            strenght -= _trash;
            _trash = 0;
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