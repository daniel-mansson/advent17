using System.Collections;
using System.Collections.Generic;

public struct AxialCoord
{
	public int q;
	public int r;

	public AxialCoord(int q, int r)
	{
		this.q = q;
		this.r = r;
	}

	public static readonly AxialCoord UpRight = new AxialCoord(0, 1);
	public static readonly AxialCoord Right = new AxialCoord(1, 0);
	public static readonly AxialCoord DownRight = new AxialCoord(1, -1);
	public static readonly AxialCoord DownLeft = new AxialCoord(0, -1);
	public static readonly AxialCoord UpLeft = new AxialCoord(-1, 1);
	public static readonly AxialCoord Left = new AxialCoord(-1, 0);

	public static readonly AxialCoord[] Directions = new AxialCoord[]
		{
			UpLeft, UpRight, Left, Right, DownLeft, DownRight
		};

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}

		var other = (AxialCoord)obj;
		return other.q == q && other.r == r;
	}

	public override int GetHashCode()
	{
		return (15000 + r) * 30000 + 15000 + q;
	}

	public static explicit operator AxialCoord(CubeCoord cubeCoord)
	{
		return new AxialCoord(cubeCoord.x, cubeCoord.y);
	}

	public static bool operator ==(AxialCoord a, AxialCoord b)
	{
		return a.Equals(b);
	}

	public static bool operator !=(AxialCoord a, AxialCoord b)
	{
		return !a.Equals(b);
	}

	public static AxialCoord operator +(AxialCoord a, AxialCoord b)
	{
		return new AxialCoord(a.q + b.q, a.r + b.r);
	}

	public static int Distance(AxialCoord a, AxialCoord b)
	{
		return CubeCoord.Distance((CubeCoord)a, (CubeCoord)b);
	}
}