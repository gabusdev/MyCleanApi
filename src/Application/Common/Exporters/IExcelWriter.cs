namespace Application.Common.Exporters;

// Interface to Export Data to Excel Book

public interface IExcelWriter
{
    /// <summary>
    /// Transforms the Data to a Stream that can be downloaded as Excel Book
    /// </summary>
    /// <typeparam name="T">Type of the data</typeparam>
    /// <param name="data">The List of Data</param>
    /// <returns>The Stream Object representing an Excel Book</returns>
    Stream WriteToStream<T>(IList<T> data);
}