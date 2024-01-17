/* Route "" ("d") and "l" sizes of math functions to "f" versions */
#include <math.h>
#undef exp2l

long double exp2l(long double x)
{
    return (long double)exp2f((float)x);
}
