# trashx: file cleanup tool

**trashx** is a command-line utility designed to help you efficiently clean up your file system by deleting files with specific extensions in a given directory.

## Installation

To use **trashx**, install it on your system with the following steps:

### Install via .NET CLI

```sh
dotnet tool install --global trashx
```

Now, **trashx** is ready to use from the command line.

## Usage

**trashx** allows you to delete files based on their extensions. The basic syntax is:

```sh
trashx -e <extensions> <directory> [-r]
```

- **`-e <extensions>`**: Specify the file extensions to match, separated by commas (e.g., `"txt,mp4,zip"`).
- **`<directory>`**: The directory where Trashx should begin its search.
- **`-r` (optional)**: Enable recursive deletion to include subdirectories.

**trashx** lists matching files along with their sizes and asks for confirmation before deleting them.

## Example

To delete all `.txt` and `.pdf` files from the `C:\ExampleFolder` directory:

```sh
trashx -e txt,pdf "C:\ExampleFolder" -r
```

### Example Output

```sh
FILE                                                                                  SIZE
----                                                                                  ----
C:\ExampleFolder\1.txt                                                               45 kB
C:\ExampleFolder\2.txt                                                              185 kB
C:\ExampleFolder\3.txt                                                               36 kB
C:\ExampleFolder\SubFolder\1.pdf                                                    3.6 MB

proceed to delete files? [y / n]: y

🗑️ Files deleted successfully.
```

## License

**trashx** is open-source software licensed under the [MIT License](https://github.com/sarf01k/trashx/blob/main/LICENSE). You are free to use, modify, and distribute this tool according to the terms of the license.
