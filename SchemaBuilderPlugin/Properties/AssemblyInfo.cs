using System.Reflection;
using System.Runtime.InteropServices;
using Rhino.PlugIns;


// Plug-in Description Attributes - all of these are optional
// These will show in Rhino's option dialog, in the tab Plug-ins
[assembly: PlugInDescription(DescriptionType.Address, "3670 Woodland Park Ave N\n\nSeattle, WA 98103")]
[assembly: PlugInDescription(DescriptionType.Country, "Denmark")]
[assembly: PlugInDescription(DescriptionType.Email, "paul.poinet@kadk.dk")]
[assembly: PlugInDescription(DescriptionType.Phone, "(+45) 41701634")]
[assembly: PlugInDescription(DescriptionType.Fax, "(")]
[assembly: PlugInDescription(DescriptionType.Organization, "Robert McNeel & Associates")]
[assembly: PlugInDescription(DescriptionType.UpdateUrl, "http://www.rhino3d.com")]
[assembly: PlugInDescription(DescriptionType.WebSite, "http://www.rhino3d.com")]

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("SchemaBuilder")] // Plug-In title is extracted from this
[assembly: AssemblyDescription("Tree builder tool to assemble multiple objects, properties and schemas")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("PaulPoinet CITA InnoChain")]
[assembly: AssemblyProduct("SchemaBuilder")]
[assembly: AssemblyCopyright("Copyright © 2017, PaulPoinet CITA InnoChain")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Plug-ins that are to be distributed by the Rhino Installer Engine
// need this piece of information.
[assembly: AssemblyInformationalVersion("2")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("3824F5B0-526D-4CE2-8A22-46D8B828608A")] // This will also be the Guid of the Rhino plug-in

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
