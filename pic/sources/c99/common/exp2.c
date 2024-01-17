/* Route "" ("d") and "l" sizes of math functions to "f" versions */
#include <math.h>
#undef exp2

double exp2(double x)
{
    return (double)exp2f((float)x);
}
