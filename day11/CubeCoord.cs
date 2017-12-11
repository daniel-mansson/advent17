using System;
using System.Collections;
using System.Collections.Generic;

public struct CubeCoord
{
	public int x;
	public int y;
	public int z;

	public CubeCoord(int x, int y, int z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public static explicit operator CubeCoord(AxialCoord axialCoord)
	{
		return new CubeCoord(axialCoord.q, axialCoord.r, -axialCoord.q - axialCoord.r);
	}

	public static int Distance(CubeCoord a, CubeCoord b)
	{
		return (Math.Abs(a.x - b.x) +
			Math.Abs(a.y - b.y) +
			Math.Abs(a.z - b.z)) / 2;
	}
}