using System;

namespace EcoGridEnergyDistributor
{
    abstract class PowerSource
    {
        private string sourceID;
        private double baseOutput;

        public string SourceID
        {
            get { return sourceID; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Source ID cannot be empty.");
                sourceID = value;
            }
        }

        public double BaseOutput
        {
            get { return baseOutput; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Base output cannot be negative.");
                baseOutput = value;
            }
        }

        public PowerSource(string id, double output)
        {
            SourceID = id;
            BaseOutput = output;
        }

        public virtual void GenerateReport()
        {
            Console.WriteLine($"Power Source {SourceID} producing {BaseOutput} kW.");
        }

        public virtual void GenerateReport(bool detailed)
        {
            if (detailed)
            {
                Console.WriteLine("=== Detailed Power Report ===");
                Console.WriteLine($"Source ID: {SourceID}");
                Console.WriteLine($"Base Output: {BaseOutput} kW");
            }
            else
            {
                GenerateReport();
            }
        }
    }

    class SolarPanel : PowerSource
    {
        private double sunlightPercent;

        public double SunlightPercent
        {
            get { return sunlightPercent; }
            set
            {
                if (value < 0 || value > 100)
                    throw new ArgumentException("Sunlight percent must be between 0 and 100.");
                sunlightPercent = value;
            }
        }

        public SolarPanel(string id, double output, double sunlight)
            : base(id, output)
        {
            SunlightPercent = sunlight;
        }

        public override void GenerateReport()
        {
            double finalOutput = BaseOutput * (SunlightPercent / 100);
            Console.WriteLine($"Solar Panel [{SourceID}] Effective Output: {finalOutput:F2} kW");
        }
    }

    class WindTurbine : PowerSource
    {
        private double windSpeed;

        public double WindSpeed
        {
            get { return windSpeed; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Wind speed cannot be negative.");
                windSpeed = value;
            }
        }

        public WindTurbine(string id, double output, double wind)
            : base(id, output)
        {
            WindSpeed = wind;
        }

        public override void GenerateReport()
        {
            double finalOutput = BaseOutput * (WindSpeed / 10);
            Console.WriteLine($"Wind Turbine [{SourceID}] Effective Output: {finalOutput:F2} kW");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ECO-GRID ENERGY DISTRIBUTOR ===\n");

            // Get user input for Solar Panel
            string solarID = GetStringInput("Enter Solar Panel ID: ");
            double solarOutput = GetPositiveDoubleInput("Enter Solar Panel Base Output (kW): ");
            double sunlightPercent = GetDoubleInRange("Enter Sunlight Percent (0-100): ", 0, 100);

            PowerSource solar = new SolarPanel(solarID, solarOutput, sunlightPercent);

            // Get user input for Wind Turbine
            string windID = GetStringInput("Enter Wind Turbine ID: ");
            double windOutput = GetPositiveDoubleInput("Enter Wind Turbine Base Output (kW): ");
            double windSpeed = GetPositiveDoubleInput("Enter Wind Speed (m/s): ");

            PowerSource wind = new WindTurbine(windID, windOutput, windSpeed);

            Console.WriteLine("\n--- Summary Report ---");
            solar.GenerateReport();
            wind.GenerateReport();

            Console.WriteLine("\n--- Detailed Report ---");
            solar.GenerateReport(true);
            wind.GenerateReport(true);

            Console.WriteLine("\nSystem running successfully.\nSession Ended.");
        }

        // Input validation helper methods
        static string GetStringInput(string prompt)
        {
            string input;
            do
            {
                Console.Write(prompt);
                input = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input))
                    Console.WriteLine("Input cannot be empty. Please try again.");
            } while (string.IsNullOrWhiteSpace(input));
            return input;
        }

        static double GetPositiveDoubleInput(string prompt)
        {
            double result;
            do
            {
                Console.Write(prompt);
                if (!double.TryParse(Console.ReadLine(), out result) || result < 0)
                {
                    Console.WriteLine("Invalid input. Must be a positive number. Try again.");
                    result = -1;
                }
            } while (result < 0);
            return result;
        }

        static double GetDoubleInRange(string prompt, double min, double max)
        {
            double result;
            do
            {
                Console.Write(prompt);
                if (!double.TryParse(Console.ReadLine(), out result) || result < min || result > max)
                {
                    Console.WriteLine($"Invalid input. Must be between {min} and {max}. Try again.");
                    result = -1;
                }
            } while (result < min || result > max);
            return result;
        }
    }
}