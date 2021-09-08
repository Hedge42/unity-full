using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Something

{
    // data
    private static Something _instance;
    private static List<Something> instanceList
    {
        get
        {
            if (_instanceList == null)
                _instanceList = new List<Something>();
            return _instanceList;
        }
    }
    private static List<Something> _instanceList;
    private static Something[] _instances;

    // accessors
    public static Something[] instances
    {
        get
        {
            if (_instances == null)
                _instances = new Something[0];
            return _instances;
        }
    }
    public static Something instance
    {
        get
        {
            if (_instance == null)
                _instance = new Something();
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }


    // public Fret[] data;

    public Something()
    {
        instanceList.Add(this);
    }

}