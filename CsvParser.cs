using System.Collections.Generic;

public static class CsvParser
{
	/// <summary>
	/// Returns a csv formatted text as a 2D string List. </summary>
	public static List<List<string>> ParseList(string csvContent) 
	{
		List<List<string>> csvTable = new List<List<string>>();
		int currentRow = 0;
		string cell;
		int i = 0;

		csvContent = csvContent.Trim();
		csvContent = csvContent.Replace("\r", "");

		if (string.IsNullOrEmpty(csvContent))
			return null;

		// Look to each char of the csv content.
		csvTable.Add(new List<string>());
		while (i < csvContent.Length)
		{
			cell = "";

			// Get the content of the cell, moving i to the next separator between cells.
			if (csvContent[i] == '"')
				cell += GetQuotedCell(csvContent, ref i);
			else
				cell += GetSimpleCell(csvContent, ref i);
			// Add the cell to the current row.
			csvTable[currentRow].Add(cell);

			// If there is a \n between cells, add a row.
			if (i < csvContent.Length && csvContent[i] == '\n')
			{
				csvTable.Add(new List<string>());
				currentRow++;
			}
			i++;    /// Add one to go to the next char from the coma or the new line, the first char of the next cell.
		}

		// If the last cell was empty, we skipped it in the loop, so add an empty cell to the last row.
		if (i == csvContent.Length)
			csvTable[currentRow].Add("");

		return csvTable;
	}

	/// <summary>
	/// Returns a csv formatted text as a 2D string Array. </summary>
	public static string[][] ParseArray(string csvContent)
	{
		// Get a 2D list form the content of the file.
		List<List<string>> twoDList = ParseList(csvContent);
		if (twoDList == null)
			return null;

		// Turn the 2D list into a 2D array.
		List<string>[] listArray = twoDList.ToArray();
		string[][] csvTable = new string[listArray.Length][];

		for (int i = 0; i < csvTable.Length; i++)
			csvTable[i] = listArray[i].ToArray();

		return csvTable;
	}


	// ------------------------------------------------------------------------------------

	/// <summary>
	/// Returns a string that goes from csvContent[i] to the next coma or \n.
	/// <para> Moving i to be the position of that coma or \n. Or csvContent.Length in the case of the last cell. </para></summary>
	private static string GetSimpleCell(string csvContent, ref int i)
	{
		string cell = "";

		while (i < csvContent.Length && csvContent[i] != ',' && csvContent[i] != '\n')
		{
			cell += csvContent[i];
			i++;
		}

		return cell;
	}

	/// <summary>
	/// Returns a string that goes from csvContent[i + 1] to the next single quotation marks.
	/// <para> Moving i to be the position of the following coma or \n. Or csvContent.Length in the case of the last cell. </para></summary>
	private static string GetQuotedCell(string csvContent, ref int i) 
	{
		string cell = "";

		i++;		/// Jump over the quoation marks the index is at.
		while (i < csvContent.Length)
		{
			// If we are at a quotation marks not followed by another ones, the cell has ended.
			if (i < csvContent.Length - 1 && csvContent[i] == '"' && csvContent[i + 1] != '"')
				break;
			// If there are two consecutive quotation marks, jump the first one to only save one of them.
			if (csvContent[i] == '"')
				i++;

			if (i < csvContent.Length)
				cell += csvContent[i];
			i++;
		}

		// Go to the next comma or \n.
		while (i < csvContent.Length && csvContent[i] != ',' && csvContent[i] != '\n')
			i++;

		return cell;
	}
}
