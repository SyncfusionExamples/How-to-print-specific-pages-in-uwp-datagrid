using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Xaml.Printing;

namespace PrintSpecificPages
{
    public class CustomPrintSpecificPages : GridPrintManager
    {
        public CustomPrintSpecificPages(SfDataGrid grid)
            : base(grid)
        {

        }

        protected override void OnAddPrintPages(AddPagesEventArgs e)
        {
            IList<Windows.Graphics.Printing.PrintPageRange> customPageRanges = e.PrintTaskOptions.CustomPageRanges;
            int pageCount = this.PageDictionary.Count;

            // An empty CustomPageRanges means "All Pages"
            if (customPageRanges.Count == 0)
            {
                // Loop over all of the preview pages and add each one to be printed
                for (var i = 1; i <= pageCount; i++)
                {
                    var printpageControl = CreatePage(i);
                    PrintDocument.AddPage(printpageControl);
                }
            }
            else
            {
                // Print only the pages chosen by the user.
                // 
                // The "Current page" option is a special case of "Custom set of pages".
                // In case the user selects the "Current page" option, the PrintDialog
                // will turn that into a CustomPageRanges containing the page that the user was looking at.
                // If the user typed in an indefinite range such as "6-", the LastPageNumber value
                // will be whatever this sample app last passed into the PrintDocument.SetPreviewPageCount API.
                foreach (PrintPageRange pageRange in customPageRanges)
                {
                    // The user may type in a page number that is not present in the document.
                    // In this case, we just ignore those pages, hence the checks
                    // (pageRange.FirstPageNumber <= printPreviewPages.Count) and (i <= printPreviewPages.Count).
                    //
                    // If the user types the same page multiple times, it will be printed multiple times
                    // (e.g 3-4;1;1 will print pages 3 and 4 followed by two copies of page 1)
                    if (pageRange.FirstPageNumber <= pageCount)
                    {
                        for (int i = pageRange.FirstPageNumber; (i <= pageRange.LastPageNumber) && (i <= pageCount); i++)
                        {
                            // Subtract 1 because page numbers are 1-based, but our list is 0-based.
                            var printpageControl = CreatePage(i);
                            PrintDocument.AddPage(printpageControl);
                        }
                    }
                }
            }
            // Indicate that all of the print pages have been provided.
            PrintDocument.AddPagesComplete();
        }
    }
}