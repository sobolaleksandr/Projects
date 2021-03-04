#pragma once

#include <memory>
#include <assert.h>
#include "pixelformats.hpp"

template<typename PixelType>
class ImageBase
{
private:
	std::unique_ptr<PixelType[]> rawdata;
	int width, height;

public:
	inline ImageBase(int Width, int Height)
	{
		assert(Width > 0 && Height > 0);

		this->width = Width;
		this->height = Height;
		this->rawdata.reset(new PixelType[Width * Height]);
	}

	ImageBase(const ImageBase&) = default;
	ImageBase(ImageBase&&) = default;

	ImageBase& operator = (const ImageBase&) = default;
	ImageBase& operator = (ImageBase&&) = default;

	inline PixelType operator() (int x, int y) const
	{
		assert(x >= 0 && x < width && y >= 0 && y < height);
		return rawdata[y * width + x];
	}

	inline PixelType& operator() (int x, int y)
	{
		assert(x >= 0 && x < width && y >= 0 && y < height);
		return rawdata[y * width + x];
	}

	inline int Width() const
	{
		return width;
	}

	inline int Height() const
	{
		return height;
	}

	inline ImageBase<PixelType> Copy() const
	{
		ImageBase<PixelType> res(width, height);

		if (rawdata)
			memcpy(res.rawdata.get(), rawdata.get(), width * height * sizeof(PixelType));

		return res;
	}
};

typedef ImageBase<ColorBytePixel> ColorByteImage;
typedef ImageBase<ColorFloatPixel> ColorFloatImage;
