using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
/// </summary>
/// <Student>Daniel Mack</Student>
/// <Class>CIS297</Class>
/// <Semester>Winter 2022</Semester>
namespace PatientRecordApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            FileOperations();
            DirectoryOperations();
            FileStreamOperations();
            SequentialAccessWriteOperation();
            ReadSequentialAccessOperation();
            FindPatients();
            SerializableDemonstration();
        }
        //File operations
        static void FileOperations()
        {
            string fileName;
            Write("Enter a filename >> ");
            fileName = ReadLine();
            if (File.Exists(fileName))
            {
                WriteLine("File exists");
                WriteLine("File was created " +
                   File.GetCreationTime(fileName));
                WriteLine("File was last written to " +
                   File.GetLastWriteTime(fileName));
            }
            else
            {
                WriteLine("File does not exist");
            }
        }
        //Directory Operations
        static void DirectoryOperations()
        {
            //Directory operations
            string directoryName;
            string[] listOfFiles;
            Write("Enter a folder >> ");
            directoryName = ReadLine();
            if (Directory.Exists(directoryName))
            {
                WriteLine("Directory exists, and it contains the following:");
                listOfFiles = Directory.GetFiles(directoryName);
                for (int x = 0; x < listOfFiles.Length; ++x)
                    WriteLine("   {0}", listOfFiles[x]);
            }
            else
            {
                WriteLine("Directory does not exist");
            }
        }
        //Using FileStream to create and write some text into it
        static void FileStreamOperations()
        {
            FileStream outFile = new
            FileStream("SomeText.txt", FileMode.Create,
            FileAccess.Write);
            StreamWriter writer = new StreamWriter(outFile);
            Write("Enter some text >> ");
            string text = ReadLine();
            writer.WriteLine(text);
            // Error occurs if the next two statements are reversed
            writer.Close();
            outFile.Close();
        }
        //Writing data to a Sequential Access text file
        static void SequentialAccessWriteOperation()
        {
            const int END = 999;
            const string DELIM = ",";
            const string FILENAME = "PatientsData.txt";
            Patient emp = new Patient();
            FileStream outFile = new FileStream(FILENAME,
               FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(outFile);
            Write("Enter Patient ID number or " + END +
               " to quit >> ");
            emp.Id_Number = Convert.ToInt32(ReadLine());
            while (emp.Id_Number != END)
            {
                Write("Enter the Customer's name >> ");
                emp.Name = ReadLine();
                Write("Enter the Medical Bill >> ");
                emp.Patient_Bills = Convert.ToDouble(ReadLine());
                writer.WriteLine(emp.Id_Number + DELIM + emp.Name +
                   DELIM + emp.Patient_Bills);
                Write("Enter next patient id number or " +
                   END + " to quit >> ");
                emp.Id_Number = Convert.ToInt32(ReadLine());
            }
            writer.Close();
            outFile.Close();
        }
        //Read data from a Sequential Access File
        static void ReadSequentialAccessOperation()
        {
            const char DELIM = ',';
            const string FILENAME = "PatientsData.txt";
            Patient emp = new Patient();
            FileStream inFile = new FileStream(FILENAME,
               FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn;
            string[] fields;
            WriteLine("\n{0,-20}{1,-20}{2,-20}\n",
               "Patient ID", "Name", "Medical Bills");
            recordIn = reader.ReadLine();
            while (recordIn != null)
            {
                fields = recordIn.Split(DELIM);
                emp.Id_Number = Convert.ToInt32(fields[0]);
                emp.Name = fields[1];
                emp.Patient_Bills = Convert.ToDouble(fields[2]);
                WriteLine("{0,-20}{1,-20}{2,-20}",
                   emp.Id_Number, emp.Name, emp.Patient_Bills.ToString("C"));
                recordIn = reader.ReadLine();
            }
            reader.Close();
            inFile.Close();
        }
        //repeatedly searches a file to produce 
        //lists of employees who meet a minimum Patient_Bills requirement
        static void FindPatients()
        {
            const char DELIM = ',';
            const int END = 999;
            const string FILENAME = "PatientsData.txt";
            Patient emp = new Patient();
            FileStream inFile = new FileStream(FILENAME,
               FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn;
            string[] fields;
            double minPatient_Bills;
            Write("Enter minimum Patient_Bills to find or " +
               END + " to quit >> ");
            minPatient_Bills = Convert.ToDouble(Console.ReadLine());
            while (minPatient_Bills != END)
            {
                WriteLine("\n{0,-20}{1,-20}{2,-20}\n",
                   "Patient ID", "Name", "Patient_Bills");
                inFile.Seek(0, SeekOrigin.Begin);
                recordIn = reader.ReadLine();
                while (recordIn != null)
                {
                    fields = recordIn.Split(DELIM);
                    emp.Id_Number = Convert.ToInt32(fields[0]);
                    emp.Name = fields[1];
                    emp.Patient_Bills = Convert.ToDouble(fields[2]);
                    if (emp.Patient_Bills >= minPatient_Bills)
                        WriteLine("{0,-20}{1,-20}{2,-20}", emp.Id_Number,
                           emp.Name, emp.Patient_Bills.ToString("C"));
                    recordIn = reader.ReadLine();
                }
                Write("\nEnter minimum Patient_Bills to find or " +
                   END + " to quit >> ");
                minPatient_Bills = Convert.ToDouble(Console.ReadLine());
            }
            reader.Close();  // Error occurs if
            inFile.Close(); //these two statements are reversed
        }
        //Serializable Demonstration
        /// <summary>
        /// writes Person class objects to a file and later reads them 
        /// from the file into the program
        /// </summary>
        static void SerializableDemonstration()
        {
            const int END = 999;
            const string FILENAME = "Data.ser";
            Person emp = new Person();
            FileStream outFile = new FileStream(FILENAME,
               FileMode.Create, FileAccess.Write);
            BinaryFormatter bFormatter = new BinaryFormatter();
            Write("Enter the Patient ID number or " + END +
               " to quit >> ");
            emp.Id_Number = Convert.ToInt32(ReadLine());
            while (emp.Id_Number != END)
            {
                Write("Enter the Customer's name >> ");
                emp.Name = ReadLine();
                Write("Enter Patient_Bills >> ");
                emp.Patient_Bills = Convert.ToDouble(ReadLine());
                bFormatter.Serialize(outFile, emp);
                Write("Enter the Patient ID number or " + END +
                   " to quit >> ");
                emp.Id_Number = Convert.ToInt32(ReadLine());
            }
            outFile.Close();
            FileStream inFile = new FileStream(FILENAME,
               FileMode.Open, FileAccess.Read);
            WriteLine("\n{0,-20}{1,-20}{2,-20}\n",
               "Patient ID", "Name", "Patient_Bills");
            while (inFile.Position < inFile.Length)
            {
                emp = (Person)bFormatter.Deserialize(inFile);
                WriteLine("{0,-20}{1,-20}{2,-20}",
                   "A" + emp.Id_Number, emp.Name, emp.Patient_Bills.ToString("C"));
            }
            inFile.Close();
        }
    }
    class Patient
    {
        public int Id_Number { get; set; }
        public string Name { get; set; }
        public double Patient_Bills { get; set; }
    }

    [Serializable]
    class Person
    {
        public int Id_Number { get; set; }
        public string Name { get; set; }
        public double Patient_Bills { get; set; }
    }
}
