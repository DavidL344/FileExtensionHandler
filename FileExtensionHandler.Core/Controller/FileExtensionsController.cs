using FileExtensionHandler.Core.Model;

namespace FileExtensionHandler.Core.Controller
{
    public class FileExtensionsController
    {
        /// <summary>
        /// Creates a new file extension entry.
        /// </summary>
        /// <param name="node">The file extension's identifier</param>
        /// <returns>A new file extension entry with its node.</returns>
        public static FileExtension Create(string node)
        {
            return new FileExtension { Node = node };
        }

        /// <summary>
        /// Copies an existing file extension entry.
        /// </summary>
        /// <param name="node">The file extension's new identifier.</param>
        /// <param name="fileExtension">An existing file extension to create a copy of.</param>
        /// <returns>A new file extension entry with the entries copied from an existing entry.</returns>
        public static FileExtension CopyTo(string node, FileExtension fileExtension)
        {
            FileExtension associationCopy = SerializationController.Clone(fileExtension);
            associationCopy.Node = node;
            return associationCopy;
        }

        /// <summary>
        /// Copies an existing file extension entry asynchronously.
        /// </summary>
        /// <param name="node">The file extension's new identifier.</param>
        /// <param name="fileExtension">An existing file extension to create a copy of.</param>
        /// <returns>A new file extension entry with the entries copied from an existing entry.</returns>
        public static async Task<FileExtension> CopyToAsync(string node, FileExtension fileExtension, CancellationToken cancellationToken = default)
        {
            FileExtension fileExtensionCopy = await SerializationController.CloneAsync(fileExtension, cancellationToken);
            fileExtensionCopy.Node = node;
            return fileExtensionCopy;
        }

        /// <summary>
        /// Saves the association information to the disk.
        /// </summary>
        /// <param name="fileExtension">The file extension information to serialize.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        public static void SaveToJson(FileExtension fileExtension, string fileExtensionsDir)
        {
            string filePath = $@"{fileExtensionsDir}\{fileExtension.Node}.json";
            SerializationController.SerializeToFile(fileExtension, filePath);
        }

        /// <summary>
        /// Saves the association information to the disk asynchronously.
        /// </summary>
        /// <param name="fileExtension">The file extension information to serialize.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        public static async Task SaveToJsonAsync(FileExtension fileExtension, string fileExtensionsDir, CancellationToken cancellationToken = default)
        {
            string filePath = $@"{fileExtensionsDir}\{fileExtension.Node}.json";
            await SerializationController.SerializeToFileAsync(fileExtension, filePath, cancellationToken);
        }

        /// <summary>
        /// Loads the file extension information.
        /// </summary>
        /// <param name="node">The file extension name of the "<![CDATA[<file extension>]]>.json" file.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <returns>Desearialized file extension information.</returns>
        public static FileExtension LoadFromJson(string node, string fileExtensionsDir)
        {
            string filePath = $@"{fileExtensionsDir}\{node}.json";
            FileExtension fileExtension = SerializationController.DeserializeFile<FileExtension>(filePath);
            fileExtension.Node = node;
            return fileExtension;
        }

        /// <summary>
        /// Loads the file extension information asynchronously.
        /// </summary>
        /// <param name="node">The file extension name of the "<![CDATA[<file extension>]]>.json" file.</param>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <returns>Desearialized file extension information.</returns>
        public static async Task<FileExtension> LoadFromJsonAsync(string node, string fileExtensionsDir, CancellationToken cancellationToken = default)
        {
            string filePath = $@"{fileExtensionsDir}\{node}.json";
            FileExtension fileExtension = await SerializationController.DeserializeFileAsync<FileExtension>(filePath, cancellationToken);
            fileExtension.Node = node;
            return fileExtension;
        }

        /// <summary>
        /// Loads a list containing file extension information.
        /// </summary>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <returns>A list of desearialized file extension information entries.</returns>
        public static List<FileExtension> GetFileExtensions(string fileExtensionsDir)
        {
            List<FileExtension> fileExtensions = new();
            DirectoryInfo directoryInfo = new(fileExtensionsDir);
            foreach (FileInfo file in directoryInfo.GetFiles("*.json"))
            {
                string fileExtensionNode = Path.GetFileNameWithoutExtension(file.Name);
                FileExtension fileExtensionData = LoadFromJson(fileExtensionNode, fileExtensionsDir);
                fileExtensions.Add(fileExtensionData);
            }
            return fileExtensions;
        }

        /// <summary>
        /// Loads a list containing file extension information asynchronously.
        /// </summary>
        /// <param name="fileExtensionsDir">The directory containing file extension information.</param>
        /// <returns>A list of desearialized file extension information entries.</returns>
        public static async Task<List<FileExtension>> GetFileExtensionsAsync(string fileExtensionsDir, CancellationToken cancellationToken = default)
        {
            List<FileExtension> fileExtensions = new();
            DirectoryInfo directoryInfo = new(fileExtensionsDir);
            foreach (FileInfo file in directoryInfo.GetFiles("*.json"))
            {
                string fileExtensionNode = Path.GetFileNameWithoutExtension(file.Name);
                FileExtension associationData = await LoadFromJsonAsync(fileExtensionNode, fileExtensionsDir, cancellationToken);
                fileExtensions.Add(associationData);
            }
            return fileExtensions;
        }
    }
}
