using System;
using static System.Console;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;
using System.IO;

namespace myPlanner {
  public class Item<T> {
    public T Data { get; set; }
    public Item<T> Prev { get; set; }
    public Item<T> Next { get; set; }
    public Item(T data) {
      Data = data;
    }
    public override string ToString() {
      return Data.ToString();
    }
  }
  public class DualList<T> {
    public Item<T> Head { get; set; }
    public Item<T> Tail { get; set; }
    public int Count { get; set; }

    public DualList() { }

    public DualList(T data) {
      var item = new Item<T>(data);
      Head = item;
      Tail = item;
      Count = 1;
    }
    public void add(T data) {
      var item = new Item<T>(data);
      Tail.Next = item;
      item.Prev = Tail;
      Tail = item;
    }
    public void del(T data) {
      var current = Head;
      while (current != null) {
        if (current.Data.Equals(data)) {
          current.Prev.Next = current.Next;
          current.Next.Prev = current.Prev;
          Count--;
          return;
        }
        current = current.Next;
      }
    }
  }
  public class Student {
    private long _IIN;
    private string _LastName;
    private string _FirstName;
    private DateTime _Birthday;
    private double _rating;


    public Student() {
      _LastName = _FirstName = "Not given";
      Random rnd = new Random();
      _IIN = rnd.Next(0, 10000000);
      _Birthday = new DateTime(rnd.Next(1920, 2022), rnd.Next(1, 12), rnd.Next(1, 31), rnd.Next(0, 24), rnd.Next(0, 24), rnd.Next(0, 60));
    }
    public Student(long IIN, string LastName, string FirstName, DateTime Birthday) {
      _IIN = IIN;
      _FirstName = FirstName;
      _LastName = LastName;
      _Birthday = Birthday;
    }
    public string FirstName {
      get {
        using (var logFile = new StreamWriter("log.txt", append: true)) {
          logFile.WriteLine($"Прочитано значение FirstName ({_FirstName}) у студента {_IIN}");
        }
        return _FirstName;
      }
      set {
        if (value.Length < 30 || value.Length > 30) {
          _FirstName = value;
        } else {
          throw new Exception("Неверное значения FirstName!");
        }
      }
    }
    public string LastName {
      get {
        using (var logFile = new StreamWriter("log.txt", append: true)) {
          logFile.WriteLine($"Прочитано значение LastName ({_LastName}) у студента {_IIN}");
        }
        return _LastName;
      }
      set {
        if (value.Length < 30 || value.Length > 30) {
          _LastName = value;
        } else {
          throw new Exception("Неверное значения LastName!");
        }
      }
    }
    public DateTime Birthday {
      get {
        using (var logFile = new StreamWriter("log.txt", append: true)) {
          logFile.WriteLine($"Прочитано значение Birthday ({_Birthday}) у студента {_IIN}");
        }
        return _Birthday;
      }
      set {
        if (value.Year > 1920 && value.Year < 2021 && value.Month > 0 && value.Month < 12 && value.Day > 0 && value.Day < 31) {
          Random rnd = new Random();
          _Birthday = new DateTime(value.Year, value.Month, value.Day, rnd.Next(0, 24), rnd.Next(0, 60), rnd.Next(0, 60));
        } else {
          throw new Exception("Неверное значения Birthday!");
        }
      }
    }
    public long IIN {
      get {
        using (var logFile = new StreamWriter("log.txt", append: true)) {
          logFile.WriteLine($"Прочитано значение IIN ({_IIN}) у студента {_IIN}");
        }
        return _IIN;
      }
      set {
        if (value > 0) {
          _IIN = value;
        } else {
          throw new Exception("Неверное значения IIN!");
        }
      }
    }
    public double Rating {
      get {
        using (var logFile = new StreamWriter("log.txt", append: true)) {
          logFile.WriteLine($"Прочитано значение Rating ({_rating}) у студента {_IIN}");
        }
        return _rating;
      }
      set {
        if (value < 0 || value > 12) {
          _rating = value;
        } else {
          throw new Exception("Неверное значения Rating!");
        }
      }
    }
    public override string ToString() {
      return $"{_IIN} {_LastName} {_FirstName} {_Birthday}";
    }
    private static Student GetStudentFromLine(string line) {

      string[] newstr = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      long IIN = Convert.ToInt64(newstr[0].ToString());

      string lastName = newstr[1].ToString();

      string firstName = newstr[2].ToString();

      string[] datestr = newstr[3].ToString().Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
      int day = Convert.ToInt32(datestr[0].ToString());
      int month = Convert.ToInt32(datestr[1].ToString());
      int year = Convert.ToInt32(datestr[2].ToString());

      string[] timestr = newstr[4].ToString().Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
      int hour = Convert.ToInt32(timestr[0].ToString());
      int minute = Convert.ToInt32(timestr[1].ToString());
      int second = Convert.ToInt32(timestr[2].ToString());
      Student temp = new Student(IIN, lastName, firstName, new DateTime(day, month, year, hour, minute, second));
      return temp;
    }
    public static Student[] GetStudentsArr(string fileName, int size = 1_000_000) {
      Student[] temp = new Student[size];
      using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default)) {
        string line;
        int i = 0;
        while ((line = sr.ReadLine()) != null) {
          temp[i] = GetStudentFromLine(line);
          i++;
          if (i == size) {
            break;
          }
        }
        if(size > i) {
          throw new Exception($"There are only { i } students, not {size}");
        }
      }
      return temp;
    }

    public static void PrintStudents(Student[] students) {
      for(int i = 0; i < students.Length; ++i) {
        WriteLine(students[i].ToString());
      }
    }
  }
  public class Group {
    private long _groupId;
    private string _groupName;
    public Group() {
      Random rnd = new Random();
      _groupId = rnd.Next(10000, 99999);
      _groupName = "Not Given";
    }
    public Group(string groupName) {
      Random rnd = new Random();
      _groupId = rnd.Next(1000, 9999);
      _groupName = groupName;
    }
    public Group(string GroupName, long GroupId) {
      _groupId = GroupId;
      _groupName = GroupName;
    }

    public string GroupName {
      get {
        return _groupName;
      }
      set {
        if (value.Length > 0) {
          _groupName = value;
        } else {
          throw new Exception("Неверное значения groupName!");
        }
      }
    }
    public long GroupId {
      get {
        return _groupId;
      }
      set {
        if (value > 0) {
          _groupId = value;
        } else {
          throw new Exception("Неверное значениe groupName!");
        }
      }
    }
    public override string ToString() {
      return $"{GroupId} {GroupName}";
    }
    private static Group GetGroupFromLine(string line) {

      string[] newstr = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
      string name = newstr[1].ToString();
      string stringID = newstr[0].ToString();

      long ID = Convert.ToInt64(stringID.ToString());
      Group ISA = new Group(name, ID);
      return ISA;
    }
    public static Group[] GetGroupsArr(string fileName, int size = 1_000_000) {
      Group[] temp = new Group[size];
      using (StreamReader sr = new StreamReader(fileName, System.Text.Encoding.Default)) {
        string line;
        int i = 0;
        while ((line = sr.ReadLine()) != null) {
          temp[i] = GetGroupFromLine(line);
          i++;
          if(i == size) {
            break;
          }
        }
        if (size > i) {
          throw new Exception($"There are only { i } groups, not {size}");
        }
      }
      return temp;
    }
    public static void PrintGroups(Group[] groups) {
      for (int i = 0; i < groups.Length; ++i) {
        WriteLine(groups[i].ToString());
      }
    }
  }

  class Program {
    public static void StudentDemo() { 

    }
    static void Main(string[] args) {
      string groupFileName = @"C:\_0\groups_1_000_000.txt";
      string studentFileName = @"C:\_0\students_10_000_000.txt";
      try {
        var groupArr = Group.GetGroupsArr(groupFileName,7);
        Group.PrintGroups(groupArr);
        WriteLine();

        var studArr = Student.GetStudentsArr(studentFileName,6);
        Student.PrintStudents(studArr);
          
      } catch (Exception ex) {
        WriteLine(ex.Message);
      }
      WriteLine("\nEnd . . .");
      ReadLine();
    }
  }
}