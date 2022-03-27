using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections;

namespace SerializeStudents
{
    public class Program
    {
        public const string FILEPATH = "students.json";
        public const int TUITIONFEE = 10000;
        public const int AGE = 25;
        public static void Main()
        {
            string input = showOptions();
            while (input != "q")
            {
                switch (input)
                {
                    case "a":   //Add student
                        {
                            AddStudent();
                            break;
                        }
                    case "d":   //Delete student
                        {
                            Console.Write("Enter the student's name you want to delete: ");
                            string studentName = Console.ReadLine();
                            while (!validInput(studentName))
                                studentName = Console.ReadLine();
                            DeleteStudent(studentName);
                            break;
                        }
                    case "l":   //Print List values
                        {
                            List<Student> studentsList = Deserialize();
                            foreach (Student student in studentsList)
                                Console.WriteLine(student);
                            break;
                        }
                    case "tf":   //Print students list with tutation fee report
                        {
                            List<Student> studentsList = Deserialize();
                            foreach (Student student in studentsList)
                            {
                                if (student.Age > AGE)
                                    Console.WriteLine($"{student} need to pay {TUITIONFEE}");
                                else
                                    Console.WriteLine($"{student} need to pay {TUITIONFEE * .9}");
                            }
                            break;
                        }
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
                input = showOptions();
            }
            Console.WriteLine("Thank you, Good bye!");
        }
        public static string showOptions()
        {
            string USER_INPUT =
            @"
        Choose an option from the following list:
            a - Add student
            d - Delete student (By Name)
            l - Print students list (name, age)
            tf - Print tutation fee report 
            ctrl + c - exit program
        Your option?";
            Console.WriteLine(USER_INPUT);
            return Console.ReadLine().Trim().ToLower(); ;
        }
        public static bool validInput(string val) //No Null input
        {
            if (val == "" || val == " ")
            {
                Console.WriteLine("Invalid input, no empty value");
                return false;
            }
            return true;
        }
        public static bool validAge(int age) //Age between 18 and 120
        {
            if (age < 18 || age > 120)
            {
                Console.WriteLine("Invalid age");
                return false;
            }
            return true;
        }

        public static void AddStudent()
        {
            List<Student> studentsList = Deserialize();
            string name;
            int age = 0;
            Console.Write("Enter student's name: ");
            name = Console.ReadLine();
            while (!validInput(name))
                name = Console.ReadLine();
            Console.Write("Enter student's age: ");
            try
            {
                age = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Age must include only numbers!");
            }
            while (!validAge(age))
            {
                Console.WriteLine("Enter a valid age! (18-120)");
                age = int.Parse(Console.ReadLine());
            }
            Student newStudent = new Student(name, age);
            foreach (Student student in studentsList)
            {
                if (student.Name.ToLower() == newStudent.Name.ToLower())
                {
                    Console.WriteLine($"Can't add {newStudent}, Student name already exists!");
                    return;
                }
            }
            studentsList.Add(newStudent);
            Console.WriteLine($"{newStudent} added successfully");
            Serialize(studentsList);
        }
        public static void DeleteStudent(string studentName)
        {
            List<Student> studentsList = Deserialize();
            bool removed = false;
            foreach (Student student in studentsList)
            {
                if (student.Name.ToLower() == studentName.ToLower())
                {
                    removed = studentsList.Remove(student);
                    Serialize(studentsList);
                    break;
                }
            }
            if (removed)
                Console.WriteLine($"{studentName} deleted successfully");
            else
            {
                Console.WriteLine($"{studentName} doesn't exist");
            }
        }
        public static List<Student> Deserialize()
        {
            string studentJsonString = File.ReadAllText(FILEPATH);
            List<Student> studentsList = JsonSerializer.Deserialize<List<Student>>(studentJsonString);
            return studentsList;
        }
        public static void Serialize(List<Student> studentsList)
        {
            string studentJsonString = JsonSerializer.Serialize(studentsList);
            File.WriteAllText(FILEPATH, studentJsonString);
        }
    }
}