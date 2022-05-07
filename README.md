# CSV Parser
These functions allow you to turn the content of a ``.csv`` file into an easy to use 2D table.

---
* Documentation
    * [Static Functions](#Static-Functions)
    * [Example](#Example)
* [About the CSV Format](#CSV-Format)
* [About this code](#Code)

---
## Documentation
### Static Functions

#### ``List<List<string>> ParseList(string csvContent)``
Returns a csv formatted text as a 2D string list.

#### ``String[][] ParseArray(string csvContent)``
Returns a csv formatted text as a 2D string array.

---
### Example
The example below shows how to use these functions to turn the content of a ``.csv`` file into a 2D string table so we can write to the console the content of each cell, separating each row with a line.
The functions receive the content of the file as a string, so you need to open and read the file beforehand. Both functions return ``null`` if the content is empty or only contains white spaces.

````c#
void WriteTable()
{
    string file = "./file.csv";
    string fileContent = File.ReadAllText(file);
    List<List<string>> csvTable = CsvParser.ParseList(fileContent);

    if (csvTable == null)
    {
        Console.WriteLine("The file is empty.");
        return;
    }

    foreach ( List<string> row in csvTable )
    {
        foreach (string cell in row)
        {
            Console.WriteLine(">>> " + cell);
        }
        Console.WriteLine("==========");
    }
}
````

---
## CSV Format

The [comma-separated values](https://en.wikipedia.org/wiki/Comma-separated_values) or ``csv``  file stores a simple table of text. It doesn’t store info about the font, color, size or anything else. Only the text that is in each cell. You can open a ``.csv`` file with your notepad to see how it actually looks. It is only plain, human-readable text that follows some simple rules:
* Each cell is separeted by a coma (``,``).
* Each row is separated by a new line (``\n``).
* If a cell contains one of those two special characters, the cell is containd between quotation marks (``"``).
* If the cell contains quotation marks, the ones that are part of the content of the cell are duplicated and appear in the raw file as (``""``).

So, for a table that looks like this:
|||
|-|-|
| A1             | B1             |
| A2             | Hello World!   |
| A3             | Hello, World!  |
| A4             | Hello<\n>World!   |
| A5             | "Hello Wolrd!" |
| A6             | Hello " World   |

The raw ``.csv`` will look like this:
````
A1,B1
A2,Hello World!
A3,"Hello, World!"
A4,"Hello
World!"
A5,"""Hello Wolrd!"""
A6,"Hello "" World"

````

> Note: The coma, new line and quotation mark characters are the convention and the most commonly used for these purposes. Your spreadsheets editor may use different special characters or allow you to change which ones are used. But this is not currently supported by this code.

---
## Code

The list parsing function (``ParseList(csvContent)``) looks at each char of the csv content in a while loop. If the first char is a quotation mark (``"``), the cell extends to the next appearance of a non-duplicated one. Saving every doble quitation marks as only one (``""``->``"``).  If there is not, the cell  ends with the next coma (``,``) or new line (``\n``). 
Now, with the loop index at the separator between two cells, the found cell is added to the current last row of the table. And, if the separator is a ``\n``, a new row is added to the table.
We add one more to the index to set it at the next character from the separator, the first char of the new cell, for the loop to repeat.

In the case of the last cell there is not a separator ``,`` or ``\n`` at the end. So, at the moment the index would be at that separator, it will be equals to the lenght of the content (``csvContent.Lenght``). And jumping over that supposed separator will always let the index at the length + 1. Therefore not meeting the condition of the loop (`` i < csvContent.Lenght``), ending the function that now returns the new list of lists of strings, aka a 2D list of strings, aka ``List<List<string>>``.

BUT, in the specific case of the last cell of the csv being empty. After the 2nd-to-last cell is a coma indicating that there is indeed another cell after that one, but because it is the last cell and it is empty, after that coma there is nothing. Jumping over it to get to the first char of that empty cell would let the indext at exactly ``csvContent.Length``. Not meeting the condition of the loop, and skipping the addition of that last empty cell. Luckly, we have just seen that after the loop the index is always at the length + 1. So if the loop has ended but ``i`` is exactly at the legnth of the csv content, we know that we have just skipped and empty cell, and are able to add it to the last row before returning the table.

The array function (``ParseArray(csvContent)``) saves the content into a list and then turns it into an array. So, unless you absolutely need to have it in an 2D array, is better to call the first function.
