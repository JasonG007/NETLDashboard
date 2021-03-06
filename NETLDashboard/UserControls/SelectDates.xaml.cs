﻿using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NETLDashboard
{
    /// <summary>
    /// Interaction logic for SelectDates.xaml
    /// </summary>
    public partial class SelectDates : UserControl
    {
        String startDate;
        String endDate;
        private HistoricalGraph[] graphNameArray;
       
        public SelectDates(int numberOfGraphs, String[] procedureArray, String[] labelArray)
        {
            graphNameArray = new HistoricalGraph[numberOfGraphs];
            
            InitializeComponent();
            Start.DisplayDateStart = DateTime.Now - TimeSpan.FromDays(90);
            End.DisplayDateStart = DateTime.Now - TimeSpan.FromDays(90);


            for (int i = 0; i < 2; i++)
            {
                historicalViewArea.ColumnDefinitions.Add(new ColumnDefinition());
                historicalViewArea.ColumnDefinitions[i].Width = new GridLength(50,GridUnitType.Star);
            }

            for (int i = 0; i < Math.Ceiling(numberOfGraphs / 2.0); i++)
            {
                historicalViewArea.RowDefinitions.Add(new RowDefinition());
                historicalViewArea.RowDefinitions[i].Height = new GridLength(400);
            }

            int k = 0;
            for (int i = 0; i < Math.Ceiling(numberOfGraphs / 2.0 ); i++)
            {
                for (int j = 0; j < 2; j++)
                {

                    if (k < numberOfGraphs)
                    {
                        graphNameArray[k] = new HistoricalGraph(procedureArray[k], labelArray[k]);
                        historicalViewArea.Children.Add(graphNameArray[k]);
                        Grid.SetRow(graphNameArray[k], i);
                        Grid.SetColumn(graphNameArray[k], j);
                        k++;
                    }
                    
                    
                }
            }

            

           


        }
        private void ResetZoomOnClick(object sender, RoutedEventArgs e)
        {
            //Use the axis MinValue/MaxValue properties to specify the values to display.
            //use double.Nan to clear it.
            for (int i = 0; i < graphNameArray.Length; i++)
            {
                graphNameArray[i].X.MinValue = double.NaN;
                graphNameArray[i].X.MaxValue = double.NaN;
                graphNameArray[i].Y.MinValue = double.NaN;
                graphNameArray[i].Y.MaxValue = double.NaN;
            }
        }

        private void Selectdates(object sender, RoutedEventArgs e)
        {
            //Create DatePicker selection window, then redraw the entire graph
            for(int i = 0; i< graphNameArray.Length; i++ )
            {
                if(graphNameArray[i].SeriesCollection.Count !=0)
                {
                    graphNameArray[i].SeriesCollection.Clear();
                }

                startDate = Start.SelectedDate.Value.Date.ToString("yyyyMMdd");
                endDate = End.SelectedDate.Value.Date.ToString("yyyyMMdd");

                if (startDate.CompareTo(endDate) > 0)
                {
                    MessageBox.Show("Start date cannot be after end date.", "Dates Mismatched");
                    return;
                }

                graphNameArray[i].SeriesCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Values = graphNameArray[i].GetData(startDate,endDate),
                        StrokeThickness = 0,
                        PointGeometrySize = 3
                        
                    }
                };

                graphNameArray[i].ZoomingMode = ZoomingOptions.X;
                graphNameArray[i].XFormatter = val => new DateTime((long)val).ToString("MM/dd/yy hh:mm");
                graphNameArray[i].YFormatter = val => val.ToString("G");
                graphNameArray[i].DataContext = graphNameArray[i];

            }
        }
    } 
}
