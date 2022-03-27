public class Student
{
    public string Name {get; set;}
    public int Age {get; set;}

    public Student(string name, int age)
    {
        this.Name = name;
        this.Age = age;
    }

    public override string ToString()
    {
        return $"{this.Name},{this.Age}";
    }
}