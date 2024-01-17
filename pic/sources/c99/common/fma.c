/* Route "" ("d") and "l" sizes of math functions to "f" versions */
#include <math.h>
#undef fma

double fma(double x, double y, double z)
{
    return (double)fmaf((float)x, (float)y, (float)z);
}
