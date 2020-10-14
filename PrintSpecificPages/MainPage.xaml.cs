using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Printing.OptionDetails;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PrintSpecificPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            dataGrid.PrintTaskRequested += DataGrid_PrintTaskRequested;
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.PrintSettings = new PrintSettings();
            dataGrid.PrintSettings.PrintManagerBase = new CustomPrintSpecificPages(this.dataGrid);
            dataGrid.PrintSettings.PrintManagerBase.Print();
        }

        private void DataGrid_PrintTaskRequested(object sender, DataGridPrintTaskRequestedEventArgs e)
        {
            e.PrintTask = e.Request.CreatePrintTask("Printing", sourceRequested =>
            {
                PrintTaskOptionDetails printDetailedOptions = PrintTaskOptionDetails.GetFromPrintTaskOptions(e.PrintTask.Options);
                IList<string> displayedOptions = printDetailedOptions.DisplayedOptions;
                displayedOptions.Add(Windows.Graphics.Printing.StandardPrintTaskOptions.CustomPageRanges);
                e.PrintTask.Options.PageRangeOptions.AllowCurrentPage = true;
                e.PrintTask.Options.PageRangeOptions.AllowAllPages = true;
                e.PrintTask.Options.PageRangeOptions.AllowCustomSetOfPages = true;
                sourceRequested.SetSource(e.PrintDocumentSource);
            });
        }
    }
}
