using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using System.Text;


public static class CharBooleanExtensions
{
		
		public static bool isUpper(this char sc)
		{
			return (
				System.Char.IsUpper(sc));
			
		}
		public static bool isPunctuation(this char sc)
		{
			return (
				System.Char.IsPunctuation(sc));
			
		}
		
		public static bool isBrace(this char sc)
		
		{
			return (sc == '{' || sc == '}');
			
		}
		public static bool isBracket(this char sc)
		
		{
			return (sc == '[' || sc == ']');
			
		}
		public static bool isOperation(this string sc)
		{
			for (int i = 0 ; i < sc.Length ; i++)
			
				if (!sc[i].isOperation()) return false;
			
			return true;
		}
		public static bool isOperation(this char sc)
		
		{
			return (sc == '<' || sc == '>' || sc == '=' || sc == '!' || sc == '-' || sc == '+' || sc == '/' || sc == '*' || sc == '|' || sc == ':');
			
		}
		public static bool isNewLine(this char sc)
		
		{
			return (sc == '\n' || sc == '\r');
			
		}
		public static string asString(this string sc)
		{
			
			
			if (sc[0] == '\'') return sc;
			else if (sc[0] != '"') return "";
			
			StringBuilder sresult = new StringBuilder();
			bool inquotes = true;
			
			for (int i = 1 ; i < sc.Length ; i++)
			{
				if (sc[i] == '"') inquotes=false;
				else if (!inquotes) return "";
				else
					sresult.Append(sc[i]);
				
			}
			return sresult.ToString();
			
			
		}
		public static bool isString(this string sc)
		
		{
					
			return sc.asString() != null;
			
		}
		public static bool isStringLiteral(this string sc)
		
		{
			for (int i = 0 ; i < sc.Length ; i++)
			
				if (!(sc[i].isLetter() || sc[i].isDigit())) return false;
			
			return true;
			
		}
		public static float? asNumber(this string sc)
		{
			
			float iresult;
			bool b = float.TryParse (sc, out iresult);
			if (b) return iresult;
			return null;
			
		}
		public static bool isNumber(this string sc)
		
		{
					
			return sc.asNumber() != null;
			
		}
		public static bool isDigit(this char sc)
		
		{
			return (
			   System.Char.IsDigit(sc));
			
		}
		public static bool isLetter(this char sc)
		
		{
			return (
			   System.Char.IsLetter(sc));
			
		}
		public static bool isSymbol(this char sc)
		
		{ // this includes tabs... newlines...
			return (
			   System.Char.IsSymbol(sc));
			
		}
		public static bool isNotBlank(this char sc)
		{
			return ( !System.Char.IsWhiteSpace(sc) );
			
		}
		public static bool isWhiteSpace(this char sc)
		{ // this includes tabs... newlines...
			return System.Char.IsWhiteSpace(sc) ;
			
		}
	
}
public static class BinaryMaskExtensions
{
	
	
	public static int AddMask(this int i, int smal)
	{
		
		return i |= smal;
	}
	public static int RemoveMask(this int i, int smal)
	{
		return i &= ~smal;
	}
	
	
	public static bool IsMasked(this int smal, int mask)
	{
		// this is only tested on small marker integers
		return ((mask & smal) == smal); 
	
	}
	public static bool IsMasking(this int i, int smal)
	{
		// this is only tested on small marker integers
		return ((i & smal) == smal); 
	
	}
	public static bool Intersects(this int i, int smal){
		
		return (i & smal) != 0;
	}
}
public static class MathIsZeroExtensions
{
	private const float Epsilon = 1e-10f;

	public static bool IsZero(this float d)
	{
		return Mathf.Abs(d) < Epsilon;
	}
}

public static class GameObjectExtensions
{
/// <summary>
    /// Returns all monobehaviours (casted to T)
    /// </summary>
    /// <typeparam name="T">interface type</typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T[] GetInterfaces<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
        var mObjs = gObj.GetComponents<MonoBehaviour>();
 
        return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
    }
 
    /// <summary>
    /// Returns the first monobehaviour that is of the interface type (casted to T)
    /// </summary>
    /// <typeparam name="T">Interface type</typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T GetInterface<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
        return gObj.GetInterfaces<T>().FirstOrDefault();
    }
 
    /// <summary>
    /// Returns the first instance of the monobehaviour that is of the interface type T (casted to T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T GetInterfaceInChildren<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
        return gObj.GetInterfacesInChildren<T>().FirstOrDefault();
    }
 
    /// <summary>
    /// Gets all monobehaviours in children that implement the interface of type T (casted to T)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gObj"></param>
    /// <returns></returns>
    public static T[] GetInterfacesInChildren<T>(this GameObject gObj)
    {
        if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
 
        var mObjs = gObj.GetComponentsInChildren<MonoBehaviour>();
 
        return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
    }
}


public static class CountStringExtension {
	
	public static int Count(this string haystack, string needle){
		
		return ( haystack.Length - haystack.Replace(needle,String.Empty).Length ) / needle.Length;

	}
	
	public static int Count(this string haystack, char needle){
		
		return ( haystack.Length - haystack.Replace(""+needle,String.Empty).Length ) ;

	}
	
	
	
	
	
}
public static class IndexOfExtension {
	
	public static int IndexOf(this StringBuilder str, string other, int start, int end = -1)
	{
		if (str.Length < 1) return -1;
		
		int found = -1;
		int i = 0;
		int ix = 0;
		int success = other.Length;
		if (end < 0) end = str.Length;
		
		
		while (i < end )
		{
			
			if (str[i++] == other[ix++])
			{	
				if (ix >= success) 
				{
					found = i - 1;
					
					return found;
					
				}
				
				while (i < end && str[i++] == other[ix++])
				{
					
					if (ix >= success) 
					{
						found = i - 1;
						
						return found;
					}
				}
			}
			ix = 0; // failed
			
		}
		
		return found;
	}
	
	
	
}
public static class ContainingStringExtension {
	
	public static bool Contains(this string str, int start, char other, bool ignore_case = true)
	{
		
		if (ignore_case)
		{
			str = str.ToLower();
			other = Char.ToLower(other);
		}
		for (int i = start ; i < str.Length ; i++)
		{
			if (str[i] == other) return true;
		}
		
		return false;
	}
	public static bool Contains(this string str, char other, bool ignore_case = true)
	{
		
		if (ignore_case)
		{
			str = str.ToLower();
			other = Char.ToLower(other);
		}
		for (int i = 0 ; i < str.Length ; i++)
		{
			if (str[i] == other) return true;
		}
		
		return false;
	}
	
	public static bool Contains(this string str, char[] other, bool ignore_case = true)
	{
		int len = other.Length;
		if (ignore_case)
		{
			str = str.ToLower();
			
			for (int i = 0 ; i < len ; i++)
			if (str.Contains(Char.ToLower(other[i]), false)) return true;
		}
		else
		for (int i = 0 ; i < len ; i++)
		
			if (str.Contains(other[i], false)) return true;
		
		
		
		return false;
	}
	
	public static bool Contains(this string[] str, string other, bool ignore_case = true)
	{
		int len = str.Length;
		if (ignore_case)
		{
			other = other.ToLower();
			for (int i = 0 ; i < len ; i++)
				if (str[i].ToLower() == other) return true;
		}
		else
		for (int i = 0 ; i < len ; i++)
			if (str[i] == other) return true;
		
		return false;
	}
	
	
	public static bool Contains(this string str, string other, bool ignore_case = true)
	{
		
		if (ignore_case)
		{
			str = str.ToLower();
			other = other.ToLower();
			return str.IndexOf(other) >= 0;
		}
		return str.IndexOf(other) >= 0;
	}
	
	
	
}


public static class NameAndTagExtension {
	
	public static GameObject FindNameXTag(this GameObject go, string findByName, string findByTag)
	{
		foreach (GameObject t in GameObject.FindGameObjectsWithTag(findByTag))
		{
			if (t.name == findByName)
			{
				return t;
				
			}
			
		}
		return null;
	}
}