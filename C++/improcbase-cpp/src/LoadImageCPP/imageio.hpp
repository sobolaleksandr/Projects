#pragma once

#if defined(WIN32) || defined(_WIN32) || defined(__WIN32) && !defined(__CYGWIN__)

#include <Windows.h>
#include <GdiPlus.h>

#endif

#include "imageformats.hpp"

class ImageIO
{
public:

	static ColorFloatImage FileToColorFloatImage(const char* filename, float brightAdjust);
	static ColorByteImage FileToColorByteImage(const char* filename);

	static void ImageToFile(const ColorFloatImage &image, const char *filename);
	static void ImageToFile(const ColorByteImage &image, const char *filename);


#if defined(WIN32) || defined(_WIN32) || defined(__WIN32) && !defined(__CYGWIN__)

private:
	static ColorFloatImage BitmapToColorFloatImage(Gdiplus::Bitmap &B, float brightAdjust);

	static std::unique_ptr<Gdiplus::Bitmap> ImageToBitmap(const ColorFloatImage &image);
	static std::unique_ptr<Gdiplus::Bitmap> ImageToBitmap(const ColorByteImage &image);

#endif
};
