using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Arbeitszeitaufschreiben;
using Arbeitszeitaufschreiben.ViewModels;

namespace Arbeitszeitaufschreiben.Views
{
    public partial class MainWindow : Window
    {
        private readonly string _jsonFilePath;
        private MainWindowViewModel ViewModel => (MainWindowViewModel)DataContext!;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            string? projectRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
            _jsonFilePath = Path.Combine(projectRoot!, "Arbeitszeitaufschreiben.json");
        }

        private void Speichern(object? sender, RoutedEventArgs e)
        {
            string? start = Startzeit.Text;
            string? end = Endzeit.Text;
            string? pause = Mittagspause.Text;
            string? datum = Datum.Text;

            if (string.IsNullOrWhiteSpace(start) || string.IsNullOrWhiteSpace(end) || string.IsNullOrWhiteSpace(pause) || string.IsNullOrWhiteSpace(datum))
                return;

            int startzeitMinuten = Convert.ToInt32(start.Split(':')[0]) * 60 + Convert.ToInt32(start.Split(':')[1]);
            int endzeitMinuten = Convert.ToInt32(end.Split(':')[0]) * 60 + Convert.ToInt32(end.Split(':')[1]);
            int pauseMinuten = Convert.ToInt32(pause.Split(':')[0]) * 60 + Convert.ToInt32(pause.Split(':')[1]);
            int gesamtMinuten = endzeitMinuten - startzeitMinuten - pauseMinuten;

            var neuerEintrag = new ArbeitszeitEintrag
            {
                Startzeit = startzeitMinuten,
                Endzeit = endzeitMinuten,
                Pause = pauseMinuten,
                Gesamt = gesamtMinuten,
                Datum = datum
            };

            // Load, add, and save
            List<ArbeitszeitEintrag> eintraege = File.Exists(_jsonFilePath)
                ? JsonSerializer.Deserialize<List<ArbeitszeitEintrag>>(File.ReadAllText(_jsonFilePath)) ?? new List<ArbeitszeitEintrag>()
                : new List<ArbeitszeitEintrag>();

            eintraege.Add(neuerEintrag);

            File.WriteAllText(_jsonFilePath, JsonSerializer.Serialize(eintraege, new JsonSerializerOptions { WriteIndented = true }));

            // Update ViewModel collection
            ViewModel.Arbeitszeiten.Clear();
            foreach (var eintrag in eintraege)
                ViewModel.Arbeitszeiten.Add(eintrag);
        }

        private void Abbrechen(object? sender, RoutedEventArgs e)
        {
            Startzeit.Text = "";
            Endzeit.Text = "";
            Mittagspause.Text = "";
            Datum.Text = "";
        }

        private void Uebersicht_Click(object? sender, RoutedEventArgs e)
        {
            MainStackPanel.IsVisible = false;
            UebersichtStackPanel.IsVisible = true;

            if (File.Exists(_jsonFilePath))
            {
                string jsonText = File.ReadAllText(_jsonFilePath);
                var arbeitszeiteintraege = JsonSerializer.Deserialize<List<ArbeitszeitEintrag>>(jsonText) ?? new List<ArbeitszeitEintrag>();
                ViewModel.Arbeitszeiten.Clear();
                foreach (var eintrag in arbeitszeiteintraege)
                {
                    ViewModel.Arbeitszeiten.Add(eintrag);
                }
            }
        }

        private void zurück_Click(object? sender, RoutedEventArgs e)
        {
            MainStackPanel.IsVisible = true;
            UebersichtStackPanel.IsVisible = false;
        }
    }
}