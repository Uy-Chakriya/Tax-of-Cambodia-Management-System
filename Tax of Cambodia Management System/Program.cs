using System;

namespace final
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Salary taxCalc = new Salary();
            Console.Title = "Cambodia Tax Calculator 2026";
            DrawHeader("CAMBODIA TAX SYSTEM");

            while (true)
            {
                Console.WriteLine("\n" + new string('-', 40));
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" [1] Salary Tax  ");
                Console.WriteLine(" [2] Property Tax ");
                Console.WriteLine(" [3] Vehicle Tax  ");
                Console.WriteLine(" [4] Exit");

                Console.ResetColor();
                Console.WriteLine(new string('-', 40));

                int choice = ReadInt("Please select an option: ");
                if (choice == 4) break;

                ProcessSelection(choice, taxCalc);
                Console.WriteLine("\n--- End of Calculation ---");
            }
        }

        static void ProcessSelection(int choice, Salary ob)
        {
            switch (choice)
            {
                case 1: RunSalaryTax(ob); break;
                case 2: RunPropertyTax(ob); break;
                case 3: RunVehicleTax(ob); break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("(!) Invalid selection. Try again.");
                    Console.ResetColor();
                    break;
            }
        }

        //==>> Salary Tax Calcuulation
        static void RunSalaryTax(Salary ob)
        {
            Console.WriteLine("\n ====>> SALARY TAX <<==== \n");
            Console.Write("Status (1: Single, 2: Married): ");
            int status = ReadInt("");
            double gs = ReadDouble("Enter Gross Salary (Riel): ");

            double tax;
            if (status == 2)
            {
                int children = ReadInt("Enter Number of Children: ");
                tax = ob.Tax1(gs, children, 1);
            }
            else
            {
                tax = ob.Tax2(gs);
            }

            DisplayResult("Monthly Tax", tax);
            DisplayResult("Net Salary", ob.NetSalary1(gs, tax));
        }

        static void RunPropertyTax(Salary ob)
        {
            Console.WriteLine("\n====>> PROPERTY TAX <<==== \n");
            Console.WriteLine("Type: 1. House | 2. Land");
            int type = ReadInt("Select Type: ");
            double price = ReadDouble("Enter Property Value (Riel): ");
            DisplayResult("Property Tax", ob.Property(price));
        }

        static void RunVehicleTax(Salary ob)
        {
            Console.WriteLine("\n ====>> VEHICLE TAX <<==== \n");
            Console.WriteLine("Type: 1. Electronic Vehicle | 2. Pick up");
            int type = ReadInt("Select Vehicle Type: ");

            if (type == 1)
            {
                double hp = ReadDouble("Enter Horsepower (HP): ");
                Console.WriteLine("Age: 1. New (<5 yrs) | 2. 5-10 yrs | 3. Over 10 yrs");
                int ageCategory = ReadInt("Select Age Category: ");

                double result = ob.CalculateEVTax(hp, ageCategory);
                DisplayResult("EV Road Tax Amount", result);
            }
            else
            {
                double engineSize = ReadDouble("Enter Engine Size (CC): ");
                double result = ob.CalculateVehicleTax(2, engineSize);
                DisplayResult("Combustion Road Tax Amount", result);
            }
        }

        static void DrawHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("****************************************");
            Console.WriteLine($"*** {title} ***");
            Console.WriteLine("****************************************");
            Console.ResetColor();
        }

        static void DisplayResult(string label, double value)
        {
            Console.Write($"{label}: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{value:#,##0} KHR");
            Console.ResetColor();
        }

        static int ReadInt(string prompt)
        {
            int val;
            Console.Write(prompt);
            while (!int.TryParse(Console.ReadLine(), out val))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid input! Enter a whole number: ");
                Console.ResetColor();
            }
            return val;
        }

        static double ReadDouble(string prompt)
        {
            double val;
            Console.Write(prompt);
            while (!double.TryParse(Console.ReadLine(), out val))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Invalid input! Enter a valid amount: ");
                Console.ResetColor();
            }
            return val;
        }
    }

    
    public class Salary
    {
        private const double ALLOWANCE_PER_DEPENDENT = 150000;
        private const double PROPERTY_TAX_THRESHOLD = 100000000;

        public double Tax1(double grossSalary, double children, double marriage)
        {
            // Form of the Allowance
            double allowance = (1 + children) * ALLOWANCE_PER_DEPENDENT; // update
            double taxableSalary = Math.Max(0, grossSalary - allowance);
            return CalculateSalaryTaxBands(taxableSalary);
        }

        public double Tax2(double grossSalary) => CalculateSalaryTaxBands(grossSalary);

        private double CalculateSalaryTaxBands(double amount)
        {
            if (amount <= 1500000) return 0;
            if (amount <= 2000000) return (amount * 0.05) - 75000;
            if (amount <= 8500000) return (amount * 0.10) - 175000;
            if (amount <= 12500000) return (amount * 0.15) - 600000;
            return (amount * 0.20) - 1225000;
        }

        public double NetSalary1(double gross, double tax) => gross - tax;

        public double Property(double price)
        {
            double taxableValue = (price * 0.8) - PROPERTY_TAX_THRESHOLD;
            return taxableValue <= 0 ? 0 : taxableValue * 0.001;
        }

        public double CalculateEVTax(double hp, int ageCategory)
        {
            if (hp <= 150)
            {
                if (ageCategory == 1) return 100000;
                if (ageCategory == 2) return 80000;
                return 60000;
            }
            else if (hp <= 200)
            {
                if (ageCategory == 1) return 150000;
                if (ageCategory == 2) return 100000;
                return 80000;
            }
            else if (hp <= 250)
            {
                if (ageCategory == 1) return 300000;
                if (ageCategory == 2) return 150000;
                return 100000;
            }
            else
            {
                if (ageCategory == 1) return 800000;
                if (ageCategory == 2) return 400000;
                return 200000;
            }
        }

        public double CalculateVehicleTax(int type, double engineSize)
        {
            if (engineSize <= 1000) return 100000;
            if (engineSize <= 2000) return 300000;
            return 600000;
        }
    }
}
