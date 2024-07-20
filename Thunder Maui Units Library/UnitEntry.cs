namespace Thor.Units;

/// <summary>
/// Represents a single unit loaded from the units file.
/// </summary>
public class UnitEntry : IUnitEntry
{
	#region Members

	private string					m_Name;
	private string					m_DefaultSymbol;
	private double					m_PreAdder;
	private double					m_Adder;
	private double					m_Multiplier;

	#endregion

	#region Construction

	/// <summary>
	/// Constructor.
	/// </summary>
	public UnitEntry()
	{
		m_Name          = "";
		m_DefaultSymbol = "";
		m_PreAdder      = 0.0;
		m_Adder         = 0.0;
		m_Multiplier    = 0.0;
	}

	#endregion

	#region Properties

	public string Name
	{
		get
		{
			return m_Name;
		}

		set
		{
			m_Name = value;
		}
	}

	public string DefaultSymbol
	{
		get
		{
			return m_DefaultSymbol;
		}

		set
		{
			m_DefaultSymbol = value;
		}
	}

	public double PreAdder
	{
		get
		{
			return m_PreAdder;
		}

		set
		{
			m_PreAdder = value;
		}
	}

	public double Adder
	{
		get
		{
			return m_Adder;
		}

		set
		{
			m_Adder = value;
		}
	}

	public double Multiplier
	{
		get
		{
			return m_Multiplier;
		}

		set
		{
			m_Multiplier = value;
		}
	}

	#endregion

} // End class.