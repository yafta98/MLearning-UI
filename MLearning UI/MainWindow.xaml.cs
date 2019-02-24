﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MLearning_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<NetworkButtonPanel, NeuralNetwork> networks = new Dictionary<NetworkButtonPanel, NeuralNetwork>();
        public NeuralNetwork SelectedNetwork = null;
        public NetworkButtonPanel SelectedButton = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            NetworkInformationPanel.Visibility = Visibility.Hidden;
        }

        public DigitImage CurrentImage { get; set; }
        public DigitImage[] Images { get; set; }

        private void RunButtonClicked(object sender, RoutedEventArgs e)
        {
            double[] inputActivations = new double[SelectedNetwork.Size.InputLayerLength];
            for (int index = 0; index < inputActivations.Length; index++)
            {
                inputActivations[index] = CurrentImage.GetValueAt(index / 28, index % 28) / 256.0;
            }
            NetworkResult result = SelectedNetwork.Run(inputActivations);
            double[] output = result.Output;
            //SetOutputShades(output);
        }

        private void NetworkAddButton_Click(object sender, RoutedEventArgs e)
        {
            NetworkButtonPanel networkPanel = new NetworkButtonPanel
            {
                Height = 80
            };
            Border border = new Border
            {
                BorderBrush = Brushes.CadetBlue,
                Child = networkPanel,
                BorderThickness = new Thickness(2),
                Visibility = Visibility.Visible
            };
            NeuralNetwork network = new NeuralNetwork(new NetworkSize(784,new int[]{ 16 },10));
            bool isInitialized = true;
            networks.Add(networkPanel, network);
            networkPanel.NetworkName.Content = "Network 1";
            networkPanel.AccuracyLabel.Content = isInitialized ? "Initialized" : "Uninitialized";
            networkPanel.AccuracyLabel.Foreground = new SolidColorBrush()
            {
                Color = isInitialized ? Color.FromRgb(200, 255, 200) : Color.FromRgb(100, 100, 100)
            };
            networkPanel.NetworkSizeLabel.Content = "784-16-10";
            networkPanel.VerticalAlignment = VerticalAlignment.Top;
            NetworksListPanel.Children.Add(border);
            DockPanel.SetDock(border, Dock.Top);
            networkPanel.MouseDown += delegate (object sender2, MouseButtonEventArgs e2)
            {
                if (SelectedButton != null)
                    SelectedButton.Background = new SolidColorBrush()
                    {
                        Color = Color.FromRgb(255, 255, 255)
                    };
                SelectedNetwork = networks[networkPanel];
                SelectedButton = networkPanel;
                networkPanel.Background = new SolidColorBrush()
                {
                    Color = Color.FromRgb(200, 200, 200)
                };
                NetworkInformationPanel.Visibility = Visibility.Visible;
            };
        }

        private void NetworkDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            DeleteNetworkConfirmationWindow window = null;
            void closer() => window.Close();
            window = new DeleteNetworkConfirmationWindow(closer);
            window.Show();
        }

        private void DeleteNetwork()
        {
            networks.Remove(SelectedButton);
        }

        private void TrainButton_Clicked(object sender, RoutedEventArgs e)
        {
            SelectedNetwork.Train();
        }
    }
}
