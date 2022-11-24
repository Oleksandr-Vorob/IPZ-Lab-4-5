
// Сервісний центр електроніки
#region Main
CComputer computer = new CComputer();
CCash cash = CCash.Initialize();
CEngineer engineer = new CEngineer();
CShedule shedule = new CShedule();
CManager manager = new CManager();

CBroom broom = new CBroom(500);
CTrash trash = new CTrash();


int Day = shedule.What_a_day();
manager.Name(Day, "Bill", "Ann", "Olexandr", "Andriy", "Volodymir", "Daryna", "Dmytro");
do
{
    Start();
    Console.WriteLine("New custumer? (+ yes; - no)");
} while (Console.ReadLine() == "+");
#endregion
#region Methods
void Start()
{
    manager.Speak0();
    CCustumer custumer = new CCustumer(Console.ReadLine());
    manager.Speak(custumer.name);
    CBroken_device device = CManager.diagnosis(engineer);
    double Price = computer.Check(device.display, device.mainboard, device.processor, device.videocard, device.power_supply, device.software);
    manager.Speak(Price);
    custumer.Pay(Price);
    cash.Cash(Price);
   trash.Trash(ref broom._strength);
}
#endregion

class CShedule
{
    #region Fields
    int _day;
    #endregion

    public int What_a_day()
    {
        Console.WriteLine("What day of the week is today?");
        Console.WriteLine(
            "1 - Monday\n" +
            "2 - Tuesday\n" +
            "3 - Wednesday\n" +
            "4 - Thursday\n" +
            "5 - Friday\n" +
            "6 - Saturday\n" +
            "7 - Sunday");
        _day = Convert.ToInt32(Console.ReadLine());
        return _day;
    }
}
class CManager
{
    string? _name;
    public void Name(int day, params string[] names)
    {
        switch (day)
        {
            case (int)Days.Monday:
                _name = names[0];
                break;
            case (int)Days.Tuesday:
                _name = names[1];
                break;
            case (int)Days.Wednesday:
                _name = names[2];
                break;
            case (int)Days.Thursday:
                _name = names[3];
                break;
            case (int)Days.Friday:
                _name = names[4];
                break;
            case (int)Days.Saturday:
                _name = names[5];
                break;
            case (int)Days.Sunday:
                _name = names[6];
                break;
            default:
                _name = "Pracivnyk";
                break;
        }
    }
    public static CBroken_device diagnosis(CEngineer engineer)
    {
        CBroken_device device = new CBroken_device();
        Console.WriteLine("(Engineer)");
        device.display = engineer.Serviceability("display");
        device.mainboard = engineer.Serviceability("mainboard");
        device.processor = engineer.Serviceability("processor");
        device.videocard = engineer.Serviceability("videocard");
        device.power_supply = engineer.Serviceability("power supply");
        device.software = engineer.Serviceability("software");
        return device;
    }
    public void Speak0()
    {
        Console.WriteLine("(Custumer)");
        Console.WriteLine("What is your name?");
    }
    public void Speak(string name1)
    {
        Console.WriteLine("Hello, " + name1 + "! My name is " + _name + ". Give me your device!");
    }
    public void Speak(double price)
    {
        Console.WriteLine("Repair costs " + price + " grn.");
    }
}

class CComputer
{
    double _displayP = 1000;
    double _mainboardP = 2000;
    double _processorP = 1500;
    double _videocardP = 5000;
    double _power_supplyP = 1200;
    double _softwareP = 1100;
    public double Check(bool display, bool mainboard, bool processor, bool videocard, bool power_supply, bool software)
    {
        double price = 0;
        if (display == false) price += _displayP;
        if (mainboard == false) price += _mainboardP;
        if (processor == false) price += _processorP;
        if (videocard == false) price += _videocardP;
        if (power_supply == false) price += _power_supplyP;
        if (software == false) price += _softwareP;
        return price;
    }
}

class CCustumer
{
    public string name;
    double cash = 30000;
    public CCustumer(string name)
    {
        this.name = name;
    }
    public void Pay(double price)
    {
        cash -= price;
    }
}

class CBroken_device
{
    public bool display;
    public bool mainboard;
    public bool processor;
    public bool videocard;
    public bool power_supply;
    public bool software;

}

class CEngineer
{
    public bool Serviceability(string component)
    {
        Console.WriteLine("Is the " + component + " working? (+ serviceable; - defective)");
        string? x = Console.ReadLine();
        if (x == "-") return false;
        else return true;
    }
}

sealed class CCash
{
    double cash = 5320;
    private static CCash single = null;
    private CCash() { }
    public static CCash Initialize()
    {
        if (single == null) single = new CCash();
        return single;
    }
    public void Cash(double money)
    {
        cash += money;
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
