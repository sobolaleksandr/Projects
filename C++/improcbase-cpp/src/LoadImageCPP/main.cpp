#if defined(WIN32) || defined(_WIN32) || defined(__WIN32) && !defined(__CYGWIN__)

#include <Windows.h>	// Windows.h must be included before GdiPlus.h
#include <GdiPlus.h>

#pragma comment(lib, "gdiplus.lib")

ULONG_PTR m_gdiplusToken;   // class member

#endif

#include "imageformats.hpp"
#include "imageio.hpp"

void BrightAdjuster(char *inputfilename, char *outputfilename, float brightAdjust)
{
	ColorFloatImage image = ImageIO::FileToColorFloatImage(inputfilename, brightAdjust);
	ImageIO::ImageToFile(image, outputfilename);
}

int main_func(int argc, char* argv[])
{
	if (argc < 4)
		return 0;

	BrightAdjuster(argv[1], argv[2], atof(argv[3]));

	return 0;
}

int main(int argc, char* argv[])
{
#if defined(WIN32) || defined(_WIN32) || defined(__WIN32) && !defined(__CYGWIN__)

	 // InitInstance
    Gdiplus::GdiplusStartupInput gdiplusStartupInput;
    Gdiplus::GdiplusStartup(&m_gdiplusToken, &gdiplusStartupInput, NULL);

	int exit_code;

	try
	{
		exit_code = main_func(argc, argv);
	}
	catch (...)
	{
		exit_code = -1;
	}

	// ExitInstance
    Gdiplus::GdiplusShutdown(m_gdiplusToken);

	return exit_code;
#else
	return main_func(argc, argv);
#endif
}


