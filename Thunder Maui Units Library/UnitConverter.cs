/*
 * Thunder Unit conversion library
 * (C)Copyright 2005/2006 Robert Harwood <robharwood@runbox.com>
 *
 * Please see included license.txt file for information on redistribution and usage.
 */
using System.Diagnostics;
using System.Xml;

namespace Thor.Units
{
	/// <summary>
	/// Unit conversion class, contains methods for loading a unit file
	/// and converting units.
	/// </summary>
	public class UnitConverter : IUnitConverter
	{
		#region Events

		/// <summary>
		/// Called when an error occurs in the unit converter.
		/// </summary>
		public event UnitEventHandler? OnError;

		#endregion

		#region Members

		public const double         UNITFILE_VERSION            =   1.0;
		public const double         FAILSAFE_VALUE              =   System.Double.NaN;
		private SymbolTable         m_SymbolTable;
		private UnitTable           m_Units;
		private GroupTable          m_UnitGroups;
		private XmlDocument         m_UnitsFile;
		private string              m_CurUnitFileName;
		private double              m_CurUnitsFileVersion;

		#endregion

		#region Construction

		/// <summary>
		/// Constructor, sets up the unit converter.
		/// </summary>
		public UnitConverter()
		{
			// Set up the tables we need
			m_SymbolTable = new SymbolTable();
			m_Units       = new UnitTable();
			m_UnitGroups  = new GroupTable();

			// Create an Xml document to hold the units file in.
			m_UnitsFile = new XmlDocument();
			m_CurUnitsFileVersion = 0.0;

			m_CurUnitFileName = "";

			InitTables();
		}

		/// <summary>
		/// Initialization.
		/// </summary>
		public void InitTables()
		{
			// Clear everything out.
			this.m_SymbolTable.Clear();
			this.m_UnitGroups.Clear();
			this.m_Units.Clear();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Units.
		/// </summary>
		public UnitTable Units
		{
			get
			{
				return m_Units;
			}

			set
			{
				m_Units = value;
			}
		}

		/// <summary>
		/// Groups.
		/// </summary>
		public GroupTable Groups
		{
			get
			{
				return m_UnitGroups;
			}

			set
			{
				m_UnitGroups = value;
			}
		}

		#endregion

		#region XML Unit File Methods

		/// <summary>
		/// Sends a specially formed error message regarding the loading of a unit file.
		/// </summary>
		/// <param name="message">Message to send.</param>
		/// <param name="filePath">Path of the unit file.</param>
		/// <param name="args">Optional format arguments.</param>
		private void SendUnitFileWarning(string message, string filePath, object[]? args)
		{
			string error = "Error in units file '"+ filePath + "' - ";
			error += message;

			if (args != null)
			{
				error = String.Format(error, args);
			}

			OnError?.Invoke(this, new UnitEventArgs(error));
		}

		/// <summary>
		/// Given the path to a units file, loads the file.
		/// </summary>
		/// <param name="filePath">Path to a units file.</param>
		/// <returns>Unit result code.</returns>
		public UnitResult LoadUnitsFile(string filePath)
		{
			string error = "";

			// Make sure the units file exists.
			if (!File.Exists(filePath))
			{
				return UnitResult.FileNotFound;
			}

			try
			{
				// Attempt to load the unit file...
				m_UnitsFile.Load(filePath);
			}
			catch (XmlException ex)
			{
				// Create the exception string.
				error = "Error parsing '{0}' at line {1}, position {2}.";
				error = String.Format(error, filePath, ex.LineNumber, ex.LinePosition);

				// Throw the exception.
				throw new UnitFileException(error, ex.Message);
			}

			// Reinitialize the data tables.
			InitTables();

			m_CurUnitFileName = "";
			m_CurUnitsFileVersion = 0.0;

			// Get a reference to a list of the XML data.
			XmlNodeList xmlData = m_UnitsFile.GetElementsByTagName("*");

			// No XML nodes? This is wrong.
			if (xmlData.Count == 0)
			{
				error = "Error parsing units file '{0}' - file contains no data.";
				error = String.Format(error, filePath);
				throw new UnitFileException(error);
			}

			// Get the root node.
			System.Xml.XmlNode? root = xmlData[0];

			// Does the file not start with a "UnitFile" node? We have problems.
			if (root.Name.ToLower() != "unitfile")
			{
				error = "Error parsing units file '{0}' - the file appears corrupt or incomplete.";
				error = String.Format(error, filePath);
				throw new UnitFileException(error);
			}

			// Store off the name of the units file if there is one.
			if (root.Attributes["name"] != null)
			{
				m_CurUnitFileName = root.Attributes["name"].Value;
			}
			else
			{
				// Units file has no internal name set on it.
				SendUnitFileWarning("file has no internal units name - using default.", filePath, null);
				m_CurUnitFileName = "Units";
			}

			// Check units file version.
			if (root.Attributes["version"] != null)
			{
				try
				{
					m_CurUnitsFileVersion = Convert.ToDouble(root.Attributes["version"].Value);
				}
				catch
				{
					m_CurUnitsFileVersion = 0.0;
				}

				if (m_CurUnitsFileVersion == 0.0)
				{
					// File version is 0.0, probably failed to convert to a double.
					error = "Error parsing '{0}' - file has no valid version number.";
					error = String.Format(error, filePath);
					throw new UnitFileException(error);
				}

				if (m_CurUnitsFileVersion > UNITFILE_VERSION)
				{
					// File version is greater than the maximum we support.
					error = "Error parsing '{0}' - file version indicates it is made for a newer version of the unit conversion library.";
					error = String.Format(error, filePath);
					throw new UnitFileException(error);
				}
			}
			else
			{
				// No version information was found at all.
				error = "Error parsing '{0}' - file has no version number specified.";
				error = String.Format(error, filePath);
				throw new UnitFileException(error);
			}

			int i = 0;

			// Parse all the unit groups and add them.
			for (i = 0; i < root.ChildNodes.Count; i++)
			{
				XmlNode groupnode = root.ChildNodes[i];

				// Ignore comments.
				if (groupnode.Name.ToLower() == "#comment")
				{
					continue;
				}

				if (groupnode.Name.ToLower() != "unitgroup")
				{
					SendUnitFileWarning("bad tag found while parsing groups (tag was '{0}'), tag ignored.", filePath, new object[] { groupnode.Name });
				}
				else
				{
					ParseGroupXMLNode(filePath, groupnode);
				}
			}

			// We were successful.
			return UnitResult.NoError;
		}

		/// <summary>
		/// Parses a group node in the unit XML document.
		/// </summary>
		/// <param name="groupnode">The XML node to parse.</param>
		/// <returns>A unit result value.</returns>
		private UnitResult ParseGroupXMLNode(string filePath, XmlNode groupnode)
		{
			int i = 0;

			if (groupnode.Attributes == null)
			{
				SendUnitFileWarning("Group not an element or does not have attributes, ignoring group.", filePath, null);
				return UnitResult.GenericError;
			}

			// Check the group has a name.
			if (groupnode.Attributes["name"] == null)
			{
				SendUnitFileWarning("Found a group with no name, ignoring group.", filePath, null);
				return UnitResult.GenericError;
			}

			// Create the group.
			UnitResult res = CreateNewGroup(groupnode.Attributes["name"].Value);

			// Make sure the group was created.
			if (res != UnitResult.NoError)
			{
				SendUnitFileWarning("Failed to create group entry, skipping group.", filePath, null);
				return UnitResult.GenericError;
			}

			// Get a reference to the group we just created.
			UnitGroup group = this.m_UnitGroups[groupnode.Attributes["name"].Value];

			// Parse all the units.
			for (i = 0; i < groupnode.ChildNodes.Count; i++)
			{
				// Get the node reference for the current unit.
				XmlNode? unitnode = groupnode.ChildNodes[i];

				if (unitnode == null)
				{
					continue;
				}

				// Ignore comments.
				if (unitnode.Name.ToLower() == "#comment")
				{
					continue;
				}

				if (unitnode.Name.ToLower() != "unit")
				{
					SendUnitFileWarning("Bad tag found while parsing units of group '{0}' (tag was '{1}'), tag ignored.", filePath, new object[] { group.Name, unitnode.Name });
				}
				else
				{
					// Parse out the unit
					res = ParseUnitXMLNode(filePath, group, unitnode);
				}
			}

			// Completed successfully.
			return UnitResult.NoError;
		}

		/// <summary>
		/// Parses an XML node that represents a unit.
		/// </summary>
		/// <param name="group">Name of the group this unit is in.</param>
		/// <param name="unitnode">The node containing the unit information.</param>
		/// <returns>A unit result value.</returns>
		private UnitResult ParseUnitXMLNode(string filePath, UnitGroup group, XmlNode unitnode)
		{
			int i = 0;

			UnitEntry unit = new UnitEntry();

			// Make sure the unit has a name.
			if (unitnode.Attributes["name"] == null)
			{
				SendUnitFileWarning("found a unit in group '{0}' with no name, ignored.", filePath, new object[] { group.Name });
				return UnitResult.GenericError;
			}

			// Store off the name.
			unit.Name = unitnode.Attributes["name"].Value;
			unit.DefaultSymbol = unit.Name.ToLower();

			// Don't allow duplicate units.
			if (GetUnitByName(unit.Name) != null)
			{
				SendUnitFileWarning("duplicate unit with name '{0}' was found and ignored.", filePath, new object[] { unit.Name });
				return UnitResult.UnitExists;
			}

			// Get every unit property.
			for (i = 0; i < unitnode.ChildNodes.Count; i++)
			{
				XmlNode unitprop = unitnode.ChildNodes[i];

				//Ignore comments.
				if (unitprop.Name.ToLower() == "#comment")
					continue;

				try
				{
					if (unitprop.Name.ToLower() == "multiply")
					{
						double x;
						if (ParseNumberString(unitprop.InnerText, out x) != UnitResult.NoError)
						{
							throw new System.Exception();
						}

						unit.Multiplier = x;

						//unit.m_Multiplier = Convert.ToDouble(unitprop.InnerText);
					}
					else
					{
						if (unitprop.Name.ToLower() == "add")
						{
							unit.Adder = Convert.ToDouble(unitprop.InnerText);
						}
						else
						{
							if (unitprop.Name.ToLower() == "preadd")
							{
								unit.PreAdder = Convert.ToDouble(unitprop.InnerText);
							}
						}
					}
				}
				catch
				{
					SendUnitFileWarning("unit '{0}' has invalid '{1}' value. Unit skipped.", filePath, new object[] { unit.Name, unitprop.Name });
					return UnitResult.GenericError;
				}

				// Parse the symbol properties.
				if (unitprop.Name.ToLower() == "symbol")
				{
					//Put the value into the symbol table
					if ((unitprop.InnerText != "") && (unitprop.InnerText != null))
					{
						if (this.m_SymbolTable[unitprop.InnerText] != null)
						{
							SendUnitFileWarning("while parsing unit '{0}' - a duplicate symbol was found and ignored ({1}).", filePath, new object[] { unit.Name, unitprop.InnerText });
						}
						else
						{
							this.m_SymbolTable[unitprop.InnerText] = unit;

							// Is this unit the default unit?
							if (unitprop.Attributes["default"] != null)
							{
								unit.DefaultSymbol = unitprop.InnerText;
							}
						}
					}
					else
					{
						SendUnitFileWarning("unit '{0}' has an invalid symbol specified, symbol skipped.", filePath, new object[] { unit.Name });
					}
				}
			}

			// Add the unit to the unit table.
			m_Units[unit.Name] = unit;

			// Add the unit to the group
			AddUnitToGroup(unit.Name, group.Name);

			// All done!
			return UnitResult.NoError;
		}

		#endregion

		#region Unit Related Methods

		/// <summary>
		/// Given the full name of the unit, returns the unit entry.
		/// </summary>
		/// <param name="unitName">Name of the unit.</param>
		/// <returns>Reference to the unit entry, or null if not found.</returns>
		public UnitEntry? GetUnitByName(string unitName)
		{
			return m_Units[unitName];
		}

		/// <summary>
		/// Given a unit symbol, gets the unit entry.
		/// </summary>
		/// <param name="unitSymbol">Symbol of the unit.</param>
		/// <returns>Reference to the unit entry, or null if symbol does not exist.</returns>
		public UnitEntry? GetUnitBySymbol(string unitSymbol)
		{
			// First check to see if they used the actual name of a unit then look at the symbol table.
			if (this.m_Units[unitSymbol] != null)
			{
				return m_Units[unitSymbol];
			}
			else
			{
				return m_SymbolTable[unitSymbol];
			}
		}

		#endregion

		#region Group Related Methods

		/// <summary>
		/// Gets a value that determines whether the given units are compatible or not.
		/// </summary>
		/// <param name="unitSymbol1">Symbol for the first unit.</param>
		/// <param name="unitSymbol2">Symbol for the second unit.</param>
		/// <returns>True if units are compatible, else false.</returns>
		public bool CompatibleUnits(string unitSymbol1, string unitSymbol2)
		{
			IUnitEntry u1 = GetUnitBySymbol(unitSymbol1);
			IUnitEntry u2 = GetUnitBySymbol(unitSymbol2);

			if (u1 == null || u2 == null)
			{
				return false;
			}

			return GetUnitGroup(u1.Name) == GetUnitGroup(u2.Name);
		}

		/// <summary>
		/// Returns a list of all the units in a given group.
		/// </summary>
		/// <param name="groupName">Name of group to extract names from.</param>
		public string[] GetListOfUnitsInGroup(string groupName)
		{
			return m_UnitGroups[groupName].Units.GetAllUnitNames();
		}

		/// <summary>
		/// Creates a new unit group and adds it to the group table.
		/// </summary>
		/// <param name="groupName">Name of the new group.</param>
		/// <returns>Unit result value.</returns>
		private UnitResult CreateNewGroup(string groupName)
		{
			// Create the new group
			UnitGroup newgroup = new UnitGroup();
			newgroup.Name = groupName;

			// Add it to the group table
			m_UnitGroups[groupName] = newgroup;

			return UnitResult.NoError;
		}

		/// <summary>
		/// Adds the named unit to the specified group.
		/// </summary>
		/// <param name="unitName">Name of the unit.</param>
		/// <param name="groupName">Name of the group to add the unit to.</param>
		/// <returns>Unit result value.</returns>
		private UnitResult AddUnitToGroup(string unitName, string groupName)
		{
			UnitEntry unit = this.m_Units[unitName];
			UnitGroup group = this.m_UnitGroups[groupName];

			// Make sure the unit exists.
			if (unit == null)
			{
				return UnitResult.UnitNotFound;
			}

			// Make sure the group exists.
			if (group == null)
			{
				return UnitResult.GroupNotFound;
			}

			// Add the unit.
			group.AddUnit(unit);

			return UnitResult.NoError;
		}

		/// <summary>
		/// Given the name of a unit, searches for the unit group it belongs to.
		/// </summary>
		/// <param name="unitName">Name of the unit.</param>
		/// <returns>The group the unit is in, or null if the unit is not valid.</returns>
		private UnitGroup GetUnitGroup(string unitName)
		{
			// Does the unit even exist?
			if (this.m_Units[unitName] == null)
			{
				return null;
			}

			// Iterate through every group
			UnitGroup[] groups = this.m_UnitGroups.GetAllGroups();
			foreach (UnitGroup group in groups)
			{
				if (group.IsInGroup(unitName))
				{
					return group;
				}
			}

			// Should never happen.
			Debug.Fail("Unit error", "A unit that does not belong to any group has been detected in GetUnitGroup() - the unit was '" + unitName + "'.");
			return null;
		}

		#endregion

		#region Conversion Methods

		/// <summary>
		/// Given a value and the current unit, converts the value back to the standard.
		/// </summary>
		/// <param name="val">Value to convert.</param>
		/// <param name="unitfrom">Name of the current units the value is in.</param>
		/// <param name="output">Variable to hold the converted value.</param>
		/// <returns>A unit result value.</returns>
		public UnitResult ConvertToStandard(double val, string unitfrom, out double output)
		{
			double x = val;

			// Default to the fail safe value.
			output = FAILSAFE_VALUE;

			IUnitEntry unit_from = GetUnitBySymbol(unitfrom);

			// Make sure both units are real units.
			if (unit_from == null)
				return UnitResult.BadUnit;

			try
			{
				// Convert the value back to the standard
				x = x + unit_from.PreAdder;
				if (unit_from.Multiplier > 0.0)
					x = x * unit_from.Multiplier;
				x = x + unit_from.Adder;

				output = x;
			}
			catch
			{
				// Probably overflowed or something.
				return UnitResult.BadValue;
			}
			return UnitResult.NoError;
		}

		/// <summary>
		/// Performs a unit conversion between two units, given a value to convert.
		/// </summary>
		/// <param name="val">The value to convert.</param>
		/// <param name="unitfrom">The name of the unit the value is currently in.</param>
		/// <param name="unitto">The name of the unit that the value is to be converted to.</param>
		/// <param name="output">The converted value.</param>
		/// <returns>Unit result value.</returns>
		public UnitResult ConvertUnits(double val, string unitfrom, string unitto, out double output)
		{
			double x = val;

			// Default to the fail safe value.
			output = FAILSAFE_VALUE;

			IUnitEntry unit_from = GetUnitBySymbol(unitfrom);
			IUnitEntry unit_to = GetUnitBySymbol(unitto);

			// Make sure both units are real units.
			if ((unit_from == null) || (unit_to == null))
			{
				return UnitResult.BadUnit;
			}

			// Make sure the units are of the same group
			if (!this.CompatibleUnits(unit_from.Name, unit_to.Name))
			{
				return UnitResult.UnitMismatch;
			}

			UnitResult conv_res;
			conv_res = ConvertToStandard(x, unit_from.Name, out x);
			if (conv_res != UnitResult.NoError)
			{
				return conv_res;
			}

			conv_res = ConvertFromStandard(x, unit_to.Name, out x);
			if (conv_res != UnitResult.NoError)
			{
				return conv_res;
			}

			output = x;

			return UnitResult.NoError;
		}

		/// <summary>
		/// Performs a unit conversion from the standard value into the specified unit.
		/// </summary>
		/// <param name="val">The value to convert.</param>
		/// <param name="unitto">The name of the unit that the value is to be converted to.</param>
		/// <param name="output">The converted value.</param>
		/// <returns>Unit result value.</returns>
		public UnitResult ConvertFromStandard(double val, string unitto, out double output)
		{
			double x = val;

			// Default to the fail safe value.
			output = FAILSAFE_VALUE;

			IUnitEntry? unit_to = GetUnitBySymbol(unitto);

			// Make sure both units are real units.
			if (unit_to == null)
			{
				return UnitResult.BadUnit;
			}

			try
			{
				// Convert to the new unit from the standard
				x = x - unit_to.PreAdder;
				if (unit_to.Multiplier > 0.0)
				{
					x = x * Math.Pow(unit_to.Multiplier, -1);
				}
				x = x - unit_to.Adder;

				output = x;
			}
			catch
			{
				// Probably overflowed or something.
				return UnitResult.BadValue;
			}

			return UnitResult.NoError;
		}

		#endregion

		#region Parsing Routines

		/// <summary>
		/// Parses a number string with operators.
		/// </summary>
		/// <param name="input">String containing numbers and operators.</param>
		/// <param name="val">Output value.</param>
		private UnitResult ParseNumberString(string input, out double val)
		{
			// Default value.
			val = 0.0;

			// Split the numbers on the ^ operator.
			string[] numbers;
			numbers = input.Split(new char[] { '^' });

			if (numbers.Length == 1)
			{
				// Only one value, so there was no ^ operator present.
				// so just return the one number.
				try
				{
					val = Convert.ToDouble(numbers[0]);
				}
				catch
				{
					return UnitResult.BadValue;
				}
			}
			else
			{
				// There is a ^ operator, so try to use it.
				try
				{
					val = Convert.ToDouble(numbers[0]);
					val = Math.Pow(val, Convert.ToDouble(numbers[1]));
				}
				catch
				{
					return UnitResult.BadValue;
				}
			}

			return UnitResult.NoError;
		}


		/// <summary>
		/// Given a string in the format "[value] [unit]", splits and returns the parts.
		/// </summary>
		/// <param name="input">Input string in the format "[value] [unit]" to be parsed.</param>
		/// <param name="val">Output variable that will hold the value.</param>
		/// <param name="unit">Output variable that will hold the unit.</param>
		/// <returns>Unit result code.</returns>
		public UnitResult ParseUnitString(string input, out double val, out string unit)
		{
			// Defaults.
			val = 0.0;
			unit = "";

			if (input == "")
			{
				return UnitResult.NoError;
			}

			int i = 0;

			string s1 = "";
			string s2 = "";

			// Look for the first letter or punctuation character.
			for (i = 0; i < input.Length; i++)
			{
				if (Char.IsLetter(input, i))// || Char.IsPunctuation(input, i))
				{
					break;
				}
			}

			s1 = input.Substring(0, i);
			s1 = s1.Trim();
			s2 = input.Substring(i);
			s2 = s2.Trim();

			// No value? default to 0.
			if (s1 == "")
			{
				s1 = "0";
			}

			try
			{
				val = Convert.ToDouble(s1);
			}
			catch
			{
				return UnitResult.BadValue;
			}

			if (this.GetUnitBySymbol(s2) == null)
			{
				return UnitResult.BadUnit;
			}

			unit = s2;

			return UnitResult.NoError;
		}

		#endregion

		#region Data String

		/// <summary>
		/// Creates a new data string, used as a bridge to the user interface.
		/// </summary>
		/// <returns>The newly created data string.</returns>
		public DataString CreateDataString()
		{
			DataString ds = new DataString(this, "");
			return ds;
		}

		public DataString CreateDataString(string unitSymbol)
		{
			DataString ds = new DataString(this, unitSymbol);
			return ds;
		}

		#endregion

	} // End class.
} // End namespace.
