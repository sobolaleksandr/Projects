#pragma once

#pragma pack(push, 1)

struct ColorBytePixel
{
	unsigned char b, g, r, a;

	inline ColorBytePixel()
		: b(0), g(0), r(0), a(0) {}

	inline ColorBytePixel(unsigned char b, unsigned char g, unsigned char r, unsigned char a = 0)
		: b(b), g(g), r(r), a(a) {}

};

struct ColorFloatPixel
{
public:

	float b, g, r, a;

	inline ColorFloatPixel()
		: b(0.0f), g(0.0f), r(0.0f), a(0.0f) {}

	inline ColorFloatPixel(float b, float g, float r, float a = 0.0f)
		: b(b), g(g), r(r), a(a) {}

	inline ColorFloatPixel& operator += (const ColorFloatPixel &other)
	{
		b += other.b;
		g += other.g;
		r += other.r;
		a += other.a;
		return *this;
	}

	inline ColorFloatPixel operator + (const ColorFloatPixel &other) const
	{
		return ColorFloatPixel(b + other.b, g + other.g, r + other.r, a + other.a);
	}

	inline ColorFloatPixel operator * (float q) const
	{
		return ColorFloatPixel(b * q, g * q, r * q, a * q);
	}
};

inline ColorFloatPixel operator * (float q, const ColorFloatPixel &p)
{
	return ColorFloatPixel(p.b * q, p.g * q, p.r * q, p.a * q);
}

#pragma pack(pop)

