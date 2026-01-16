using OutSystems.ExternalLibraries.SDK;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Ardo.DiffJSONLists
{
    /// <summary>
    /// Version 1.0
    /// Helper extension for JSON. Allows you to manipulate and parse JSON using JSONPath.
    /// Uses the Newtonsoft.Json library (https://www.newtonsoft.com/json)
    /// </summary>
    [OSInterface(Description = "Version 0.1.0\r\n\r\nHelper extension for JSON. Allows you to manipulate and parse JSON using JSONPath.\r\n\r\nUses the Newtonsoft.Json library (https://www.newtonsoft.com/json)", IconResourceName = "DiffJSONLists.resources.logo.png", Name = "DiffJSONLists")]
    public interface IDiffJSONLists
    {


        /// <summary>
        /// Action to find differences between lists of JSON objects 
        /// </summary>
        [OSAction(Description = "Action to find differences between lists of JSON objects", IconResourceName = "DiffJSONLists.resources.logo.png", ReturnName = "Result")]
        public string JSON_DiffLists(
            [OSParameter(Description = "The Left JSON to be used")]
            string leftJsonIn, 
            [OSParameter(Description = "The Right JSON to be used")]
            string rightJsonIn,
            [OSParameter(Description = "The object key, in general is Id")]
            string keyFieldIn,
            [OSParameter(Description = "Return the unchanged values")]
            bool ReturnUnchangedIn,
            [OSParameter(Description = "List of ignored changed fields separated by a comma")]
            string IgnoreChangedFieldsIn,
            [OSParameter(Description = "JSON List of field renames (from → to) example: [ { \"From\": \"oldField\", \"To\": \"newField\" } ]")]
            string ReplaceFieldMatrixIn,
            [OSParameter(Description = "Used to add a space between words CreatedBy -> Created By")]
            bool HumanizeFieldsIn
            );

    }

}
