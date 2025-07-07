using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using Avalonia.Controls;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Arbeitszeitaufschreiben.Views;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    public new event PropertyChangedEventHandler? PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this; 
        Uebersicht_Click(null, null); 
    }


    public ObservableCollection<ArbeitszeitEintrag> Eintraege { get; set; } = new();


    private void Speichern(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        string? start = Startzeit.Text;
        string? end = Endzeit.Text;
        string? pause = Mittagspause.Text;
        string? datum = Datum.Text;

        int startzeitMinuten = Convert.ToInt32(start!.Split(':')[0]) * 60 + Convert.ToInt32(start.Split(':')[1]);
        int endzeitMinuten = Convert.ToInt32(end!.Split(':')[0]) * 60 + Convert.ToInt32(end.Split(':')[1]);
        int pauseMinuten = Convert.ToInt32(pause!.Split(':')[0]) * 60 + Convert.ToInt32(pause.Split(':')[1]);
        int gesamtMinuten = endzeitMinuten - startzeitMinuten - pauseMinuten;

        var neuerEintrag = new ArbeitszeitEintrag
        {
            Startzeit = startzeitMinuten,
            Endzeit = endzeitMinuten,
            Pause = pauseMinuten,
            Gesamt = gesamtMinuten,
            Datum = datum
        };

        string? projectRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;
        string pfad = Path.Combine(projectRoot!, "Arbeitszeit-Tracker.json");

        List<ArbeitszeitEintrag> eintraege = File.Exists(pfad)
            ? JsonSerializer.Deserialize<List<ArbeitszeitEintrag>>(File.ReadAllText(pfad)) ?? new List<ArbeitszeitEintrag>()
            : new List<ArbeitszeitEintrag>();

        eintraege.Add(neuerEintrag);

        File.WriteAllText(pfad, JsonSerializer.Serialize(eintraege, new JsonSerializerOptions { WriteIndented = true }));
    }

    private void Abbrechen(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        
    }
    
   
    private void Uebersicht_Click(object? sender, RoutedEventArgs? e)
    {
        MainStackPanel.IsVisible = false;
        UebersichtStackPanel.IsVisible = true;

        
    }

    
    private void zurück_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        MainStackPanel.IsVisible = true;
        UebersichtStackPanel.IsVisible = false;
    }
}