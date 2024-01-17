/* Route "" ("d") and "l" sizes of math functions to "f" versions */
#include <math.h>
#undef fmal

long double fmal(long double x, long double y, long double z)
{
    return (long double)fmaf((float)x, (float)y, (float)z);
}
