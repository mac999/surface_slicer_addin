# rhino-openslicer
Rhino addin tool for generating tool path such as CNC, 3D printer etc. 
Written by Taewook, Kang (laputa99999@gmail.com)

* 2015.4.25 - 0.1
1. Slicing Brep with 1 cutting plane.
2. Slicing with 1 cutting plane and surface.

* 2015.4.23
Requirement definition like below.
1. Slicing funtion between Brep and surface.
2. Extracting the slice point and plane by using cutting plane and the object.
3. This function has parameters such as the object, cutting plane, and slicing interval. 

* 2017.10.10
1. 2.0을 나누어 uv 지점에서 노이즈 편차가 되도록 함.
_interval = 0.1
_noise = 20
randomPoint(pt, 0.1, 20)
{
	maxValue = 1000 * 0.1;
	maxValue = 100;
	maxRandom = 100 * 20 / 100;
	maxRandom = 100 * 0.2;
	maxRandom = 20;
	v1 = 20 / 100;		// 0.2
	v2 = 0 / 100;		// 0.0
	v3 = 10 / 100;		// 0.1

	pt2.X = pt1.X * 0.2;
}

* 2017.11.30 - fixed add. add delpoints



