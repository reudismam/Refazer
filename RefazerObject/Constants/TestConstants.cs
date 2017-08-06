namespace RefazerObject.Constants
{
    public class TestConstants
    {
        /// <summary>
        /// Defines the path to the file that logs informations about generated programs.
        /// </summary>
        public const string LogPathFile = "ProgStatus.xlsx";

        /// <summary>
        /// Defines the path to the folder that contains the metadata
        /// </summary>
        public const string MetadataFolder = "cprose";

        /// <summary>
        /// Defines the path to the diff file.
        /// </summary>
        public const string DiffFolder = @"metadata\diff.df";

        /// <summary>
        /// Defines the path to the before file to perform the diff
        /// </summary>
        public const string BeforeFile = @"metadata_tool\B";

        /// <summary>
        /// Defines the path to the after file to perform the diff
        /// </summary>
        public const string AfterFile = @"\metadata_tool\A";

        /// <summary>
        /// Defines the path to the transformed locations
        /// </summary>
        public const string TransformedLocations = @"\metadata\transformed_locations.json";

        /// <summary>
        /// Defines the path to the transformed locations
        /// </summary>
        public const string TransformedLocationsTool = @"\metadata\transformed_locations.json";

        /// <summary>
        /// Defines the path to all transformed locations
        /// </summary>
        public const string TransformedLocationsAll = @"\metadata\transformed_locationsAll";

        /// <summary>
        /// Defines the before and after locations
        /// </summary>
        public const string BeforeAfterLocationsTool = @"\metadata_tool\before_after_locations";

        /// <summary>
        /// Defines the before and after locations for the tool
        /// </summary>
        public const string BeforeAfterLocations = @"\metadata\before_after_locations";

        /// <summary>
        /// Defines the before and after locations for all locations
        /// </summary>
        public const string BeforeAfterLocationsAll = @"\metadata_tool\before_after_locationsAll";
    }
}
