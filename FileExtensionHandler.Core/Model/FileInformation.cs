namespace FileExtensionHandler.Core.Model
{
    public struct FileInformation
    {
        public string AppProtocol { get; internal set; }
        public List<Association> Associations { get; internal set; }
        public FileExtension FileExtension { get; internal set; }
        public string Location { get; internal set; }
        public string[] Protocols { get; internal set; }

        public string Name => Path.GetFileName(LocationNoParameters);
        public string LocationNoParameters => Streamed ? Location.Split('?')[0] : Location;
        public bool IsLocationValid => LocationNoParameters.IndexOfAny(Path.GetInvalidPathChars()) == -1 && LocationNoParameters.IndexOfAny(Path.GetInvalidFileNameChars()) == -1;
        public bool Streamed
        {
            get
            {
                // Do not count "fexth" and "file" protocols as "protocols streaming files" 
                int minimumProtocolsToQualify = 1;
                if (Protocols.Contains("file:///") || Protocols.Contains("file://")) minimumProtocolsToQualify++;
                if (Protocols.Contains($"{AppProtocol}://") || Protocols.Contains($"{AppProtocol}:")) minimumProtocolsToQualify++;
                return Protocols.Length >= minimumProtocolsToQualify;
            }
        }
        public string Type => FileExtension.Name;
        public bool AppProtocolUsed => Protocols.Contains($"{AppProtocol.ToLower()}://") || Protocols.Contains($"{AppProtocol.ToLower()}:");

        public Association? DefaultAssociation
        {
            get
            {
                return DefaultAssociationIndex != -1 ? Associations[DefaultAssociationIndex] : null;
            }
        }

        public int DefaultAssociationIndex
        {
            get
            {
                string? defaultAssociationName = FileExtension.DefaultAssociation;
                return Associations.FindIndex(x => x.Node != null && defaultAssociationName != null && x.Node.Equals(defaultAssociationName));
            }
        }
    }
}
