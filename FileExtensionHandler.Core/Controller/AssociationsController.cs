using FileExtensionHandler.Core.Model;

namespace FileExtensionHandler.Core.Controller
{
    public class AssociationsController
    {
        /// <summary>
        /// Creates a new association entry.
        /// </summary>
        /// <param name="node">The association's identifier.</param>
        /// <returns>A new association entry with its node.</returns>
        public static Association Create(string node)
        {
            return new Association { Node = node };
        }

        /// <summary>
        /// Copies an existing association entry.
        /// </summary>
        /// <param name="node">The association's new identifier.</param>
        /// <param name="association">An existing association to create a copy of.</param>
        /// <returns>A new association entry with the entries copied from an existing entry.</returns>
        public static Association CopyTo(string node, Association association)
        {
            Association associationCopy = SerializationController.Clone(association);
            associationCopy.Node = node;
            return associationCopy;
        }

        /// <summary>
        /// Copies an existing association entry asynchronously.
        /// </summary>
        /// <param name="node">The association's new identifier.</param>
        /// <param name="association">An existing association to create a copy of.</param>
        /// <returns>A new association entry with the entries copied from an existing entry.</returns>
        public static async Task<Association> CopyToAsync(string node, Association association, CancellationToken cancellationToken = default)
        {
            Association associationCopy = await SerializationController.CloneAsync(association, cancellationToken);
            associationCopy.Node = node;
            return associationCopy;
        }

        /// <summary>
        /// Saves the association information to the disk.
        /// </summary>
        /// <param name="association">The association to serialize.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        public static void SaveToJson(Association association, string associationsDir)
        {
            string filePath = $@"{associationsDir}\{association.Node}.json";
            SerializationController.SerializeToFile(association, filePath);
        }

        /// <summary>
        /// Saves the association information to the disk asynchronously.
        /// </summary>
        /// <param name="association">The association to serialize.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        public static async Task SaveToJsonAsync(Association association, string associationsDir, CancellationToken cancellationToken = default)
        {
            string filePath = $@"{associationsDir}\{association.Node}.json";
            await SerializationController.SerializeToFileAsync(association, filePath, cancellationToken);
        }

        /// <summary>
        /// Loads the association information.
        /// </summary>
        /// <param name="node">The association name of the "<![CDATA[<association name>]]>.json" file.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <returns>Desearialized association information.</returns>
        public static Association LoadFromJson(string node, string associationsDir)
        {
            string filePath = $@"{associationsDir}\{node}.json";
            Association association = SerializationController.DeserializeFile<Association>(filePath);
            association.Node = node;
            return association;
        }

        /// <summary>
        /// Loads the association information asynchronously.
        /// </summary>
        /// <param name="node">The association name of the "<![CDATA[<association name>]]>.json" file.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <returns>Desearialized association information.</returns>
        public static async Task<Association> LoadFromJsonAsync(string node, string associationsDir, CancellationToken cancellationToken = default)
        {
            string filePath = $@"{associationsDir}\{node}.json";
            Association association = await SerializationController.DeserializeFileAsync<Association>(filePath, cancellationToken);
            association.Node = node;
            return association;
        }

        /// <summary>
        /// Loads a list containing association information.
        /// </summary>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <returns>A list of desearialized association information entries.</returns>
        public static List<Association> GetAssociations(string associationsDir)
        {
            List<Association> associations = new();
            DirectoryInfo directoryInfo = new(associationsDir);
            foreach (FileInfo file in directoryInfo.GetFiles("*.json"))
            {
                string associationNode = Path.GetFileNameWithoutExtension(file.Name);
                Association associationData = LoadFromJson(associationNode, associationsDir);
                associations.Add(associationData);
            }
            return associations;
        }

        /// <summary>
        /// Loads a list containing association information asynchronously.
        /// </summary>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <returns>A list of desearialized association information entries.</returns>
        public static async Task<List<Association>> GetAssociationsAsync(string associationsDir, CancellationToken cancellationToken = default)
        {
            List<Association> associations = new();
            DirectoryInfo directoryInfo = new(associationsDir);
            foreach (FileInfo file in directoryInfo.GetFiles("*.json"))
            {
                string associationNode = Path.GetFileNameWithoutExtension(file.Name);
                Association associationData = await LoadFromJsonAsync(associationNode, associationsDir, cancellationToken);
                associations.Add(associationData);
            }
            return associations;
        }

        /// <summary>
        /// Loads a list containing associations for the selected file extension.
        /// </summary>
        /// <param name="fileExtension">The file extension struct with associations.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <returns>A list of associations.</returns>
        public static List<Association> GetAssociations(FileExtension fileExtension, string associationsDir)
        {
            List<Association> list = new();
            if (fileExtension.Associations.Length == 0)
            {
                // The file has no file extension and there's no JSON file to handle the case
                list.Add(new Association() { Node = null, Name = "Let Windows decide what to do with the file extension" });

                List<string> allAssociations = new();
                DirectoryInfo directoryInfo = new(associationsDir);
                foreach (FileInfo file in directoryInfo.GetFiles("*.json"))
                {
                    allAssociations.Add(Path.GetFileNameWithoutExtension(file.FullName));
                }
                fileExtension.Associations = allAssociations.ToArray();
            }

            foreach (string associationNode in fileExtension.Associations)
            {
                // Skip the current iteration if the association file doesn't exist
                string fileAssociationPath = associationsDir + $@"\{associationNode}.json";
                if (!File.Exists(fileAssociationPath)) continue;

                Association association = LoadFromJson(associationNode, associationsDir);
                list.Add(association);
            }
            return list;
        }

        /// <summary>
        /// Loads a list containing associations for the selected file extension asynchronously.
        /// </summary>
        /// <param name="fileExtension">The file extension struct with associations.</param>
        /// <param name="associationsDir">The directory containing associations.</param>
        /// <returns>A list of associations.</returns>
        public static async Task<List<Association>> GetAssociationsAsync(FileExtension fileExtension, string associationsDir, CancellationToken cancellationToken = default)
        {
            List<Association> list = new();
            if (fileExtension.Associations.Length == 0)
            {
                // The file has no file extension and there's no JSON file to handle the case
                list.Add(new Association() { Node = null, Name = "Let Windows decide what to do with the file extension" });

                List<string> allAssociations = new();
                DirectoryInfo directoryInfo = new(associationsDir);
                foreach (FileInfo file in directoryInfo.GetFiles("*.json"))
                {
                    allAssociations.Add(Path.GetFileNameWithoutExtension(file.FullName));
                }
                fileExtension.Associations = allAssociations.ToArray();
            }

            foreach (string associationNode in fileExtension.Associations)
            {
                // Skip the current iteration if the association file doesn't exist
                string fileAssociationPath = associationsDir + $@"\{associationNode}.json";
                if (!File.Exists(fileAssociationPath)) continue;

                Association association = await LoadFromJsonAsync(associationNode, associationsDir, cancellationToken);
                list.Add(association);
            }
            return list;
        }
    }
}
