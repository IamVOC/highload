namespace SpaceBattle.Lib;
public class Vector
{
    private int[] coordinates;
    private int Size;
    public Vector(params int[] input)
    {
        int count = (int)input.Length;
        if (count == 0)
        {
            coordinates = new int[0];
            Size = 0;
        }
        else
        {
            coordinates = new int[count];
            Size = coordinates.Length;
            input.CopyTo(coordinates, 0);
        }
        Array.Copy(input, coordinates, coordinates.Length);
    }
    public override string ToString()
    {
        if (Size == 0)
        {
            return ("");
        }
        string? output = "Vector(";
        for (int i = 0; i < Size - 1; i++)
        {
            output += coordinates[i] + ",";
        }
        output += coordinates[Size - 1] + ")";
        return (output);
    }
    public int this[uint s]
    {
        get
        {
            if (s >= Size) throw new IndexOutOfRangeException();
            return (coordinates[s]);
        }
        set
        {
            if (s >= Size) throw new IndexOutOfRangeException();
            coordinates[s] = value;
        }

    }
    public static Vector operator +(Vector v1, Vector v2)
    {
        if (v1.Size != v2.Size)
        {
            throw new ArgumentException();
        }
        Vector temp = new Vector(v1.coordinates);
        for (int i = 0; i < v1.Size; i++)
        {
            temp.coordinates[i] = v1.coordinates[i] + v2.coordinates[i];
        }
        return (temp);
    }
    public static Vector operator -(Vector v1, Vector v2)
    {
        if (v1.Size != v2.Size)
        {
            throw new ArgumentException();
        }
        Vector temp = new Vector(v1.coordinates);
        for (int i = 0; i < v1.Size; i++)
        {
            temp.coordinates[i] = v1.coordinates[i] - v2.coordinates[i];
        }
        return (temp);
    }
    public static Vector operator *(int t, Vector v)
    {
        Vector temp = new Vector(v.coordinates);
        for (int i = 0; i < v.Size; i++)
        {
            temp.coordinates[i] = t * v.coordinates[i];
        }
        return (temp);
    }
    public static bool operator !=(Vector v1, Vector v2) => !(v1 == v2);
    public static bool operator <(Vector v1, Vector v2)
    {

        for (int i = 0; i < Math.Max(v1.Size, v2.Size); i++)
        {
            if (i == v1.Size)
            {
                return (true);
            }
            if (i == v2.Size)
            {
                return (false);
            }
            if (v1.coordinates[i] > v2.coordinates[i])
            {
                return (false);
            }
            if (v1.coordinates[i] < v2.coordinates[i])
            {
                return (true);
            }
        }
        return (false);
    }
    public static bool operator >(Vector v1, Vector v2)
    {

        for (int i = 0; i < Math.Max(v1.Size, v2.Size); i++)
        {
            if (i == v1.Size)
            {
                return (false);
            }
            if (i == v2.Size)
            {
                return (true);
            }
            if (v1.coordinates[i] < v2.coordinates[i])
            {
                return (false);
            }
            if (v1.coordinates[i] > v2.coordinates[i])
            {
                return (true);
            }
        }
        return (false);
    }
    public static bool operator ==(Vector v1, Vector v2)
    {
        if (v1.Size != v2.Size) return false;
        for (int i = 0; i < v1.Size; i++)
        {
            if (v1.coordinates[i] != v2.coordinates[i]) return false;
        }
        return true;
    }
    public override bool Equals(object? obj) => obj is Vector v && coordinates.SequenceEqual(v.coordinates);
    public override int GetHashCode()
    {
        return String.Join("", coordinates.Select(x => x.ToString())).GetHashCode();
    }
    public int[] getCoords()
    {
        return coordinates;
    }
}

