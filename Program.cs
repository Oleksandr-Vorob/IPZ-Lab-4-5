
// Сервісний центр електроніки

#region Main
do
{
    CFacade facade = new CFacade();
    facade.Start();
    Console.WriteLine("New custumer? (+ yes; - no)");
} while (Console.ReadLine() == "+");
#endregion

class CFacade
{
    CShedule shedule = new CShedule();
    static Days Day = CShedule.What_a_day();
    CManager manager = new CManager(Day, "Bill", "Ann", "Olexandr", "Andriy", "Volodymir", "Daryna", "Dmytro");
    CComputer computer = new CComputer();
    CEngineer engineer = new CEngineer();
    CCash cash = CCash.Initialize();

    CBroom broom = new CBroom(500);
    CTrash trash = new CTrash();

    public void Start()
    {
        manager.AskDevice();
        CCustumer custumer = new CCustumer("Custumer", manager, engineer);
        CBroken_device device = custumer.device;
        double Price = computer.Check(device, engineer);
        manager.TellPrice(Price);
        custumer.Pay(Price);
        CPayHistory history = new CPayHistory();
        history.History.Push(cash.SaveState());
        cash.Cash(Price);
        Console.WriteLine("Do you want your money to go to charity? (+ yes, - no)");
        if (Console.ReadLine() == "+") cash.RestoreState(history.History.Pop());
        trash.Trash(ref broom._strength);
    }
}

class CShedule
{
    #region Fields
    static string? day;
    #endregion

    public static Days What_a_day()
    {
        Console.WriteLine("What day of the week is today?");
        for (int i = 1; i < 8; i++)
        {
            Console.WriteLine(i + "-" + (Days)i);
        }
    Ask: day = Console.ReadLine();
        if (day != "Monday" && day != "Tuesday" && day != "Wednesday" && day != "Thursday" && day != "Friday" && day != "Saturday" && day != "Sunday")
        {
            Console.WriteLine("It's not a day of the week! Try again.");
            goto Ask;
        }
        Days Day = (Days)Enum.Parse(typeof(Days), day, true);
        return Day;
    }
}
class CManager
{
    string? _name;
    public CManager(Days day, params string[] names)
    {
        _name = names[(int)day - 1];
        if (_name == null) _name = "Manager";
    }

    public CBroken_device Transmission(CBroken_device device, CEngineer engineer)
    {
        device = engineer.Diagnosis(device);
        return device;

    }

    public void AskName()
    {
        Console.WriteLine("(Custumer)");
        Console.WriteLine("What is your name?");
    }
    public void AskDevice()
    {
        Console.WriteLine("Hello! My name is " + _name + ". Give me your device!");
    }
    public void TellPrice(double price)
    {
        Console.WriteLine("Repair costs " + price + " grn.");
    }
}

class CComputer
{
    public double Check(CBroken_device device, CEngineer engineer)
    {
        double price = 0;
        if (device.Display == false) price += engineer.Display();
        if (device.Mainboard == false) price += engineer.Mainboard();
        if (device.Processor == false) price += engineer.Processor();
        if (device.Videocard == false) price += engineer.Videocard();
        if (device.PowerSupply == false) price += engineer.Power_supply();
        if (device.Software == false) price += engineer.Software();
        return price;
    }
}

class CCustumer
{
    public string Name;
    double cash = 30000;
    public CBroken_device device;
    public CCustumer(string name, CManager manager, CEngineer engineer)
    {
        this.Name = name;
        device = manager.Transmission(device, engineer);

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
    public CBroken_device Diagnosis(CBroken_device device)
    {
        Console.WriteLine("(Engineer)");
        device.Display = Serviceability("display");
        device.Mainboard = Serviceability("mainboard");
        device.Processor = Serviceability("processor");
        device.Videocard = Serviceability("videocard");
        device.PowerSupply = Serviceability("power supply");
        device.Software = Serviceability("software");
        return device;
    }
    public bool Serviceability(string component)
    {
        Console.WriteLine("Is the " + component + " working? (+ serviceable; - defective)");
        string? x = Console.ReadLine();
        if (x == "-") return false;
        else return true;
    }
    Random rnd = new Random();
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
    public double _strength;
    public CBroom(double strength)
    {
        this._strength = strength;
    }

}

class CTrash
{
    int _trash = 0;
    public void Trash(ref double strenght)
    {
        Random rnd = new Random();
        int x = rnd.Next(5, 20);
        _trash += x;
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