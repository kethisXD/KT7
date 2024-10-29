//
//TASK 2
//
abstract class Animal
{
    public string Name { get; set; }

    public Animal(string name)
    {
        Name = name;
    }

    public abstract void SayHello();
}


class Dog : Animal
{
    public Dog(string name) : base(name) { }

    public override void SayHello()
    {
        Console.WriteLine($"Собака по имени {Name} говорит: Гав!");
    }
}


class Cat : Animal
{
    public Cat(string name) : base(name) { }

    public override void SayHello()
    {
        Console.WriteLine($"Кошка по имени {Name} говорит: Мяу!");
    }
}


//
//TASK 1
//
public interface IConverter<in T, out U>
{
    public U Convert(T value);
}

//public delegate IConverter<T, U> ConvertOperation<T, U>();
class StringToIntConverter : IConverter<string, int>
{
    public int Convert(string value)
    {
        return int.Parse(value);
    }
}

class ObjectToStringConverter : IConverter<object, string>
{
    public string Convert(object obj)
    {
        return obj.ToString();
    }
}


class Program
{
    static void ProcessAnimals(List<Animal> listAnimal, Action<Animal> action)
    {
        foreach (var item in listAnimal)
        {
            action(item);
        }
    }
    static U[] ConvertArray<T, U>(T[] array, IConverter<T, U> converter)
    {
        U[] resultArray = new U[array.Length];
        for (int i = 0; i < array.Length; i++)
        {
            resultArray[i] = converter.Convert(array[i]);
        }
        return resultArray;
    }
    static void Main()
    {
        //TASK 1
        string[] StringValues = {"1","2","3","4","5","6","7"};
        IConverter<string, int> stringToIntConverter = new StringToIntConverter();
        int[] intResults = ConvertArray(StringValues, stringToIntConverter); 
        System.Console.WriteLine("Str to int: " + string.Join(", ", intResults));

        object[] objects = { 1, "two", 3.0 };
        IConverter<object, string> objToStrConverter = new ObjectToStringConverter();
        string[] stringResults = ConvertArray(objects, objToStrConverter);
        Console.WriteLine("Objects to Strings: " + string.Join(", ", stringResults));


        // Ковариатность: U is out, поэтому мы можем использовать IConverter<object, object> вместо IConverter<object, String>
        IConverter<object, object> objToObjConverter = objToStrConverter;
        object[] objResults = ConvertArray(objects, objToObjConverter);
        Console.WriteLine("Objects to Objects: " + string.Join(", ", objResults));

        IConverter<dynamic, string> dynamicToStrConverter = objToStrConverter;
        dynamic[] dynamicValues = { 1, "two", 3.0 };
        string[] dynamicResults = ConvertArray(dynamicValues, dynamicToStrConverter);
        Console.WriteLine("Dynamics to Strings: " + string.Join(", ", dynamicResults));
        //TASK 1


        //TASK 2
        List<Animal> animals = new List<Animal>
        {
            new Dog("Бобик"),
            new Cat("Мурка")
        };

        // Действие для Animal
        Action<Animal> actionAnimal = a => a.SayHello();

        // Действие для Dog
        Action<Dog> actionDog = d => Console.WriteLine($"Пес по имени {d.Name} виляя хвостом!");

        // Действие для Object
        Action<object> actionObject = o => Console.WriteLine($"Объект: {o.ToString()}");

        Console.WriteLine("Вызов с Action<Animal>:");
        ProcessAnimals(animals, actionAnimal);
                List<Dog> dogs = new List<Dog>
        {
            new Dog("Шарик"),
            new Dog("Тузик")
        };

        void ProcessDogs(List<Dog> dogList, Action<Dog> dogAction)
        {
            foreach (var dog in dogList)
            {
                dogAction(dog);
            }
        }

        Console.WriteLine("\nВызов ProcessDogs с Action<Animal> (контрвариантность):");
        ProcessDogs(dogs, actionAnimal);
    }
}