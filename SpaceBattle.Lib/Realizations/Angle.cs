namespace SpaceBattle.Lib;

public class Angle
{
    private int n, m;

    private static int gcd(int a, int b)
    {
        if (Math.Abs(b) == 0) 
            return Math.Abs(a);
        else 
            return gcd(Math.Abs(b), Math.Abs(a) % Math.Abs(b));
    }

    public Angle(int n, int m)
    {
        if (m == 0) throw new ArgumentException();
        if (n >= 0 && m < 0 || n <= 0 && m < 0) 
        {
            m *= -1;
            n *= -1;
        }
        this.n = n / gcd(n, m);
        this.m = m / gcd(n, m);
    }

    public static Angle operator + (Angle a, Angle b)
    {
        int tmpN = a.n * b.m + b.n * a.m;
        int tmpM = a.m * b.m;
        int d = gcd(tmpN, tmpM);
        return new Angle(tmpN / d, tmpM / d);
    } 

    public static bool operator ==(Angle a, Angle b) => a.n == b.n && a.m == b.m;

    public static bool operator !=(Angle a, Angle b) => !(a == b);

    public override bool Equals(object? obj) => obj is Angle a && n == a.n && m == a.m;

    public override int GetHashCode() => (this.n.ToString() + this.m.ToString()).GetHashCode();
}
