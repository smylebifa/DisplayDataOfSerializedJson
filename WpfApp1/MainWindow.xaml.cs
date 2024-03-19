using System.Windows;
using System.Text.Json;
using System.IO;

namespace WpfApp1
{
    public class PointsData
    {
        public double X { get; set; }
        public double Y { get; set; }
    }

    public class ModuleData
    {
        public int Id { get; set; }
        public PointsData[] Points { get; set; }
    }

    public class MeteringData
    {
        public double Speed { get; set; }
        public double Distance { get; set; }
        public ModuleData[] MeteringProfiles { get; set; }
    }
    

    public class MeteringsData
    {
        public string Id { get; set; }
        public MeteringData[] Meterings { get; set; }
    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Yout absolute path to json file
            string path = "C:\\Users\\samoi\\Source\\Repos\\WpfApp1\\WpfApp1\\src\\data.json";
            try
            {
                string json = File.ReadAllText(path);

                JsonSerializerOptions options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;
                var meteringsData = JsonSerializer.Deserialize<MeteringsData>(json, options);

                // set size for using sample for visualization
                int size = meteringsData.Meterings.Length;

                List<double> dataX = new List<double>();
                List<double> dataY = new List<double>();
                List<double>[] arrX = new List<double>[size];
                List<double>[] arrY = new List<double>[size];

                var myPalette = new ScottPlot.Palettes.Category20();

                int c = 0;
                for (int i = 0; i < meteringsData.Meterings.Length; i++)
                {
                    var meteringData = meteringsData.Meterings[i];

                    for (int j = 0; j < meteringData.MeteringProfiles.Length; j++)
                    {
                        var metering = meteringData.MeteringProfiles[j];
                        dataX = new List<double>();
                        dataY = new List<double>();
                        for (int k = 0; k < metering.Points.Length - 1; k++)
                        {
                            dataX.Add(metering.Points[k].X);
                            dataY.Add(metering.Points[k].Y);
                        }
                        if (dataX.Count > 0 && dataY.Count > 0)
                        {
                            arrX[c] = dataX;
                            arrY[c] = dataY;

                            WpfPlot1.Plot.Add.Scatter(arrX[c], arrY[c], myPalette.GetColor(c));
                            WpfPlot1.Refresh();

                            c++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}