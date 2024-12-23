using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace Fish
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFileButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
              
                string[] fileContent = File.ReadAllLines(openFileDialog.FileName);
                StartTimeTextBox.Text = fileContent[0];
                TemperaturesTextBox.Text = string.Join(" ", fileContent[1].Split(' '));
            }
        }

        private void CheckConditionsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                string fishName = FishNameTextBox.Text;
                int Tmax = int.Parse(TmaxTextBox.Text);
                int t1 = int.Parse(T1TextBox.Text);
                int Tmin = int.Parse(TminTextBox.Text);
                int t2 = int.Parse(T2TextBox.Text);
                DateTime startTime = DateTime.Parse(StartTimeTextBox.Text);
                List<int> temperatures = TemperaturesTextBox.Text.Split(' ').Select(int.Parse).ToList();

               
                bool conditionsViolated = false;
                List<string> violationsReport = new List<string>();

                int exceedTmaxTime = 0;
                int belowTminTime = 0;

                for (int i = 0; i < temperatures.Count; i++)
                {
                    int currentTemp = temperatures[i];
                    DateTime currentTime = startTime.AddMinutes(i * 10);

                    
                    if (currentTemp > Tmax)
                    {
                        exceedTmaxTime += 10;
                        if (exceedTmaxTime > t1)
                        {
                            conditionsViolated = true;
                            violationsReport.Add(
                                $"Время нарушения Tmax: {currentTime}, Требуемая температура: <= {Tmax}, Фактическая температура: {currentTemp}");
                        }
                    }
                    else
                    {
                        exceedTmaxTime = 0;
                    }

                    
                    if (currentTemp < Tmin)
                    {
                        belowTminTime += 10;
                        if (belowTminTime > t2)
                        {
                            conditionsViolated = true;
                            violationsReport.Add(
                                $"Время нарушения Tmin: {currentTime}, Требуемая температура: >= {Tmin}, Фактическая температура: {currentTemp}");
                        }
                    }
                    else
                    {
                        belowTminTime = 0;
                    }
                }

                string report = $"Вид рыбы: {fishName}\n" +
                                $"Максимально допустимая температура: {Tmax}°C на срок не более {t1} минут.\n" +
                                $"Минимально допустимая температура: {Tmin}°C на срок не более {t2} минут.\n" +
                                $"Дата/время начала измерения: {startTime}\n\n";

                if (conditionsViolated)
                {
                    report += "Условия хранения нарушены. Отчет о нарушениях:\n";
                    foreach (var violation in violationsReport)
                    {
                        report += violation + "\n";
                    }
                }
                else
                {
                    report += "Условия хранения соблюдены.";
                }

                ReportTextBox.Text = report;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveReportButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, ReportTextBox.Text);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
